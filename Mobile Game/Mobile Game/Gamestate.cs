using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mobile_Game
{
    public interface MainGui
    {
        void update();
        void draw(SpriteBatch spritebatch);
    }
    public interface GuiElement
    {
        void update();
        void draw(SpriteBatch spritebatch);
        int checkID();
    }


    class Gamestate : MainGui
    {
        private List<List<GuiElement>> MainList;
        private List<GuiElement> BackgroundList,MidList,ForegroundList,UiList, StartScreen;
        public List<Enemy> enemylist;
        public List<Big_Enemy> big_enemylist;
        public bool paused, gamestarted, gunshop_active, game_over;
        public int GameWidth, GameHeight;
        Image BG2_1, BG2_1_prop, BG2_2, BG2_3, BG2_4, contbg, Menu_bg, BG1, BG2, BG3, BG4, BG5, BG6, BG7, BG8, BG9, Fence1, Fence2, analogarea, lantern1, lantern2, lantern3, lantern4, lantern5, pausebg;
        Light light1, light2, light3, light4, light5;
        Player player;
        TouchButton analogstick;
        Button B_Button, X_Button, A_Button, cont, settingsbutton, start_button, name_change, moneycheat_button, skipstage_button, exit_button, retry_button, next_level;
        Texture2D[] Walkanim, enemy_Walkanim, big_enemy_Walkanim;
        Gun pistol, hands, rifle, mp7a1;
        GunManager gunmanager;
        Enemy enemy1,enemy2,enemy3,enemy4,enemy5,enemy6,enemy7,enemy8,enemy9,enemy10,enemy11,enemy12,enemy13;
        Texture2D Ammobox;
        ONOFFswitch fps_switch,op2,op3,op4;
        VirtKeyBoard keyboard;
        Song Intro_Music;
        Big_Enemy big_enemy1,big_enemy2,big_enemy3,big_enemy4;
        Gunstore gunstore;
        Scorecounter player1score;
        GameOver endscreen;
        Level current_level, l1_1, l1_2, l1_3, l1_4, l1_5, l1_6, l1_7, l1_8, l1_9, l2_1, l2_2, l2_3, l2_4;

        public Gamestate(Texture2D gun_light, Texture2D shadow, Texture2D[] tele_anim, Action exit, SoundEffect footstep1, SoundEffect footstep2, SoundEffect footstep3, SoundEffect ka_ching, Song gunshop_song, Texture2D gunshop_img, Texture2D gunshop_bg, Song intro_music, Texture2D menu_bg,Texture2D settings, Song gunready, SoundEffect gunfire1, SoundEffect gunfire2, 
            SoundEffect mp7a1_sound, Texture2D fire1, Texture2D fire2, Texture2D ammobox, Texture2D[] punchanim, Texture2D[] enemy_punchanim,
            Texture2D chrtr, Texture2D chrtrw1, Texture2D chrtrw2, Texture2D enemy_chrtr, Texture2D enemy_chrtrw1, Texture2D enemy_chrtrw2, Texture2D fiftyblack, Texture2D whitep, 
            Texture2D pistol_gun_img, Texture2D rifle_gun_img, Texture2D hands_gun_img, Texture2D lantern, Texture2D pistol_icon, 
            Texture2D rifle_icon, Texture2D hands_icon, Texture2D background1, Texture2D background2, Texture2D background3, 
            Texture2D background4, Texture2D background5, Texture2D background6, Texture2D background7, Texture2D background8, 
            Texture2D background9, Texture2D fence, Texture2D licht, Texture2D analogarea_img, Texture2D analogstick_img, 
            Texture2D button_A, Texture2D button_B, Texture2D button_X, Texture2D mp7a1_icon, Texture2D mp7a1_gun, Texture2D big_enemy, Texture2D big_enemy_walk1, Texture2D big_enemy_Walk2,
            Texture2D bg2_1, Texture2D bg2_1_prop, Texture2D bg2_2, Texture2D bg2_3, Texture2D bg2_4)
        {
            this.GameHeight = Game1.GameHeight;
            this.GameWidth = Game1.GameWidth;

            this.StartScreen = new List<GuiElement>();
            this.BackgroundList = new List<GuiElement>();
            this.MidList = new List<GuiElement>();
            this.ForegroundList = new List<GuiElement>();
            this.UiList = new List<GuiElement>();
            this.enemylist = new List<Enemy>();
            this.big_enemylist = new List<Big_Enemy>();
            MainList = new List<List<GuiElement>>();
            MainList.Add(BackgroundList);
            MainList.Add(MidList);
            MainList.Add(ForegroundList);
            MainList.Add(UiList);
            
            this.paused = false;
            this.gamestarted = false;
            this.gunshop_active = false;
            this.game_over = false;

            this.Walkanim = new Texture2D[4];
            this.Walkanim[0] = chrtr;
            this.Walkanim[1] = chrtrw1;
            this.Walkanim[2] = chrtr;
            this.Walkanim[3] = chrtrw2;

            this.enemy_Walkanim = new Texture2D[4];
            this.enemy_Walkanim[0] = enemy_chrtr;
            this.enemy_Walkanim[1] = enemy_chrtrw1;
            this.enemy_Walkanim[2] = enemy_chrtr;
            this.enemy_Walkanim[3] = enemy_chrtrw2;

            this.big_enemy_Walkanim = new Texture2D[4];
            this.big_enemy_Walkanim[0] = big_enemy;
            this.big_enemy_Walkanim[1] = big_enemy_walk1;
            this.big_enemy_Walkanim[2] = big_enemy;
            this.big_enemy_Walkanim[3] = big_enemy_Walk2;

            this.Ammobox = ammobox;

            this.mp7a1 = new Gun(gun_light, mp7a1_sound, whitep, 0.4f, 96, mp7a1_gun, mp7a1_icon, fire1, fire2, (int)(GameWidth / 7.407), (int)(GameWidth / 21.052), (int)(GameWidth / 110), 28, 1.67, 3, 7);
            this.pistol = new Gun(gun_light, gunfire2, whitep, 1, 99, pistol_gun_img, pistol_icon, fire1, fire2, (int)(GameWidth / 7.407), (int)(GameWidth / 21.052), (int)(GameWidth / 110),42.105,2,3,25);
            this.rifle = new Gun(gun_light, gunfire1, whitep, 0.8f, 98, rifle_gun_img, rifle_icon, fire1, fire2, (int)(GameWidth / 7.407), (int)(GameWidth / 21.052), 0,17.02,1.3,3.5,11);
            this.hands = new Gun(gun_light, gunfire1, whitep, 0f, 97, hands_gun_img, hands_icon, fire1, fire2, (int)(GameWidth / 7.407), (int)(GameWidth / 21.052), 0, 17.02, 1.3, 3.5, 11);

            this.lantern1 = new Image(121, lantern, (int)(GameWidth / 10), (int)(GameWidth / 25), (int)(GameWidth / 17), (int)(GameWidth / 4.444));
            this.lantern2 = new Image(122, lantern, (int)(GameWidth / 2.285), (int)(GameWidth / 25), (int)(GameWidth / 17), (int)(GameWidth / 4.444));
            this.lantern3 = new Image(123, lantern, (int)(GameWidth / 1.29), (int)(GameWidth / 25), (int)(GameWidth / 17), (int)(GameWidth / 4.444));
            this.lantern4 = new Image(124, lantern, (int)(GameWidth / 3), (int)(GameWidth / 25), (int)(GameWidth / 17), (int)(GameWidth / 4.444));
            this.lantern5 = new Image(125, lantern, (int)(GameWidth / 1.5), (int)(GameWidth / 25), (int)(GameWidth / 17), (int)(GameWidth / 4.444));
            
            this.light1 = new Light(131, licht, 0, (int)(GameWidth / 25), (int)(GameWidth / 4.04), (int)(GameWidth / 2.72));
            this.light2 = new Light(132, licht, (int)(GameWidth / 2.96), (int)(GameWidth / 25), (int)(GameWidth / 4.04), (int)(GameWidth / 2.72));
            this.light3 = new Light(133, licht, (int)(GameWidth / 1.48), (int)(GameWidth / 25), (int)(GameWidth / 4.04), (int)(GameWidth / 2.72));
            this.light4 = new Light(134, licht, (int)(GameWidth / 4.333), (int)(GameWidth / 25), (int)(GameWidth / 4.04), (int)(GameWidth / 2.72));
            this.light5 = new Light(135, licht, (int)(GameWidth / 1.764), (int)(GameWidth / 25), (int)(GameWidth / 4.04), (int)(GameWidth / 2.72));

            this.gunmanager = new GunManager(104, gunready, fiftyblack, this.hands, this.pistol, this.rifle, this.mp7a1);
            this.BG1 = new Image(1, background1, 0, 0, GameWidth, GameHeight);
            this.BG2 = new Image(1, background2, 0, 0, GameWidth, GameHeight);
            this.BG3 = new Image(1, background3, 0, 0, GameWidth, GameHeight);
            this.BG4 = new Image(1, background4, 0, 0, GameWidth, GameHeight);
            this.BG5 = new Image(1, background5, 0, 0, GameWidth, GameHeight);
            this.BG6 = new Image(1, background6, 0, 0, GameWidth, GameHeight);
            this.BG7 = new Image(1, background7, 0, 0, GameWidth, GameHeight);
            this.BG8 = new Image(1, background8, 0, 0, GameWidth, GameHeight);
            this.BG9 = new Image(1, background9, 0, 0, GameWidth, GameHeight);

            this.BG2_1 = new Image(0, bg2_1, 0, 0, GameWidth, GameHeight);
            this.BG2_1_prop = new Image(0, bg2_1_prop, (int)(Game1.GameWidth/1.639), (int)(Game1.GameWidth / 10.3), (int)(Game1.GameWidth / 2.564), (int)(Game1.GameWidth / 3.125));
            this.BG2_2 = new Image(0, bg2_2, 0, 0, GameWidth, GameHeight);
            this.BG2_3 = new Image(0, bg2_3, 0, 0, GameWidth, GameHeight);
            this.BG2_4 = new Image(0, bg2_4, 0, 0, GameWidth, GameHeight);

            this.Fence1 = new Image(1, fence, 0, (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 15.686));
            this.Fence2 = new Image(1, fence, (int)(GameWidth/2), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 15.686));

            this.analogarea = new Image(4, analogarea_img, (int)(GameWidth / 80), (int)(GameWidth / 2.6666), (int)(GameWidth / 5.333333), (int)(GameWidth / 5.333333));
            this.analogstick = new TouchButton(5, analogstick_img, (int)(GameWidth / 13.33333), (int)(GameWidth / 2.2857), (int)(GameWidth / 16), (int)(GameWidth / 16));
            this.player = new Player( 6, shadow, footstep1, footstep2, footstep3, chrtr, this.Walkanim, punchanim, whitep, analogstick, gunmanager, (int)(GameWidth / 16), (int)(GameWidth / 3.2), (int)(GameWidth / 12.5), (int)(GameWidth / 10));
            this.A_Button = new Button(20, "",button_A, (int)(GameWidth / 1.739), (int)(GameWidth / 2.2), (int)(GameWidth / 12.307), (int)(GameWidth / 12.307), () => { });
            this.B_Button = new Button(21, "", button_B, (int)(GameWidth / 1.428), (int)(GameWidth / 2.329), (int)(GameWidth / 12.307), (int)(GameWidth / 12.307), () => { player.Attack(); });
            this.X_Button = new Button(22, "", button_X, (int)(GameWidth / 1.212), (int)(GameWidth / 2.474), (int)(GameWidth / 12.307), (int)(GameWidth / 12.307), () => { player.jumpbool = true; });
            this.moneycheat_button = new Button(0, "add 1000 score", fiftyblack, (int)(GameWidth / 2), (int)(GameWidth / 5.333), (int)(GameWidth / 4), (int)(GameWidth / 16), () => { if (!this.moneycheat_button.active) { player.score += 1000; }; });
            this.skipstage_button = new Button(0, "Skip stage", fiftyblack, (int)(GameWidth / 2), (int)(GameWidth / 4), (int)(GameWidth / 4), (int)(GameWidth / 16), () => { if (!this.skipstage_button.active && this.current_level.Next != null) { this.current_level = this.current_level.NextLevel(); this.current_level.OnEnter(); }; });

            this.enemy1 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.1));
            this.enemy2 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
            this.enemy3 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.3));
            this.enemy4 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.35), (int)(GameWidth / 3.1));
            this.enemy5 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
            this.enemy6 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1), (int)(GameWidth / 3.3));
            this.enemy7 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.2));
            this.enemy8 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.1));
            this.enemy9 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.45), (int)(GameWidth / 3.2));
            this.enemy10 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.3), (int)(GameWidth / 3.3));
            this.enemy11 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.1));
            this.enemy12 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.1), (int)(GameWidth / 3.2));
            this.enemy13 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.15), (int)(GameWidth / 3.3));

            this.big_enemy1 = new Big_Enemy(0, shadow, big_enemy, whitep,this.big_enemy_Walkanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.2));
            this.big_enemy2 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
            this.big_enemy3 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.4), (int)(GameWidth / 3.2));
            this.big_enemy4 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.3), (int)(GameWidth / 3.2));

            this.l1_1 = new Level(BG1, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { this.AddToFront(light1); this.AddToFront(light2); this.AddToFront(light3); },
                () => { },
                () => { this.AddToBack(lantern1); this.AddToBack(lantern2); this.AddToBack(lantern3); this.AddToBack(gunstore); },
                () => {} );

            this.l1_2 = new Level(BG2, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy1 },
                new List<Big_Enemy> { },
                () => { this.AddToFront(light4); this.AddToFront(light5); }, 
                () => { }, 
                () => { this.AddToBack(lantern4); this.AddToBack(lantern5); },
                () => {} );

            this.l1_3 = new Level(BG3, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy2, enemy3 },
                new List<Big_Enemy> { },
                () => { }, 
                () => { }, 
                () => { },
                () => {} );

            this.l1_4 = new Level(BG4, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy4, enemy5, enemy6 },
                new List<Big_Enemy> { },
                () => { this.AddToFront(Fence2); }, 
                () => { }, 
                () => { this.AddToBack(gunstore); },
                () => {} );

            this.l1_5 = new Level(BG5, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { big_enemy1 },
                () => { this.AddToFront(Fence1); this.AddToFront(Fence2); }, 
                () => { }, 
                () => { },
                () => {} );

            this.l1_6 = new Level(BG6, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy7, enemy8, enemy9, enemy10 },
                new List<Big_Enemy> { },
                () => { this.AddToFront(Fence1); this.AddToFront(Fence2); }, 
                () => { }, 
                () => { },
                () => {} );

            this.l1_7 = new Level(BG7, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy11, enemy12 },
                new List<Big_Enemy> { big_enemy2 },
                () => { }, 
                () => { }, 
                () => { this.AddToBack(gunstore); },
                () => {} );

            this.l1_8 = new Level(BG8, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { enemy13 },
                new List<Big_Enemy> {  big_enemy3, big_enemy4 },
                () => { }, 
                () => { }, 
                () => { },
                () => {} );

            this.l1_9 = new Level(BG9, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { }, 
                () => { }, 
                () => { }, 
                () => { if (this.player.x > (int)(Game1.GameWidth / 2)) { this.game_over = true; } });

            this.l2_1 = new Level(BG2_1, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { this.AddToFront(this.BG2_1_prop); this.AddToFront(light1); this.AddToFront(light2);},
                () => { },
                () => { this.AddToBack(lantern1); this.AddToBack(lantern2); },
                () => {});

            this.l2_2 = new Level(BG2_2, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { },
                () => { },
                () => { },
                () => { });

            this.l2_3 = new Level(BG2_3, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { },
                () => { },
                () => { },
                () => { });

            this.l2_4 = new Level(BG2_4, null, null, BackgroundList, MidList, ForegroundList, UiList, enemylist, big_enemylist,
                new List<Enemy> { },
                new List<Big_Enemy> { },
                () => { },
                () => { },
                () => { },
                () => { });

            this.current_level = l1_1;

            this.l1_1.Next = l1_2; this.l1_2.Prev = l1_1;
            this.l1_2.Next = l1_3; this.l1_3.Prev = l1_2;
            this.l1_3.Next = l1_4; this.l1_4.Prev = l1_3;
            this.l1_4.Next = l1_5; this.l1_5.Prev = l1_4;
            this.l1_5.Next = l1_6; this.l1_6.Prev = l1_5;
            this.l1_6.Next = l1_7; this.l1_7.Prev = l1_6;
            this.l1_7.Next = l1_8; this.l1_8.Prev = l1_7;
            this.l1_8.Next = l1_9; this.l1_9.Prev = l1_8;

            this.l2_1.Next = l2_2; this.l2_2.Prev = l2_1;
            this.l2_2.Next = l2_3; this.l2_3.Prev = l2_2;
            this.l2_3.Next = l2_4; this.l2_4.Prev = l2_3;
            


            this.player1score = new Scorecounter((int)(Game1.GameWidth/80), (int)(Game1.GameWidth / 16), player);
            this.keyboard = new VirtKeyBoard(fiftyblack, "enter name", player, () => { this.StartScreen.Add(this.start_button); this.StartScreen.Add(this.name_change); this.StartScreen.Remove(keyboard); });
            this.settingsbutton = new Button(1, "", settings, 0, 0, (int)(GameWidth/16), (int)(GameWidth / 16), () => { this.paused = true; });
            this.contbg = new Image(1,fiftyblack,0, (int)(GameWidth / 16), GameWidth, (int)(GameWidth / 2.105));
            this.Menu_bg = new Image(1, menu_bg, 0, 0, Game1.GameWidth, Game1.GameHeight);
            this.start_button = new Button(1, "START",fiftyblack,GameWidth/2 - (int)(GameWidth / 6.5), GameHeight/2, (int)(GameWidth / 3.8), (int)(GameWidth / 13), () => { this.gamestarted = true; this.paused = false; this.gunshop_active = false; MediaPlayer.Stop(); });
            this.name_change = new Button(1, "CHANGE NAME", fiftyblack, GameWidth / 2 - (int)(GameWidth / 6.5), GameHeight / 2 + (int)(GameWidth / 9), (int)(GameWidth / 3.8), (int)(GameWidth / 13), () => { this.StartScreen.Remove(this.start_button); this.StartScreen.Remove(this.name_change); this.StartScreen.Add(keyboard); Thread.Sleep(120); });
            this.gunstore = new Gunstore(gunshop_img, ka_ching, gunshop_song, fiftyblack,gunshop_bg, mp7a1_gun, rifle_gun_img, pistol_gun_img, whitep, (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth/8), player, A_Button,() => { this.gunshop_active = true; this.gunmanager.currentgun = this.gunmanager.Gun1; this.player1score.y = (int)(Game1.GameWidth / 1.95); }, () => { this.gunshop_active = false; this.player1score.y = (int)(Game1.GameWidth / 16); });

            this.fps_switch = new ONOFFswitch("Draw FPS", (int)(GameWidth / 16), (int)(GameWidth / 5.333),whitep,analogstick_img);
            this.op2 = new ONOFFswitch("GodMode", (int)(GameWidth / 16), (int)(GameWidth / 4), whitep, analogstick_img);
            this.op3 = new ONOFFswitch("Skip Stage", (int)(GameWidth / 16), (int)(GameWidth / 3.2), whitep, analogstick_img);
            this.op4 = new ONOFFswitch("Option", (int)(GameWidth / 16), (int)(GameWidth / 2.666), whitep, analogstick_img);

            this.retry_button = new Button(1, "Retry", fiftyblack, GameWidth / 2 - (int)(GameWidth / 6.5), GameHeight / 2 + (int)(GameWidth / 12), (int)(GameWidth / 3.8), (int)(GameWidth / 13), () => {

                this.current_level = l1_1;

                this.MidList.Remove(enemy1); this.enemylist.Remove(enemy1);
                this.MidList.Remove(enemy2); this.enemylist.Remove(enemy2);
                this.MidList.Remove(enemy3); this.enemylist.Remove(enemy3);
                this.MidList.Remove(enemy4); this.enemylist.Remove(enemy4);
                this.MidList.Remove(enemy5); this.enemylist.Remove(enemy5);
                this.MidList.Remove(enemy6); this.enemylist.Remove(enemy6);
                this.MidList.Remove(enemy7); this.enemylist.Remove(enemy7);
                this.MidList.Remove(enemy8); this.enemylist.Remove(enemy8);
                this.MidList.Remove(enemy9); this.enemylist.Remove(enemy9);
                this.MidList.Remove(enemy10); this.enemylist.Remove(enemy10);
                this.MidList.Remove(enemy11); this.enemylist.Remove(enemy11);
                this.MidList.Remove(enemy12); this.enemylist.Remove(enemy12);
                this.MidList.Remove(enemy13); this.enemylist.Remove(enemy13);

                this.MidList.Remove(big_enemy1); this.big_enemylist.Remove(big_enemy1);
                this.MidList.Remove(big_enemy2); this.big_enemylist.Remove(big_enemy2);
                this.MidList.Remove(big_enemy3); this.big_enemylist.Remove(big_enemy3);
                this.MidList.Remove(big_enemy4); this.big_enemylist.Remove(big_enemy4);

                this.BackgroundList.RemoveRange(0, BackgroundList.Count);
                this.MidList.RemoveRange(0, MidList.Count);
                this.UiList.RemoveRange(0, UiList.Count);
                this.ForegroundList.RemoveRange(0, ForegroundList.Count);

                this.AddToBack(BG1);
                this.AddToBack(lantern1);
                this.AddToBack(lantern2);
                this.AddToBack(lantern3);
                this.AddToFront(light1);
                this.AddToFront(light2);
                this.AddToFront(light3);
                this.AddToBack(gunstore);
                this.AddToUI(player1score);
                this.AddToUI(analogarea);
                this.AddToUI(analogstick);
                this.AddToMid(player);
                this.AddToUI(A_Button);
                this.AddToUI(B_Button);
                this.AddToUI(X_Button);
                this.AddToMid(gunmanager);
                this.AddToUI(settingsbutton);

                this.enemy1 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.1));
                this.enemy2 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
                this.enemy3 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.3));
                this.enemy4 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.35), (int)(GameWidth / 3.1));
                this.enemy5 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
                this.enemy6 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1), (int)(GameWidth / 3.3));
                this.enemy7 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.2));
                this.enemy8 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.1));
                this.enemy9 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.45), (int)(GameWidth / 3.2));
                this.enemy10 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.3), (int)(GameWidth / 3.3));
                this.enemy11 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.1));
                this.enemy12 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.1), (int)(GameWidth / 3.2));
                this.enemy13 = new Enemy(0, shadow, enemy_chrtr, whitep, this.enemy_Walkanim, enemy_punchanim, (int)(GameWidth / 1.15), (int)(GameWidth / 3.3));

                this.big_enemy1 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.5), (int)(GameWidth / 3.2));
                this.big_enemy2 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.2), (int)(GameWidth / 3.2));
                this.big_enemy3 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.4), (int)(GameWidth / 3.2));
                this.big_enemy4 = new Big_Enemy(0, shadow, big_enemy, whitep, this.big_enemy_Walkanim, (int)(GameWidth / 1.3), (int)(GameWidth / 3.2));

                this.player.x = (int)(Game1.GameWidth/40);
                this.player.score = 500;
                this.player.health = 100;

                this.gunmanager.gunlist.RemoveAll(gun => gun == gun);
                this.gunmanager.gunlist.Add(this.gunmanager.Gun1);
                this.gunmanager.currentgun = this.gunmanager.Gun1;

                this.endscreen.reset();
                this.gamestarted = false;
                this.game_over = false;
            });
            this.next_level = new Button(1, "Next Level", fiftyblack, GameWidth / 2 - (int)(GameWidth / 6.5), GameHeight / 2 + (int)(GameWidth / 12), (int)(GameWidth / 3.8), (int)(GameWidth / 13), () => { });
            this.exit_button = new Button(1, "Exit Game", fiftyblack, GameWidth / 2 - (int)(GameWidth / 6.5), GameHeight / 2 + (int)(GameWidth / 6), (int)(GameWidth / 3.8), (int)(GameWidth / 13), () => { exit(); });
            this.endscreen = new GameOver(player,exit_button,next_level,fiftyblack,tele_anim, () => { this.player.x = (int)(Game1.GameWidth / 8); this.player.y = (int)(Game1.GameHeight / 2); this.game_over = false; this.current_level = l2_1; this.current_level.OnEnter(); });
            this.cont = new Button(1, "BACK", fiftyblack, (int)(GameWidth / 80), (int)(GameWidth / 13.333), (int)(GameWidth / 4.444), (int)(GameWidth / 16), () => { this.paused = false; });
            this.Intro_Music = intro_music;
            

            this.StartScreen.Add(Menu_bg);
            this.StartScreen.Add(start_button);
            this.StartScreen.Add(name_change);

            this.current_level.OnEnter();
            this.AddToUI(player1score);
            this.AddToUI(analogarea);
            this.AddToUI(analogstick);
            this.AddToMid(player);
            this.AddToUI(A_Button);
            this.AddToUI(B_Button);
            this.AddToUI(X_Button);
            this.AddToUI(settingsbutton);

        }

        public void AddToBack(GuiElement item)
        {
            BackgroundList.Add(item);
        }

        public void AddToMid(GuiElement item)
        {
            MidList.Add(item);
        }

        public void AddToFront(GuiElement item)
        {
            ForegroundList.Add(item);
        }

        public void AddToUI(GuiElement item)
        {
            UiList.Add(item);
        }
        


        public void draw(SpriteBatch spritebatch)
        {
            if (this.gamestarted == false)
            {
                foreach (GuiElement item in StartScreen.ToList())
                {
                    item.draw(spritebatch);
                }
            }
            else
            {
                foreach (List<GuiElement> list in MainList)
                {
                    foreach (GuiElement item in list.ToList())
                    {
                        item.draw(spritebatch);

                        if (fps_switch.active)
                        {
                            Game1.drawfps = true;
                        }
                        else {Game1.drawfps = false; }

                        if (op2.active && player.health < 100)
                        {
                            player.health += 1;
                        }
                        else { }
                    }
                }
                if (this.paused)
                {
                    this.contbg.draw(spritebatch);
                    this.cont.draw(spritebatch);
                    this.cont.draw(spritebatch);
                    this.fps_switch.draw(spritebatch);
                    this.op2.draw(spritebatch);
                    this.op3.draw(spritebatch);
                    this.op4.draw(spritebatch);
                    this.moneycheat_button.draw(spritebatch);
                    skipstage_button.draw(spritebatch);
                    this.retry_button.draw(spritebatch);
                    this.exit_button.draw(spritebatch);


                }
                else if (this.game_over)
                {
                    this.endscreen.draw(spritebatch);
                }
                else if (gunshop_active)
                {
                    this.gunstore.draw(spritebatch);
                    this.gunmanager.draw(spritebatch);
                    this.player1score.draw(spritebatch);
                }
                else if (player.health <= 0)
                {
                    this.contbg.draw(spritebatch);
                    spritebatch.DrawString(Game1.Font, "GAME OVER", new Vector2(GameWidth / 2, GameHeight / 2), Color.White, 0, Game1.Font.MeasureString("GAME OVER") / 2, (Game1.GameWidth / 400f) * 1.5f, SpriteEffects.None, 0.5f);
                    spritebatch.DrawString(Game1.Font, "GAME OVER", new Vector2(GameWidth / 2, GameHeight / 2), Color.White, 0, Game1.Font.MeasureString("GAME OVER") / 2, (Game1.GameWidth / 400f) * 1.5f, SpriteEffects.None, 0.5f);
                    this.retry_button.draw(spritebatch);
                    this.exit_button.draw(spritebatch);
                }

            }
            
        }

        public void update()
        {
            if (this.gamestarted == false)
            {
                foreach (GuiElement item in StartScreen.ToList())
                {
                    item.update();
                    
                }
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Volume = 0.8f;
                    MediaPlayer.Play(this.Intro_Music);
                }

            }
            else
            {
                if (this.paused)
                {
                    this.contbg.update();
                    this.cont.update();
                    this.fps_switch.update();
                    this.op2.update();
                    this.op3.update();
                    this.op4.update();
                    this.moneycheat_button.update();
                    if (this.moneycheat_button.y == this.moneycheat_button.y_released) { this.moneycheat_button.active = false; }
                    this.skipstage_button.update();
                    if (this.skipstage_button.y == this.skipstage_button.y_released) { this.skipstage_button.active = false; }
                    this.retry_button.update();
                    this.exit_button.update();
                }
                else if (this.gunshop_active)
                {
                    this.gunstore.update();
                    this.gunmanager.update();
                }
                else if (player.health < 0)
                {
                    this.retry_button.update();
                    this.exit_button.update();
                }
                else if (this.game_over)
                {
                    this.endscreen.update();
                }
                else
                {
                    this.current_level.update();
                    foreach (List<GuiElement> list in MainList)
                    {
                        foreach (GuiElement item in list.ToList())
                        {
                            item.update();
                        }
                    }
                }
            }
            

            this.levelchange();
            this.enemiesAction();
        }

        public void levelchange()
        {
            if (player.x < 10)
            {
                if (this.current_level.PrevLevel() != null)
                {
                    this.current_level = this.current_level.PrevLevel();
                    this.current_level.OnEnter();
                    this.player.x = GameWidth - (player.width + 2);
                }
                
            }
            if (player.x > GameWidth - (player.width + 1))
            {
                if (this.current_level.NextLevel() != null)
                {
                    this.current_level = this.current_level.NextLevel();
                    this.current_level.OnEnter();
                    this.player.x = ((int)(GameWidth / 80) + 11);
                }
                
            }
        }

        public void enemiesAction()
        {
            foreach (Enemy foe in enemylist.ToList())
            {
                foe.active(this.player, ForegroundList);
                if (foe.y > GameHeight)
                {
                    enemylist.Remove(foe);
                    MidList.Remove(foe);
                }

                if (foe.vel == 22)
                {
                    current_level.AddProp(new AmmoBox(50,140,Ammobox,(int)(foe.x + foe.width/2),(int)(foe.y + foe.height/1.5), player, MidList, current_level));
                }

                if (foe.x < player.x + player.width && foe.x + foe.width > player.x && foe.punchloop == 40)
                {
                    int dmg = Game1.rnd.Next(7,13);
                    player.health -= dmg;
                    player.coleur = Color.Red;
                    this.ForegroundList.Add(new FloatingText(Convert.ToString("-"+dmg), player.x + (player.width / 2), player.y + (player.height / 2),this.ForegroundList));
                }
                
            }
            foreach (Big_Enemy foe in big_enemylist.ToList())
            {
                foe.active(this.player, ForegroundList);
                if (foe.y > GameHeight)
                {
                    big_enemylist.Remove(foe);
                    MidList.Remove(foe);
                }

                if (foe.vel == 22)
                {
                    current_level.AddProp(new AmmoBox(400,600,Ammobox, (int)(foe.x + foe.width / 2), (int)(foe.y + foe.height / 1.5), player, MidList, current_level));
                }

                if (foe.x < player.x + player.width && foe.x + foe.width > player.x && foe.punchloop == 40)
                {
                    int dmg = Game1.rnd.Next(7, 13);
                    player.health -= dmg;
                    player.coleur = Color.Red;
                    this.ForegroundList.Add(new FloatingText(Convert.ToString("-" + dmg), player.x + (player.width / 2), player.y + (player.height / 2), this.ForegroundList));
                }

            }

        }
    }
}