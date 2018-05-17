using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mobile_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        Gamestate gamestate, currentview;
        Texture2D gun_light, shadow, gunshop_img, gunshop_bg,Fire1, Fire2, ammobox, chrtr, chrtrw1, chrtrw2, enemy_chrtr, enemy_chrtrw1, enemy_chrtrw2, fiftyblack, whitep, pistol_gun_img, 
            rifle_gun_img, hands_gun_img, lantern, pistol_icon, rifle_icon, hands_icon,
            background1, background2, background3, background4, background5, background6, background7, 
            background8, background9, fence, licht, analogarea, analogstick_img, button_A, button_B, button_X, 
            setting, mp7a1_icon, mp7a1_gun, menu_bg, big_enemy_chrtr, big_enemy_chrtrw1, big_enemy_chrtrw2,
            bg2_1, bg2_1_roof ,bg2_2, bg2_3, bg2_4;
        public static SpriteFont Font;
        Texture2D[] punchanim, enemy_punchanim, teleport_anim;
        public static Random rnd;
        public static int GameHeight;
        public static int GameWidth;
        public static bool drawfps;
        double cooldown;
        Action Go;
        SoundEffect gunfire1, gunfire2, mp7a1_sound, ka_ching, footstep1, footstep2, footstep3;
        Song gunready, intro_sound, gunshop_song;
        

        public Game1(Action go)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GameHeight = this.Window.ClientBounds.Height;
            GameWidth = (int)(GameHeight * 1.77777777778);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GameWidth;
            graphics.PreferredBackBufferHeight = GameHeight;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            this.cooldown = 0.016f;
            this.Go = go;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Font");
            rnd = new Random();

            gun_light = Content.Load<Texture2D>("gun_light");
            shadow = Content.Load<Texture2D>("shadow");
            gunshop_bg = Content.Load<Texture2D>("gunshop_bg");
            gunshop_img = Content.Load<Texture2D>("gunshop_img");
            intro_sound = Content.Load<Song>("beat");
            gunshop_song = Content.Load<Song>("elevator_music");
            ka_ching = Content.Load<SoundEffect>("ka_ching");
            footstep1 = Content.Load<SoundEffect>("Footsteps1");
            footstep2 = Content.Load<SoundEffect>("Footsteps2");
            footstep3 = Content.Load<SoundEffect>("Footsteps3");
            mp7a1_sound = Content.Load<SoundEffect>("mp7a1");
            mp7a1_icon = Content.Load<Texture2D>("mp7a1_icon");
            mp7a1_gun = Content.Load<Texture2D>("mp7a1_gun");
            setting = Content.Load<Texture2D>("settings");
            gunready = Content.Load<Song>("gunready");
            gunfire1 = Content.Load<SoundEffect>("gunshot1");
            gunfire2 = Content.Load<SoundEffect>("gunshot2");
            Fire1 = Content.Load<Texture2D>("Fire1");
            Fire2 = Content.Load<Texture2D>("Fire2");
            ammobox = Content.Load<Texture2D>("ammobox");
            enemy_chrtr = Content.Load<Texture2D>("enemy");
            enemy_chrtrw1 = Content.Load<Texture2D>("enemy_walk1");
            enemy_chrtrw2 = Content.Load<Texture2D>("enemy_walk2");
            chrtr = Content.Load<Texture2D>("character");
            chrtrw1 = Content.Load<Texture2D>("character_walk1");
            chrtrw2 = Content.Load<Texture2D>("character_walk2");
            fiftyblack = Content.Load<Texture2D>("50black");
            whitep = Content.Load<Texture2D>("whitepixel");
            pistol_gun_img = Content.Load<Texture2D>("pistol_gun");
            rifle_gun_img = Content.Load<Texture2D>("rifle_gun");
            hands_gun_img = Content.Load<Texture2D>("bat_gun");
            pistol_icon = Content.Load<Texture2D>("pistol");
            rifle_icon = Content.Load<Texture2D>("rifle");
            hands_icon = Content.Load<Texture2D>("bat");
            lantern = Content.Load<Texture2D>("lantern");
            background1 = Content.Load<Texture2D>("bg");
            background2 = Content.Load<Texture2D>("BG2");
            background3 = Content.Load<Texture2D>("BG3");
            background4 = Content.Load<Texture2D>("BG4");
            background5 = Content.Load<Texture2D>("BG5");
            background6 = Content.Load<Texture2D>("BG6");
            background7 = Content.Load<Texture2D>("BG7");
            background8 = Content.Load<Texture2D>("BG8");
            background9 = Content.Load<Texture2D>("BG9");
            fence = Content.Load<Texture2D>("fence");
            licht = Content.Load<Texture2D>("light");
            analogarea = Content.Load<Texture2D>("padarea");
            analogstick_img = Content.Load<Texture2D>("pad");
            button_A = Content.Load<Texture2D>("AButton");
            button_B = Content.Load<Texture2D>("BButton");
            button_X = Content.Load<Texture2D>("XButton");
            menu_bg = Content.Load<Texture2D>("menu_background");
            big_enemy_chrtr = Content.Load<Texture2D>("big_enemy");
            big_enemy_chrtrw1 = Content.Load<Texture2D>("big_enemy_walk1");
            big_enemy_chrtrw2 = Content.Load<Texture2D>("big_enemy_walk2");
            bg2_1 = Content.Load<Texture2D>("bg2_1");
            bg2_1_roof = Content.Load<Texture2D>("bg2_1_prop");
            bg2_2 = Content.Load<Texture2D>("bg2_2");
            bg2_3 = Content.Load<Texture2D>("bg2_3");
            bg2_4 = Content.Load<Texture2D>("bg2_4");

            punchanim = new Texture2D[6];
            for (int i = 0; i < 6; ++i) { punchanim[i] = Content.Load<Texture2D>(("character_punch" + Convert.ToString(i + 1))); }
            enemy_punchanim = new Texture2D[6];
            for (int i = 0; i < 6; ++i) { enemy_punchanim[i] = Content.Load<Texture2D>(("enemy_punch" + Convert.ToString(i + 1))); }
            teleport_anim = new Texture2D[11];
            for (int t = 0; t < 11; ++t) { teleport_anim[t] = Content.Load<Texture2D>("teleport" + Convert.ToString(t + 1)); }


            gamestate = new Gamestate(gun_light, shadow, teleport_anim, () => { Exit(); }, footstep1, footstep2, footstep3, ka_ching, gunshop_song, gunshop_img, gunshop_bg, intro_sound, menu_bg,setting, 
                gunready, gunfire1, gunfire2, mp7a1_sound, Fire1, Fire2, ammobox,punchanim, enemy_punchanim, chrtr, chrtrw1, chrtrw2, enemy_chrtr, enemy_chrtrw1, enemy_chrtrw2, fiftyblack, 
                whitep, pistol_gun_img, rifle_gun_img, hands_gun_img, lantern, 
                pistol_icon, rifle_icon, hands_icon, background1, background2, background3, background4, background5, background6, background7, background8, background9, fence, licht, analogarea, 
                analogstick_img, button_A, button_B, button_X, mp7a1_icon,mp7a1_gun,big_enemy_chrtr,big_enemy_chrtrw1,big_enemy_chrtrw2, bg2_1, bg2_1_roof, bg2_2, bg2_3, bg2_4);



            currentview = gamestate;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }


            this.cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (this.cooldown <= 0)
            {
                currentview.update();
                this.cooldown = 0.016f;
                this.Go();
            }

            base.Update(gameTime);
        }
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            currentview.draw(spriteBatch);
            if (drawfps)
            {
                spriteBatch.DrawString(Game1.Font, Convert.ToString("FPS: " + Math.Ceiling(1 / gameTime.ElapsedGameTime.TotalSeconds)), new Vector2((int)(GameWidth / 8), 0), Color.Aqua, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
