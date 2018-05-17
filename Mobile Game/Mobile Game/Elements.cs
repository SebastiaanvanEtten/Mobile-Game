using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mobile_Game
{
    public class TouchButton : GuiElement
    {
        Texture2D texture;
        TouchCollection touchCollection;
        public int ID, x, y, width, height;
        public TouchButton(int ID, Texture2D image, int x, int y, int width, int height)
        {
            this.texture = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
        }

        public void update()
        {
            this.x = (int)(Game1.GameWidth / 13.33333);
            this.y = (int)(Game1.GameWidth / 2.2857);

            touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {

                if (tl.Position.X < (int)(Game1.GameWidth / 2.666) && tl.Position.Y > (int)(Game1.GameWidth / 8))
                {
                    this.x = (int)tl.Position.X - (width / 2);
                    this.y = (int)tl.Position.Y - (height / 2);
                }

                if (this.x > (int)(Game1.GameWidth / 7.2727)) { this.x = (int)(Game1.GameWidth / 7.2727); }
                if (this.x < (int)(Game1.GameWidth / 80)) { this.x = (int)(Game1.GameWidth / 80); }
                if (this.y < (int)(Game1.GameWidth / 2.66666)) { this.y = (int)(Game1.GameWidth / 2.66666); }
                if (this.y > (int)(Game1.GameWidth / 2)) { this.y = (int)(Game1.GameWidth / 2); }
            }
        }
    }

    public class Level : GuiElement
    {
        public Level Prev { get; set; }
        public Level Next { get; set; }
        List<GuiElement> Backgroundlist { get; set; }
        List<GuiElement> Midgroundlist { get; set; }
        List<GuiElement> Foregroundlist { get; set; }
        List<GuiElement> Uilist { get; set; }
        List<Enemy> Enemylist { get; set; }
        List<Big_Enemy> Big_enemylist { get; set; }
        List<Enemy> Enemys { get; set; }
        List<Big_Enemy> Big_Enemys { get; set; }
        List<GuiElement> Props { get; set; }
        Image Backgroundimage { get; set; }
        Action AddToFront { get; set; }
        Action AddToMid { get; set; }
        Action AddToBack { get; set; }
        Action ActiveAction { get; set; }



        public Level(Image backgroundimage, Level previouslevel,Level nextlevel,List<GuiElement> backgroundlist,List<GuiElement> midgroundlist,
        List<GuiElement> foregroundlist,List<GuiElement> uilist,List<Enemy> enemylist,List<Big_Enemy> Bigenemylist, List<Enemy> enemys, List<Big_Enemy> big_enemys, 
        Action Addtofront, Action Addtomid, Action Addtoback, Action activeaction)
        {
            this.Prev = previouslevel;
            this.Next = nextlevel;
            this.Backgroundlist = backgroundlist;
            this.Midgroundlist = midgroundlist;
            this.Foregroundlist = foregroundlist;
            this.Uilist= uilist;
            this.Enemylist = enemylist;
            this.Big_enemylist = Bigenemylist;
            this.Backgroundimage = backgroundimage;
            this.AddToBack = Addtoback;
            this.AddToMid = Addtomid;
            this.AddToFront = Addtofront;
            this.Enemys = enemys;
            this.Big_Enemys = big_enemys;
            this.ActiveAction = activeaction;
            this.Props = new List<GuiElement>();
        }

        public void AddProp(GuiElement item)
        {
            this.Props.Add(item);
            this.Midgroundlist.Add(item);
        }

        public void RemoveProp(GuiElement item)
        {
            this.Props.Remove(item);
            this.Midgroundlist.Remove(item);
        }

        public void OnEnter()
        {

            this.Backgroundlist.RemoveRange(0, Backgroundlist.Count);
            this.Foregroundlist.RemoveRange(0, Foregroundlist.Count);

            this.Backgroundlist.Add(this.Backgroundimage);
            
            this.AddToBack();
            this.AddToMid();
            this.AddToFront();
            
            foreach (GuiElement item in Props.ToList())
            {
                this.Midgroundlist.Add(item);
            }
            

            foreach (Enemy enemy in this.Enemys.ToList())
            {
                if (enemy.health > 0)
                {
                    this.Midgroundlist.Add(enemy);
                    this.Enemylist.Add(enemy);
                }
                
            }
            foreach (Big_Enemy bigenemy in this.Big_Enemys.ToList())
            {
                if (bigenemy.health > 0)
                {
                    this.Midgroundlist.Add(bigenemy);
                    this.Big_enemylist.Add(bigenemy);
                }
            }
        }

        public Level NextLevel()
        {
            this.RemoveAllCurrentEnemys();
            this.RemoveAllProps();
            return this.Next;
        }

        public Level PrevLevel()
        {
            this.RemoveAllCurrentEnemys();
            this.RemoveAllProps();
            return this.Prev;
        }

        public void update()
        {
            ActiveAction();
        }

        public void draw(SpriteBatch spritebatch)
        {

        }

        public void RemoveAllProps()
        {
            foreach (GuiElement item in Props.ToList())
            {
                this.Midgroundlist.Remove(item);
            }
        }

        public void RemoveAllCurrentEnemys()
        {
            foreach (Enemy enemy in this.Enemylist.ToList())
            {
                this.Midgroundlist.Remove(enemy);
                this.Enemylist.Remove(enemy);

            }
            foreach (Big_Enemy big_enemy in this.Big_enemylist.ToList())
            {
                this.Midgroundlist.Remove(big_enemy);
                this.Big_enemylist.Remove(big_enemy);
            }
        }
        public int checkID()
        {
            return 0;
        }
    }

    public class VirtKeyBoard : GuiElement
    {
        public class toets : GuiElement
        {
            Texture2D texture;
            TouchCollection touchCollection;
            Action action;
            public int ID, x, y, width, height, y_pressed, y_released;
            string Text;
            bool clickable;


            public toets(int ID, string text, Texture2D image, int x, int y, int width, int height, Action actie)
            {
                this.action = actie;
                this.texture = image;
                this.ID = ID;
                this.x = x;
                this.y = y;
                this.y_released = y;
                this.y_pressed = (y + (int)(Game1.GameHeight / 80));
                this.width = width;
                this.height = height;
                this.Text = text;
                this.clickable = true;
            }

            public int checkID()
            {
                return ID;
            }

            public void draw(SpriteBatch spritebatch)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
                if (y == y_released)
                {
                    spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
                }
                spritebatch.DrawString(Game1.Font, this.Text, new Vector2((this.x + this.width / 2), (this.y + this.height / 2)), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, this.Text, new Vector2((this.x + this.width / 2), (this.y + this.height / 2)), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);
            }

            public void update()
            {
                this.y = this.y_released;
                touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if (this.clickable == true && tl.Position.X > this.x && tl.Position.X < this.x + (this.width) && tl.Position.Y > this.y && tl.Position.Y < this.y + (this.height))
                    {
                        this.y = this.y_pressed;
                        this.action();
                        this.clickable = false;
                    }
                    else { this.y = this.y_released; }
                }

                if (touchCollection.Count() == 0) { this.clickable = true; }

            }
        }
        public List<toets> buttonlist;
        public toets q, w, e, r, t, y, u, i, o, p, a, s, d, f, g, h, j, k, l, z, x, c, v, b, n, m, enter, backspace;
        public Image textbox, background;
        public string text, Title;

        public VirtKeyBoard(Texture2D texture, string title, Player speler, Action actie)
        {
            this.q = new toets(0, "q", texture, (int)(Game1.GameWidth / 8), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "q"; });
            this.w = new toets(0, "w", texture, (int)(Game1.GameWidth / 5), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "w"; });
            this.e = new toets(0, "e", texture, (int)(Game1.GameWidth / 3.636), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "e"; });
            this.r = new toets(0, "r", texture, (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "r"; });
            this.t = new toets(0, "t", texture, (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "t"; });
            this.y = new toets(0, "y", texture, (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "y"; });
            this.u = new toets(0, "u", texture, (int)(Game1.GameWidth / 1.739), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "u"; });
            this.i = new toets(0, "i", texture, (int)(Game1.GameWidth / 1.538), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "i"; });
            this.o = new toets(0, "o", texture, (int)(Game1.GameWidth / 1.379), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "o"; });
            this.p = new toets(0, "p", texture, (int)(Game1.GameWidth / 1.25), (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "p"; });
            this.a = new toets(0, "a", texture, (int)(Game1.GameWidth / 6.15), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "a"; });
            this.s = new toets(0, "s", texture, (int)(Game1.GameWidth / 4.210), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "s"; });
            this.d = new toets(0, "d", texture, (int)(Game1.GameWidth / 3.2), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "d"; });
            this.f = new toets(0, "f", texture, (int)(Game1.GameWidth / 2.58), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "f"; });
            this.g = new toets(0, "g", texture, (int)(Game1.GameWidth / 2.16), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "g"; });
            this.h = new toets(0, "h", texture, (int)(Game1.GameWidth / 1.86), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "h"; });
            this.j = new toets(0, "j", texture, (int)(Game1.GameWidth / 1.63), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "j"; });
            this.k = new toets(0, "k", texture, (int)(Game1.GameWidth / 1.45), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "k"; });
            this.l = new toets(0, "l", texture, (int)(Game1.GameWidth / 1.31), (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "l"; });
            this.z = new toets(0, "z", texture, (int)(Game1.GameWidth / 5), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "z"; });
            this.x = new toets(0, "x", texture, (int)(Game1.GameWidth / 3.636), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "x"; });
            this.c = new toets(0, "c", texture, (int)(Game1.GameWidth / 2.857), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "c"; });
            this.v = new toets(0, "v", texture, (int)(Game1.GameWidth / 2.352), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "v"; });
            this.b = new toets(0, "b", texture, (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "b"; });
            this.n = new toets(0, "n", texture, (int)(Game1.GameWidth / 1.739), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "n"; });
            this.m = new toets(0, "m", texture, (int)(Game1.GameWidth / 1.538), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 13.333), (int)(Game1.GameWidth / 13.333), () => { this.text += "m"; });
            this.enter = new toets(0, "Done", texture, (int)(Game1.GameWidth / 1.379), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 6.15), (int)(Game1.GameWidth / 13.333), () => { actie(); speler.Name = this.text; });
            this.backspace = new toets(0, "Erase", texture, (int)(Game1.GameWidth / 32), (int)(Game1.GameWidth / 2), (int)(Game1.GameWidth / 6.15), (int)(Game1.GameWidth / 13.333), () => { if (this.text.Length > 0) { this.text = this.text.Remove(this.text.Length - 1); } });

            this.Title = title;
            this.textbox = new Image(0, texture, 0, (int)(Game1.GameWidth / 4), Game1.GameWidth, (int)(Game1.GameWidth / 13.333));
            this.background = new Image(0, texture, 0, 0, Game1.GameWidth, Game1.GameHeight);
            this.text = "";
            buttonlist = new List<toets> { q, w, e, r, t, y, u, i, o, p, a, s, d, f, g, h, j, k, l, z, x, c, v, b, n, m, enter, backspace };
            Thread.Sleep(200);
        }

        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {

            background.draw(spritebatch);

            foreach (toets knop in buttonlist)
            {
                knop.draw(spritebatch);
            }

            spritebatch.DrawString(Game1.Font, this.Title, new Vector2(Game1.GameWidth / 2, Game1.GameHeight / 3), Color.White, 0, Game1.Font.MeasureString(this.Title) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, this.Title, new Vector2(Game1.GameWidth / 2, Game1.GameHeight / 3), Color.White, 0, Game1.Font.MeasureString(this.Title) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);
            textbox.draw(spritebatch);
            textbox.draw(spritebatch);
            spritebatch.DrawString(Game1.Font, this.text, new Vector2(Game1.GameWidth / 2, Game1.GameHeight / 2), Color.White, 0, Game1.Font.MeasureString(this.text) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, this.text, new Vector2(Game1.GameWidth / 2, Game1.GameHeight / 2), Color.White, 0, Game1.Font.MeasureString(this.text) / 2, (Game1.GameWidth / 800f) * 2.5f, SpriteEffects.None, 0.5f);

        }

        public void update()
        {
            foreach (toets knop in buttonlist)
            {
                knop.update();
            }
        }
    }

    public class ONOFFswitch : GuiElement
    {
        int X, Y, Height, Width, X_offset, R, G;
        public bool active, busy;
        string Text;
        Texture2D BGTexture, PinTexture;
        Color color;

        public ONOFFswitch(string text, int x, int y, Texture2D BGtexture, Texture2D Pintexture)
        {
            this.X = x;
            this.Y = y;
            this.X_offset = 0;
            this.busy = false;
            this.Height = (int)(Game1.GameWidth / 24);
            this.Width = (int)(Game1.GameWidth / 10);
            this.active = false;
            this.PinTexture = Pintexture;
            this.BGTexture = BGtexture;
            this.Text = text;
            this.color = Color.Red;
            this.R = 250;
            this.G = 0;
        }

        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.BGTexture, new Rectangle(this.X, this.Y, this.Width, this.Height), Color.Black);
            spritebatch.Draw(this.PinTexture, new Rectangle(this.X + this.X_offset, this.Y, this.Height, this.Height), this.color);
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2(this.X + Width, Y), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2(this.X + Width, Y), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
        }

        public void update()
        {

            TouchCollection touchcollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchcollection)
            {
                if (tl.Position.X > this.X && tl.Position.X < this.X + this.Width && tl.Position.Y > this.Y && tl.Position.Y < this.Y + this.Height)
                {
                    this.busy = true;
                }
            }

            this.toggle();
        }

        private void toggle()
        {
            this.color = new Color(this.R, this.G, 0);

            if (this.busy)
            {
                if (this.active)
                {
                    if (this.X_offset <= -1)
                    {
                        this.X_offset = 0;
                        this.active = false;
                        this.busy = false;
                    }
                    else
                    {
                        this.X_offset -= (int)((this.Width - this.Height) / 15);
                        this.R += 25;
                        this.G -= 25;
                    }
                }
                else
                {
                    if (this.X_offset >= (this.Width - this.Height + 1))
                    {
                        this.X_offset = (this.Width - this.Height);
                        this.active = true;
                        this.busy = false;
                    }
                    else
                    {
                        this.X_offset += (int)((this.Width - this.Height) / 15);
                        this.R -= 25;
                        this.G += 25;
                    }
                }
            }

        }
    }

    public class Button : GuiElement
    {
        Texture2D texture;
        TouchCollection touchCollection;
        Action action;
        public int ID, x, y, width, height, y_pressed, y_released;
        string Text;
        public bool active;

        public Button(int ID, string text, Texture2D image, int x, int y, int width, int height, Action actie)
        {
            this.action = actie;
            this.texture = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.y_released = y;
            this.y_pressed = (y + (int)(Game1.GameHeight / 80));
            this.width = width;
            this.height = height;
            this.Text = text;
            this.active = false;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2((this.x + this.width / 2), (this.y + this.height / 2)), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2((this.x + this.width / 2), (this.y + this.height / 2)), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
        }

        public void update()
        {
            this.y = this.y_released;
            touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.Position.X > this.x && tl.Position.X < this.x + (this.width) && tl.Position.Y > this.y && tl.Position.Y < this.y + (this.height))
                {
                    this.y = this.y_pressed; ;
                    this.action();
                    this.active = true;
                }
                else { this.y = this.y_released; this.active = false; }
            }

        }
    }

    public class Image : GuiElement
    {
        Texture2D texture;
        public int ID, x, y, width, height;
        public Image(int ID, Texture2D image, int x, int y, int width, int height)
        {
            this.texture = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
        }

        public void update()
        {

        }
    }

    public class Scorecounter : GuiElement
    {
        public int x, y;
        Player player;

        public Scorecounter(int x, int y, Player player)
        {
            this.x = x;
            this.y = y;
            this.player = player;
        }
        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(Game1.Font, Convert.ToString("Score: " + player.score), new Vector2(this.x, this.y), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, Convert.ToString("Score: " + player.score), new Vector2(this.x, this.y), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, Convert.ToString("Score: " + player.score), new Vector2(this.x, this.y), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);

        }

        public void update()
        {
            
        }
    }

    public class PopUp : GuiElement
    {
        TouchCollection touchCollection;
        Texture2D texture;
        List<GuiElement> Lijst;
        public int ID, x, y, width, height;

        public PopUp(int ID, Texture2D image, int x, int y, int width, int height, List<GuiElement> list)
        {
            this.texture = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.Lijst = list;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
        }

        public void update()
        {
            touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.Position.X > this.x && tl.Position.X < this.x + (this.width) && tl.Position.Y > this.y && tl.Position.Y < this.y + (this.height))
                {
                    this.Lijst.Remove(this);
                }
            }
        }
    }

    public class Light : GuiElement
    {
        Texture2D texture;
        private bool flicker;
        public int ID, x, y, width, height, count;
        public Light(int ID, Texture2D image, int x, int y, int width, int height)
        {
            this.flicker = false;
            this.texture = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.count = 0;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            if (this.flicker == false)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
            }
        }

        public void update()
        {
            if (Game1.rnd.Next(1, 400) == 150 && this.flicker == false)
            {
                this.flicker = true;
            }
            if (this.flicker == true)
            {
                this.count++;
                if (this.count == 30)
                {
                    this.flicker = false;
                    this.count = 0;
                }
            }
        }
    }

    public class Player : GuiElement
    {
        public Color coleur;
        Texture2D[] WalkAnimation, punchinganimation;
        Texture2D texture, idle, WhitePixel,shadow;
        TouchButton movement;
        public GunManager guns;
        public bool shoot, jumpbool, FacingRight, ispunching, walking;
        public int score, ID, x, y, width, height, oldpos, animationloop, health, punchloop, ShadowOffset;
        private int vel, Jumploop, MoveSpeed, groundline, mainloop;
        public string Name;
        SoundEffect footstep1, footstep2, footstep3;

        public Player(int ID, Texture2D shadow, SoundEffect footstep1, SoundEffect footstep2, SoundEffect footstep3, Texture2D image, Texture2D[] walkanim, Texture2D[] punchanim, Texture2D whitepixel, TouchButton movement, GunManager gunmanager, int x, int y, int width, int height)
        {
            this.shadow = shadow;
            this.footstep1 = footstep1;
            this.footstep2 = footstep2;
            this.footstep3 = footstep3;
            this.ispunching = false;
            this.WhitePixel = whitepixel;
            this.guns = gunmanager;
            this.mainloop = 0;
            this.WalkAnimation = walkanim;
            this.punchinganimation = punchanim;
            this.oldpos = x;
            this.groundline = (int)(Game1.GameWidth / 3.2);
            this.Jumploop = 0;
            this.animationloop = 0;
            this.vel = 0;
            this.punchloop = 0;
            this.MoveSpeed = (int)(Game1.GameWidth / 200);
            this.movement = movement;
            this.texture = image;
            this.idle = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.FacingRight = true;
            this.shoot = false;
            this.health = 100;
            this.score = 500;
            this.coleur = Color.White;
            this.Name = "";
            this.walking = false;
            this.ShadowOffset = 0;
        }

        public int checkID()
        {
            return ID;
        }

        public void Attack()
        {
            this.guns.currentgun.Attack(this.guns.bulletlist);
            this.ispunching = guns.currentgun.punch;
        }

        public void draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < 2; i++) { spritebatch.DrawString(Game1.Font, this.Name, new Vector2((this.x + this.width / 2), (this.y - (this.height / 3))), Color.White, 0, Game1.Font.MeasureString(this.Name) / 2, (Game1.GameWidth / 800f) * 1.2f, SpriteEffects.None, 0.5f); }

            spritebatch.Draw(this.shadow, new Rectangle(this.x - (int)(Game1.GameWidth / 28.57), this.ShadowOffset + this.y + (int)(this.height/1.3), (int)(Game1.GameWidth/6.66), (int)(Game1.GameWidth / 24.24)), Color.White);
            if (FacingRight)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), this.coleur);

            }
            else
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), null, this.coleur, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);

            }

            this.healthbar(spritebatch);

        }

        public void update()
        {
            this.guns.currentgun.facingright = this.FacingRight;
            this.guns.currentgun.x = (int)(this.x - this.width / 8);
            this.guns.currentgun.y = (int)(this.y + this.height / 2.5);

            if (this.x - this.oldpos > 0)
            {
                this.FacingRight = true;
            }
            else if (this.x - this.oldpos < 0)
            {
                this.FacingRight = false;
            }
            oldpos = this.x;



            this.mainloop++;
            if (this.mainloop > 65530) { this.mainloop = 0; }

            if (mainloop % 15 == 0)
            {
                this.coleur = Color.White;
            }

            if (this.movement.x > (int)(Game1.GameWidth / 10)) { this.x += (int)(this.MoveSpeed / 1.2); }
            else if (this.movement.x < (int)(Game1.GameWidth / 20)) { this.x -= (int)(this.MoveSpeed / 1.2); }
            if (this.movement.y > (int)(Game1.GameWidth / 2.1621)) { this.y += (int)(this.MoveSpeed / 1.8); }
            else if (this.movement.y < (int)(Game1.GameWidth / 2.4242)) { this.y -= (int)(this.MoveSpeed / 1.8); }

            this.jump();
            this.animate();
            this.punch();

            if (jumpbool == false)
            {
                if (this.y < (int)(Game1.GameWidth / 5.35)) { this.y = (int)(Game1.GameWidth / 5.35); }
                else if (this.y > (int)(Game1.GameWidth / 3.35)) { this.y = (int)(Game1.GameWidth / 3.35); }
            }

            if (this.x < 0) { this.x = 0; }
            else if (this.x + width > Game1.GameWidth) { this.x = Game1.GameWidth - width; }

        }

        public void punch()
        {
            if (this.ispunching == true && this.jumpbool == false)
            {
                this.punchloop++;
                if (this.punchloop == 0) { this.texture = this.punchinganimation[0]; }
                else if (this.punchloop == 4) { this.texture = this.punchinganimation[1]; }
                else if (this.punchloop == 8) { this.texture = this.punchinganimation[2]; }
                else if (this.punchloop == 12) { this.texture = this.punchinganimation[3]; }
                else if (this.punchloop == 16) { this.texture = this.punchinganimation[4]; }
                else if (this.punchloop == 20) { this.texture = this.punchinganimation[5]; }
                else if (this.punchloop == 24) { this.texture = this.punchinganimation[0]; this.ispunching = false; this.guns.currentgun.punch = false; this.punchloop = 0; }
            }
        }

        public void animate()
        {
            if (this.ispunching == false)
            {
                if (mainloop % 7 == 0)
                {
                    this.animationloop++;
                }

                if (this.mainloop % 18 == 0 && this.walking)
                {
                    int p = Game1.rnd.Next(1, 4);
                    if (p == 1) { this.footstep1.Play(1.0f, 0, 0); }
                    if (p == 2) { this.footstep2.Play(1.0f, 0, 0); }
                    if (p == 3) { this.footstep3.Play(1.0f, 0, 0); }
                }

                if (this.animationloop > 3)
                {
                    this.animationloop = 0;
                }

                if (this.x - oldpos != 0 || this.movement.y > (int)(Game1.GameWidth / 2.1621) || this.movement.y < (int)(Game1.GameWidth / 2.4242))
                {
                    this.texture = this.WalkAnimation[animationloop];
                    this.walking = true;
                }
                else { this.texture = idle; this.walking = false; }
            }

        }

        public void healthbar(SpriteBatch sb)
        {
            sb.Draw(this.WhitePixel, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), 100 * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.Red);
            sb.Draw(this.WhitePixel, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), this.health * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.LawnGreen);
        }

        public void jump()
        {


            if (this.jumpbool == true)
            {
                int Gspeed = Convert.ToInt32(-0.2 * this.vel + 3); //deze formule is de richtingscoeficiënt van "f(x) = -0.1x^2+4x" de "+4" aan het einde is de hoogte van de sprong
                this.y -= (int)(Game1.GameHeight / 90) * Gspeed; // (height/108) is de zwaartekracht, kijk maar wat er gebeurt bij Height/250
                this.ShadowOffset += (int)(Game1.GameHeight / 90) * Gspeed;
                this.vel++;
                if (this.vel == 0) { Jumploop++; }
                else if (this.vel == 7) { Jumploop++; }
                else if (this.vel == 11) { Jumploop++; }
                else if (this.vel == 16) { Jumploop++; }
                else if (this.vel == 19) { Jumploop++; }
                else if (this.vel == 23) { Jumploop++; }
                else if (this.vel == 26) { Jumploop++; }
                else if (this.vel == 31) { Jumploop++; }
                else if (this.vel == 34) { Jumploop++; }


                if (this.y >= groundline)
                {
                    this.jumpbool = false;
                    this.Jumploop = 0;
                    this.vel = 0;
                }
            }
            else
            {
                groundline = this.y;
                this.ShadowOffset = 0;
            }
        }
    }

    public class GunManager : GuiElement
    {
        Texture2D SlotImage;
        public Gun Gun1, Gun2, Gun3, Gun4, currentgun;
        TouchCollection touchCollection;
        public int ID;
        Song GunReady;
        public List<Gun> gunlist;
        bool alreadyclicked;
        public List<GuiElement> bulletlist;

        public GunManager(int ID, Song gunready, Texture2D slotimage, Gun gun1, Gun gun2, Gun gun3, Gun gun4)
        {
            this.gunlist = new List<Gun>();
            this.bulletlist = new List<GuiElement>();
            this.alreadyclicked = false;
            this.GunReady = gunready;

            this.Gun1 = gun1;
            this.Gun2 = gun2;
            this.Gun3 = gun3;
            this.Gun4 = gun4;

            this.gunlist.Add(Gun1);

            this.currentgun = this.Gun1;

            this.SlotImage = slotimage;
            this.ID = ID;
        }

        public int checkID()
        {
            return ID;
        }

        public void addgun(Gun gun)
        {
            if (gunlist.Contains(gun)) { }
            else
            {
                gunlist.Add(gun);
            }
        }

        public void draw(SpriteBatch spritebatch)
        {
            foreach (GuiElement bullet in bulletlist.ToList())
            {
                if (bullet != null)
                {
                    bullet.draw(spritebatch);
                }
            }

            foreach (Gun weap in gunlist)
            {
                spritebatch.Draw(this.SlotImage, new Rectangle(((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42)), (int)(Game1.GameWidth / 160), (int)(Game1.GameWidth / 11.42), (int)(Game1.GameWidth / 11.42)), Color.White);
                spritebatch.Draw(weap.GunLogo, new Rectangle(((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42)), (int)(Game1.GameWidth / 160), (int)(Game1.GameWidth / 11.42), (int)(Game1.GameWidth / 11.42)), Color.White);

                if (weap.ID != 97)
                {
                    if (weap.amount_of_bullets > 0)
                    {
                        for (int p = 0; p < 3; p++)
                        {
                            spritebatch.DrawString(Game1.Font, Convert.ToString(weap.amount_of_bullets), new Vector2(((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42)) + (int)(Game1.GameWidth / 20), (int)(Game1.GameWidth / 18)), Color.LightGreen, 0, new Vector2(0, 0), (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0f);
                        }
                    }
                    else
                    {
                        for (int p = 0; p < 3; p++)
                        {
                            spritebatch.DrawString(Game1.Font, Convert.ToString(weap.amount_of_bullets), new Vector2(((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42)) + (int)(Game1.GameWidth / 20), (int)(Game1.GameWidth / 18)), Color.Red, 0, new Vector2(0, 0), (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0f);
                        }
                    }
                }

            }

            this.currentgun.draw(spritebatch);
        }

        public void update()
        {
            foreach (Gun weap in gunlist.ToList())
            {
                weap.update();
                foreach (var doublegun in gunlist.GroupBy(gun => gun).SelectMany(group => group.Skip(1)))
                {
                    this.gunlist.Remove(doublegun);
                }

            }
            if (this.currentgun.amount_of_bullets <= 0)
            {
                this.currentgun = this.Gun1;
            }

            foreach (Bullet bullet in bulletlist.ToList())
            {
                if (bullet != null)
                {
                    bullet.update();
                    if (bullet.x > Game1.GameWidth || bullet.x < 0)
                    {
                        bulletlist.Remove(bullet);
                    }
                }
            }

            touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                foreach (Gun weap in gunlist)
                {
                    if (this.alreadyclicked == false && tl.Position.X > ((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42))
                        && tl.Position.X < ((Game1.GameWidth / 2) - (gunlist.Count() * (int)(Game1.GameWidth / 20))) + (gunlist.FindIndex(a => a == weap) * (int)(Game1.GameWidth / 11.42))
                        + (int)(Game1.GameWidth / 11.42) && tl.Position.Y > (int)(Game1.GameWidth / 160) - ((int)(Game1.GameWidth / 11.42) / 2)
                        && tl.Position.Y < (int)(Game1.GameWidth / 160) + (int)(Game1.GameWidth / 11.42))
                    {

                        if (weap.ID == 97)
                        {
                            currentgun = Gun1;
                            MediaPlayer.Volume = 0.4f;
                            MediaPlayer.Play(this.GunReady);
                        }
                        else if (weap.ID == 99)
                        {
                            currentgun = Gun2;
                            MediaPlayer.Volume = 0.4f;
                            MediaPlayer.Play(this.GunReady);
                        }
                        else if (weap.ID == 96)
                        {
                            currentgun = Gun4;
                            MediaPlayer.Volume = 0.4f;
                            MediaPlayer.Play(this.GunReady);
                        }
                        else if (weap.ID == 98)
                        {
                            currentgun = Gun3;
                            MediaPlayer.Volume = 0.4f;
                            MediaPlayer.Play(this.GunReady);
                        }
                    }
                }
            }
        }

    }

    public class Gun : GuiElement
    {
        public Texture2D texture, GunLogo, Fire1, Fire2, WhitePixel, gunlight;
        public bool facingright, shoot, AllowedToShoot, punch;
        public int ID, width, x, y, height, delter, amount_of_bullets, mainloop,
            rndint;
        public double ShootSpeed, X_right_offset, X_left_offset, Height_offset;
        public float volume;
        SoundEffect gunfire;

        public Gun(Texture2D gun_light, SoundEffect GunFire, Texture2D whitepixel, float VoLume, int ID, Texture2D image, Texture2D gunlogo, Texture2D fire1, Texture2D fire2, int width, int height, int delter, double X_left_of, double X_right_of, double Height_of, double shootspeed)
        {
            this.gunlight = gun_light;
            this.amount_of_bullets = 0;
            this.texture = image;
            this.facingright = true;
            this.ID = ID;
            this.mainloop = 0;
            this.x = 0;
            this.y = 0;
            this.X_right_offset = X_right_of;
            this.X_left_offset = X_left_of;
            this.Height_offset = Height_of;
            this.width = width;
            this.height = height;
            this.delter = delter;
            this.GunLogo = gunlogo;
            this.ShootSpeed = shootspeed;
            this.Fire1 = fire1;
            this.Fire2 = fire2;
            this.volume = VoLume;
            this.WhitePixel = whitepixel;
            this.gunfire = GunFire;
        }

        public int checkID()
        {
            return ID;
        }

        public void Attack(List<GuiElement> bulletlist)
        {
            if (this.ID == 97)
            {
                this.punch = true;

            }
            else
            {
                if (AllowedToShoot && this.amount_of_bullets > 0)
                {
                    this.gunfire.Play(this.volume, 0, 0);
                    this.AllowedToShoot = false;
                    this.mainloop = 1;
                    this.shoot = true;
                    bulletlist.Add(new Bullet(0, WhitePixel, this.x + (int)(this.width / 2), (int)(this.y), facingright));
                    this.amount_of_bullets -= 1;
                }
            }
        }

        public void draw(SpriteBatch spritebatch)
        {

            if (facingright)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y - this.delter, this.width, this.height), Color.White);
            }
            else
            {
                spritebatch.Draw(this.texture, new Rectangle((int)(this.x - this.width / 4), this.y - this.delter, this.width, this.height), null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            }



            if (this.shoot == true)
            {
                if (facingright)
                {
                    if (this.rndint == 1)
                    {
                        spritebatch.Draw(this.Fire1, new Rectangle(this.x + (int)(this.width / this.X_right_offset), this.y - (int)(this.height / this.Height_offset), (int)(Game1.GameWidth / 17.02), (int)(Game1.GameWidth / 22.857)), Color.White);
                    }
                    else if (this.rndint == 2)
                    {
                        spritebatch.Draw(this.Fire2, new Rectangle(this.x + (int)(this.width / this.X_right_offset), this.y - (int)(this.height / this.Height_offset), (int)(Game1.GameWidth / 17.02), (int)(Game1.GameWidth / 22.857)), Color.White);
                    }
                    spritebatch.Draw(this.gunlight, new Rectangle(this.x + (int)(this.width / this.X_right_offset) - (int)(Game1.GameWidth / 3), this.y - (int)(this.height / this.Height_offset) - (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth / 1.5), (int)(Game1.GameWidth / 2)), Color.White);
                    spritebatch.Draw(this.gunlight, new Rectangle(this.x + (int)(this.width / this.X_right_offset) - (int)(Game1.GameWidth / 3), this.y - (int)(this.height / this.Height_offset) - (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth / 1.5), (int)(Game1.GameWidth / 2)), Color.White);


                }
                else
                {
                    if (this.rndint == 1)
                    {
                        spritebatch.Draw(this.Fire1, new Rectangle(this.x - (int)(Game1.GameWidth / this.X_left_offset), (int)(this.y - this.height / this.Height_offset), (int)(Game1.GameWidth / 17.02), (int)(Game1.GameWidth / 22.857)), null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
                    }
                    else if (this.rndint == 2)
                    {
                        spritebatch.Draw(this.Fire2, new Rectangle(this.x - (int)(Game1.GameWidth / this.X_left_offset), (int)(this.y - this.height / this.Height_offset), (int)(Game1.GameWidth / 17.02), (int)(Game1.GameWidth / 22.857)), null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
                    }
                    spritebatch.Draw(this.gunlight, new Rectangle(this.x - (int)(Game1.GameWidth / this.X_left_offset) - (int)(Game1.GameWidth / 3), (int)(this.y - this.height / this.Height_offset) - (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth / 1.5), (int)(Game1.GameWidth / 2)), Color.White);
                    spritebatch.Draw(this.gunlight, new Rectangle(this.x - (int)(Game1.GameWidth / this.X_left_offset) - (int)(Game1.GameWidth / 3), (int)(this.y - this.height / this.Height_offset) - (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth / 1.5), (int)(Game1.GameWidth / 2)), Color.White);
                }
            }
        }

        public void update()
        {
            

            if (this.mainloop % this.ShootSpeed == 0) { this.AllowedToShoot = true; }

            this.mainloop++;
            if (this.mainloop > 65530) { this.mainloop = 0; }

            if (mainloop % 5 == 0)
            {
                this.shoot = false;
                this.rndint = Game1.rnd.Next(1, 3);
            }
        }

    }

    public class Bullet : GuiElement
    {
        Texture2D texture;
        private bool FacingRight;
        public int ID, x, y, width, height;
        public Bullet(int ID, Texture2D image, int x, int y, bool facingright)
        {
            this.texture = image;
            this.ID = ID;
            this.FacingRight = facingright;
            this.x = x;
            this.y = y;
            this.width = (int)(Game1.GameWidth / 100);
            this.height = (int)(Game1.GameWidth / 200);
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
        }

        public void update()
        {
            if (FacingRight)
            {
                this.x += (int)(Game1.GameWidth / 40);
            }
            else
            {
                this.x -= (int)(Game1.GameWidth / 40);
            }

            if (this.x > Game1.GameWidth || this.x < 0)
            {
                //delete self;
            }
        }

    }

    public class Enemy : GuiElement
    {
        Color coleur;
        Texture2D texture, idle, whitep, shadow;
        Texture2D[] WalkAnim, PunchAnim;
        List<GuiElement> bulletlist, Voorlist;
        public bool DoSomeBool, DoAttackBool, DoWalkBackBool, FacingRight, DoFindBool;
        public int ID, x, y, width, height, enemyloop, oldposX, oldposY, animationloop,
            currentstate, MoveSpeed, player_x, player_y, player_punchloop, dosomecount,
            dosomecount_Max, walkbackcount, health, punchloop, vel;
        public Enemy(int ID, Texture2D shadow, Texture2D image, Texture2D whitep, Texture2D[] walkanim, Texture2D[] punchanim, int x, int y)
        {
            this.shadow = shadow;
            this.PunchAnim = punchanim;
            this.whitep = whitep;
            this.health = 100;
            this.texture = image;
            this.idle = image;
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = (int)(Game1.GameWidth / 12.5);
            this.height = (int)(Game1.GameWidth / 10);
            this.enemyloop = 0;
            this.currentstate = 0;
            this.MoveSpeed = (int)(Game1.GameWidth / 230);
            this.DoSomeBool = true;
            this.player_x = 0;
            this.player_y = 0;
            this.player_punchloop = 0;
            this.dosomecount = 0;
            this.dosomecount_Max = 10;
            this.walkbackcount = 0;
            this.WalkAnim = walkanim;
            this.oldposX = 0;
            this.oldposY = 0;
            this.vel = 0;
            this.animationloop = 0;
            this.punchloop = 0;
            this.FacingRight = true;
            this.bulletlist = new List<GuiElement>();
            this.Voorlist = new List<GuiElement>();
            this.coleur = Color.White;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.shadow, new Rectangle(this.x - (int)(Game1.GameWidth / 28.57), this.y + (int)(this.height / 1.3), (int)(Game1.GameWidth / 6.66), (int)(Game1.GameWidth / 24.24)), Color.White);
            if (FacingRight)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), this.coleur);
            }
            else
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), null, this.coleur, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            }

            this.healthbar(spritebatch);
        }

        public void update()
        {
            this.enemyloop++;
            if (this.enemyloop > 65530) { enemyloop = 0; }
            if (enemyloop % 10 == 0)
            {
                this.coleur = Color.White;
            }
            if (this.health > 1)
            {
                if (DoSomeBool) { this.dosomething(); }
                if (DoAttackBool) { this.attack(); }
                if (DoWalkBackBool) { this.walkback(); }
                if (DoFindBool) { this.findtarget(); }

                this.animate();
                this.checkifhit();

                if (this.y < (int)(Game1.GameWidth / 5.35)) { this.y = (int)(Game1.GameWidth / 5.35); }
                else if (this.y > (int)(Game1.GameWidth / 3.35)) { this.y = (int)(Game1.GameWidth / 3.35); }
            }
            else
            {
                this.y -= (int)((Game1.GameHeight / 70) * (-0.2 * this.vel + 2.2)); //70 = zwaartekracht, 2.2 = hoogte
                this.vel++;
                this.coleur = Color.Red;
                this.DoAttackBool = false;
            }


            if (this.x < 0) { this.x = 0; }
            else if (this.x + width > Game1.GameWidth) { this.x = Game1.GameWidth - width; }

            if (this.x - this.oldposX > 0)
            {
                this.FacingRight = true;
            }
            else if (this.x - this.oldposX < 0)
            {
                this.FacingRight = false;
            }
            oldposX = this.x;
            oldposY = this.y;
        }


        public void checkifhit()
        {
            foreach (Bullet bullet in this.bulletlist.ToList())
            {
                if (bullet.x > this.x && bullet.x < this.x + width && bullet.y > this.y && bullet.y < this.y + height)
                {
                    int dmg = Game1.rnd.Next(12, 19);
                    this.bulletlist.Remove(bullet);
                    this.health -= dmg;
                    this.coleur = Color.Red;
                    this.x += (int)(Game1.GameHeight / 160);
                    this.Voorlist.Add(new FloatingText(Convert.ToString("-" + dmg), this.x + (this.width / 2), this.y + (this.height / 2), Voorlist));
                }
            }
            if (this.x < this.player_x + this.width && this.x + this.width > this.player_x && this.player_punchloop == 20)
            {
                this.health -= 20;
                this.coleur = Color.Red;
                this.Voorlist.Add(new FloatingText("-20", this.x + (this.width / 2), this.y + (this.height / 2), Voorlist));
            }

        }

        public void animate()
        {
            if (enemyloop % 7 == 0)
            {
                this.animationloop++;
            }

            if (this.animationloop > 3)
            {
                this.animationloop = 0;
            }
            if (this.DoAttackBool == false)
            {
                if (this.x - oldposX != 0 || this.y - oldposY != 0)
                {
                    this.texture = this.WalkAnim[animationloop];
                }
                else { this.texture = idle; }
            }

        }

        public void active(Player player, List<GuiElement> ForeGList)
        {
            this.player_x = player.x;
            this.player_y = player.y;
            this.bulletlist = player.guns.bulletlist;
            this.player_punchloop = player.punchloop;
            this.Voorlist = ForeGList;
        }

        public void walkback()
        {
            this.DoFindBool = false;
            this.DoAttackBool = false;
            this.DoSomeBool = false;

            if (enemyloop % 30 == 0)
            {
                this.walkbackcount++;
                this.currentstate = Game1.rnd.Next(1, 4);
                if (this.walkbackcount > 3)
                {
                    this.walkbackcount = 0;
                    this.DoWalkBackBool = false;
                    this.DoSomeBool = true;
                    this.dosomecount_Max = Game1.rnd.Next(5, 11);
                }
            }

            if (currentstate == 1)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }

            if (currentstate == 2)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
            }

            if (currentstate == 3)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }
        }

        public void findtarget()
        {
            if (this.x < player_x + this.width && this.x + this.width > player_x && this.y < player_y + (this.height / 2) && this.y > player_y - (this.height / 2))
            {
                this.DoFindBool = false;
                this.DoSomeBool = false;
                this.DoWalkBackBool = false;
                this.DoAttackBool = true;
            }
            else
            {
                if (this.x < player_x)
                {
                    this.x += (int)(this.MoveSpeed / 1.2);
                }
                if (this.x > player_x)
                {
                    this.x -= (int)(this.MoveSpeed / 1.2);
                }
                if (this.y > player_y)
                {
                    this.y -= (int)(this.MoveSpeed / 1.8);
                }
                if (this.y < player_y)
                {
                    this.y += (int)(this.MoveSpeed / 1.8);
                }
            }
        }

        public void attack()
        {
            this.punchloop++;
            if (this.punchloop == 0) { this.texture = this.PunchAnim[0]; }
            else if (this.punchloop == 8) { this.texture = this.PunchAnim[1]; }
            else if (this.punchloop == 16) { this.texture = this.PunchAnim[2]; }
            else if (this.punchloop == 24) { this.texture = this.PunchAnim[3]; }
            else if (this.punchloop == 32) { this.texture = this.PunchAnim[4]; }
            else if (this.punchloop == 40) { this.texture = this.PunchAnim[5]; }
            else if (this.punchloop == 48) { this.texture = this.PunchAnim[0]; this.punchloop = 0; this.DoAttackBool = false; this.DoWalkBackBool = true; }
        }

        public void dosomething()
        {
            if (enemyloop % 22 == 0)
            {
                this.dosomecount++;
                this.currentstate = Game1.rnd.Next(1, 9);
                if (this.x > this.player_x - (int)(Game1.GameWidth / 5) && this.x < this.player_x + (int)(Game1.GameWidth / 4))
                {
                    this.DoSomeBool = false;
                    this.DoAttackBool = false;
                    this.DoWalkBackBool = false;
                    this.DoFindBool = true;
                    this.dosomecount = 0;
                }
                else if (this.dosomecount > this.dosomecount_Max)
                {
                    this.DoSomeBool = false;
                    this.DoAttackBool = false;
                    this.DoWalkBackBool = false;
                    this.DoFindBool = true;
                    this.dosomecount = 0;
                }
            }

            if (currentstate == 0)
            {

            }
            else if (currentstate == 1)
            {
                this.x -= (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 2)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x -= (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 3)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 4)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 5)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 6)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
                this.y += (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 7)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 8)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
                this.x -= (int)(this.MoveSpeed / 1.2);
            }

        }

        public void healthbar(SpriteBatch sb)
        {
            sb.Draw(this.whitep, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), 100 * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.Red);
            sb.Draw(this.whitep, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), this.health * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.LawnGreen);
        }


    }

    public class Big_Enemy : GuiElement
    {
        Player player;
        Color coleur;
        Texture2D texture, idle, whitep, shadow;
        Texture2D[] WalkAnim;
        List<GuiElement> bulletlist, Voorlist;
        public bool DoSomeBool, DoAttackBool, DoWalkBackBool, FacingRight, DoFindBool;
        public int lazerlength, ID, x, y, width, height, enemyloop, oldposX, oldposY, animationloop,
            currentstate, MoveSpeed, player_x, player_y, player_punchloop, dosomecount,
            dosomecount_Max, walkbackcount, health, punchloop, vel;
        public Big_Enemy(int ID, Texture2D shadow, Texture2D image, Texture2D whitep, Texture2D[] walkanim, int x, int y)
        {
            this.shadow = shadow;
            this.whitep = whitep;
            this.health = 400;
            this.texture = image;
            this.idle = image;
            this.ID = ID;
            this.lazerlength = 1;
            this.width = (int)(Game1.GameWidth / 8.34);
            this.height = (int)(Game1.GameWidth / 6.67);
            this.x = x;
            this.y = y;
            this.enemyloop = 0;
            this.currentstate = 0;
            this.MoveSpeed = (int)(Game1.GameWidth / 230);
            this.DoSomeBool = true;
            this.player_x = 0;
            this.player_y = 0;
            this.player_punchloop = 0;
            this.dosomecount = 0;
            this.dosomecount_Max = 10;
            this.walkbackcount = 0;
            this.WalkAnim = walkanim;
            this.oldposX = 0;
            this.oldposY = 0;
            this.vel = 0;
            this.animationloop = 0;
            this.punchloop = 0;
            this.FacingRight = true;
            this.bulletlist = new List<GuiElement>();
            this.Voorlist = new List<GuiElement>();
            this.coleur = Color.White;
        }

        public int checkID()
        {
            return ID;
        }

        public void draw(SpriteBatch spritebatch)
        {

            if (this.DoAttackBool)
            {
                Texture2D Lazer = new Texture2D(spritebatch.GraphicsDevice, this.lazerlength, (int)(Game1.GameWidth / 80));
                Color[] pixeldata = new Color[this.lazerlength * (int)(Game1.GameWidth / 80)];
                for (int i = 0; i < pixeldata.Length; ++i) pixeldata[i] = new Color(0, 215, 215); ;
                Lazer.SetData(pixeldata);
                Vector2 coords = new Vector2(this.x - this.lazerlength, this.y + this.height/3);

                Texture2D Lazer2 = new Texture2D(spritebatch.GraphicsDevice, this.lazerlength, (int)(Game1.GameWidth / 160));
                Color[] pixeldata2 = new Color[this.lazerlength * (int)(Game1.GameWidth / 160)];
                for (int i = 0; i < pixeldata2.Length; ++i) pixeldata2[i] = new Color(213, 255, 255);
                Lazer2.SetData(pixeldata2);
                Vector2 coords2 = new Vector2(this.x - this.lazerlength, this.y + this.height / 3 + (int)(Game1.GameWidth / 320));

                spritebatch.Draw(Lazer, coords, Color.White);
                spritebatch.Draw(Lazer2, coords2, Color.White);
            }

            spritebatch.Draw(this.shadow, new Rectangle(this.x - (int)(Game1.GameWidth / 15.38), this.y + (int)(this.height / 1.45), (int)(Game1.GameWidth / 4), (int)(Game1.GameWidth / 14.54)), Color.White);
            if (FacingRight)
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), this.coleur);
            }
            else
            {
                spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), null, this.coleur, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            }

            this.healthbar(spritebatch);
        }

        public void update()
        {
            this.enemyloop++;
            if (this.enemyloop > 65530) { enemyloop = 0; }
            if (enemyloop % 10 == 0)
            {
                this.coleur = Color.White;
            }
            if (this.health == 0) { this.health = -1;}
            if (this.health > 1)
            {
                if (DoSomeBool) { this.dosomething(); }
                if (DoAttackBool) { this.attack(); }
                if (DoWalkBackBool) { this.walkback(); }
                if (DoFindBool) { this.findtarget(); }

                this.animate();
                this.checkifhit();

                if (this.y < (int)(Game1.GameWidth / 7.13)) { this.y = (int)(Game1.GameWidth / 7.13); }
                else if (this.y > (int)(Game1.GameWidth / 3.83)) { this.y = (int)(Game1.GameWidth / 3.83); }
            }
            else
            {
                this.y -= (int)((Game1.GameHeight / 70) * (-0.2 * this.vel + 2.2)); //70 = zwaartekracht, 2.2 = hoogte
                this.vel++;
                this.coleur = Color.Red;
                this.DoAttackBool = false;
            }


            if (this.x < 0) { this.x = 0; }
            else if (this.x + width > Game1.GameWidth) { this.x = Game1.GameWidth - width; }

            if (this.x - this.oldposX > 0)
            {
                this.FacingRight = true;
            }
            else if (this.x - this.oldposX < 0)
            {
                this.FacingRight = false;
            }
            oldposX = this.x;
            oldposY = this.y;
        }


        public void checkifhit()
        {
            foreach (Bullet bullet in this.bulletlist.ToList())
            {
                if (bullet.x > this.x && bullet.x < this.x + width && bullet.y > this.y && bullet.y < this.y + height)
                {
                    int dmg = Game1.rnd.Next(12, 19);
                    this.bulletlist.Remove(bullet);
                    this.health -= dmg;
                    this.coleur = Color.Red;
                    this.x += (int)(Game1.GameHeight / 160);
                    this.Voorlist.Add(new FloatingText(Convert.ToString("-" + dmg), this.x + (this.width / 2), this.y + (this.height / 2), Voorlist));
                }
            }
            if (this.x < this.player_x + this.width && this.x + this.width > this.player_x && this.player_punchloop == 20)
            {
                this.health -= 20;
                this.coleur = Color.Red;
                this.Voorlist.Add(new FloatingText("-20", this.x + (this.width / 2), this.y + (this.height / 2), Voorlist));
            }

        }

        public void animate()
        {
            if (enemyloop % 7 == 0)
            {
                this.animationloop++;
            }

            if (this.animationloop > 3)
            {
                this.animationloop = 0;
            }
            if (this.DoAttackBool == false)
            {
                if (this.x - oldposX != 0 || this.y - oldposY != 0)
                {
                    this.texture = this.WalkAnim[animationloop];
                }
                else { this.texture = idle; }
            }

        }

        public void active(Player player, List<GuiElement> ForeGList)
        {
            this.player = player;
            this.player_x = player.x;
            this.player_y = player.y;
            this.bulletlist = player.guns.bulletlist;
            this.player_punchloop = player.punchloop;
            this.Voorlist = ForeGList;
        }

        public void walkback()
        {
            this.DoFindBool = false;
            this.DoAttackBool = false;
            this.DoSomeBool = false;

            if (enemyloop % 30 == 0)
            {
                this.walkbackcount++;
                this.currentstate = Game1.rnd.Next(1, 4);
                if (this.walkbackcount > 3)
                {
                    this.walkbackcount = 0;
                    this.DoWalkBackBool = false;
                    this.DoSomeBool = true;
                    this.dosomecount_Max = Game1.rnd.Next(5, 11);
                }
            }

            if (currentstate == 1)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }

            if (currentstate == 2)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
            }

            if (currentstate == 3)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }
        }

        public void findtarget()
        {
            if (this.x - player_x < (int)(Game1.GameWidth/2.5))
            {
                this.FacingRight = false;
                this.DoFindBool = false;
                this.DoSomeBool = false;
                this.DoWalkBackBool = false;
                this.DoAttackBool = true;
            }
            else
            {
                if (this.x < player_x)
                {
                    this.x += (int)(this.MoveSpeed / 1.2);
                }
                if (this.x > player_x)
                {
                    this.x -= (int)(this.MoveSpeed / 1.2);
                }
                if (this.y > player_y)
                {
                    this.y -= (int)(this.MoveSpeed / 1.8);
                }
                if (this.y < player_y)
                {
                    this.y += (int)(this.MoveSpeed / 1.8);
                }
            }
        }

        public void attack()
        {
            punchloop++;
            this.FacingRight = false;
            this.lazerlength += (int)(Game1.GameWidth/80);
            if (this.punchloop == 55)
            {
                this.punchloop = 0; this.DoAttackBool = false; this.DoWalkBackBool = true; this.lazerlength = 1;
            }
            if (player.jumpbool == false && player.x + player.width > this.x - this.lazerlength && player.y < this.y + this.height/2 && this.y + this.height / 2 < player.y + player.height)
            {
                if (enemyloop % 10 == 0)
                {
                    int damage = Game1.rnd.Next(3, 7);

                    this.player.health -= damage;
                    this.Voorlist.Add(new FloatingText("-"+damage, this.player.x, this.player.y, Voorlist));
                }
                
            }
            
        }

        public void dosomething()
        {
            if (enemyloop % 22 == 0)
            {
                this.dosomecount++;
                this.currentstate = Game1.rnd.Next(1, 9);
                if (this.x > this.player_x - (int)(Game1.GameWidth / 5) && this.x < this.player_x + (int)(Game1.GameWidth / 4))
                {
                    this.DoSomeBool = false;
                    this.DoAttackBool = false;
                    this.DoWalkBackBool = false;
                    this.DoFindBool = true;
                    this.dosomecount = 0;
                }
                else if (this.dosomecount > this.dosomecount_Max)
                {
                    this.DoSomeBool = false;
                    this.DoAttackBool = false;
                    this.DoWalkBackBool = false;
                    this.DoFindBool = true;
                    this.dosomecount = 0;
                }
            }

            if (currentstate == 0)
            {

            }
            else if (currentstate == 1)
            {
                this.x -= (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 2)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x -= (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 3)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 4)
            {
                this.y -= (int)(this.MoveSpeed / 1.8);
                this.x += (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 5)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
            }
            else if (currentstate == 6)
            {
                this.x += (int)(this.MoveSpeed / 1.2);
                this.y += (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 7)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
            }
            else if (currentstate == 8)
            {
                this.y += (int)(this.MoveSpeed / 1.8);
                this.x -= (int)(this.MoveSpeed / 1.2);
            }

        }

        public void healthbar(SpriteBatch sb)
        {
            sb.Draw(this.whitep, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), 100 * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.Red);
            sb.Draw(this.whitep, new Rectangle(this.x - (int)(Game1.GameWidth / 53.33), this.y - (int)(Game1.GameWidth / 53.33), (this.health / 4) * (int)(Game1.GameWidth / 800), (int)(Game1.GameWidth / 114.28)), Color.LawnGreen);
        }


    }

    public class FloatingText : GuiElement
    {
        string Text;
        int x, y;
        List<GuiElement> voorgrond;

        public FloatingText(string Text, int x, int y, List<GuiElement> ForeGList)
        {
            this.x = x;
            this.y = y;
            this.Text = Text;
            this.voorgrond = ForeGList;
        }

        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2(this.x, y), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
            spritebatch.DrawString(Game1.Font, this.Text, new Vector2(this.x, y), Color.White, 0, Game1.Font.MeasureString(this.Text) / 2, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
        }

        public void update()
        {
            this.y -= (int)(Game1.GameWidth / 160);
            if (this.y < 0)
            {
                this.voorgrond.Remove(this);
            }
        }
    }

    public class AmmoBox : GuiElement
    {
        Texture2D texture;
        Player player;
        List<GuiElement> Midlist;
        public int x, y, width, height, min,max;
        Level Level;

        public AmmoBox(int min, int max,Texture2D image, int x, int y, Player player, List<GuiElement> midlist, Level level)
        {
            this.max = max;
            this.min = min;
            this.texture = image;
            this.x = x;
            this.y = y;
            this.width = (int)(Game1.GameWidth / 20);
            this.height = (int)(Game1.GameWidth / 20);
            this.player = player;
            this.Midlist = midlist;
            this.Level = level;
        }

        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.texture, new Rectangle(this.x, this.y, this.width, this.height), Color.White);
        }

        public void update()
        {
            if (player.x + player.width > this.x && this.x + this.width > player.x)
            {
                int number = Game1.rnd.Next(min, max+1);
                this.player.score += number;
                this.Midlist.Add(new FloatingText(Convert.ToString("+"+number+" Score"),this.x,this.y,this.Midlist));
                this.Level.RemoveProp(this);
            }
        }
    }

    public class Gunstore : GuiElement
    {
        TouchCollection touchCollection;
        Texture2D img, gunstore_bg, gunshop_img;
        Player player;
        Button knop, mp7a1_button, rifle_button, pistol_button;
        int x, y, height, width;
        Action onclick, ExitAction;
        bool active;
        Button backbutton;
        List<GuiElement> drawableList;
        Song song;

        public Gunstore(Texture2D gunshop_img, SoundEffect ka_ching, Song song, Texture2D blackp, Texture2D gunstore_bg, Texture2D mp7a1_img, Texture2D rifle_img, Texture2D pistol_img, Texture2D image, int x, int y, Player speler, Button knop, Action onclick, Action ExitAction)
        {
            this.song = song;
            this.drawableList = new List<GuiElement>();
            this.gunshop_img = gunshop_img;
            this.gunstore_bg = gunstore_bg;
            this.onclick = onclick;
            this.ExitAction = ExitAction;
            this.knop = knop;
            this.y = y;
            this.x = x;
            this.height = (int)(Game1.GameWidth / 6.66);
            this.width = (int)(Game1.GameWidth / 4.6);
            this.player = speler;
            this.img = image;
            this.active = false;
            this.backbutton = new Button(0, "Exit", blackp, (int)(Game1.GameWidth / 1.176), (int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 8), (int)(Game1.GameWidth / 16), () => { this.active = false; ExitAction(); });
            this.mp7a1_button = new Button(0, "", mp7a1_img, (int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 5.33), (int)(Game1.GameWidth / 3.72), (int)(Game1.GameWidth / 10.66), () => {
                if (!mp7a1_button.active) {
                    if (player.score >= 250 && this.player.guns.gunlist.Any(gun => gun.ID == this.player.guns.Gun4.ID))
                    {
                        player.guns.Gun4.amount_of_bullets += 10;
                        drawableList.Add(new FloatingText("-250", (int)(Game1.GameWidth / 1.77), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 250;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                    else if (player.score >= 1800)
                    {
                        player.guns.addgun(player.guns.Gun4);
                        player.guns.Gun4.amount_of_bullets = 30;
                        drawableList.Add(new FloatingText("-1800", (int)(Game1.GameWidth / 1.77), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 1800;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                }
            });

            this.rifle_button = new Button(0, "", rifle_img, (int)(Game1.GameWidth / 3.81), (int)(Game1.GameWidth / 5.33), (int)(Game1.GameWidth / 3.72), (int)(Game1.GameWidth / 10.66), () => {
                if (!rifle_button.active)
                {
                    if (player.score >= 150 && this.player.guns.gunlist.Any(gun => gun.ID == this.player.guns.Gun3.ID))
                    {
                        player.guns.Gun3.amount_of_bullets += 10;
                        drawableList.Add(new FloatingText("-150", (int)(Game1.GameWidth / 3.2), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 150;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                    else if (player.score >= 1000)
                    {
                        player.guns.addgun(player.guns.Gun3);
                        player.guns.Gun3.amount_of_bullets = 25;
                        drawableList.Add(new FloatingText("-1000", (int)(Game1.GameWidth / 3.2), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 1000;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                }
            });
            this.pistol_button = new Button(0, "", pistol_img, (int)(Game1.GameWidth / 80), (int)(Game1.GameWidth / 5.33), (int)(Game1.GameWidth / 3.72), (int)(Game1.GameWidth / 10.66), () => {
                if (!pistol_button.active)
                {
                    if (player.score >= 100 && this.player.guns.gunlist.Any(gun => gun.ID == this.player.guns.Gun2.ID))
                    {
                        player.guns.Gun2.amount_of_bullets += 10;
                        drawableList.Add(new FloatingText("-100", (int)(Game1.GameWidth / 16), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 100;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                    else if (player.score >= 500)
                    {
                        player.guns.addgun(player.guns.Gun2);
                        player.guns.Gun2.amount_of_bullets = 15;
                        drawableList.Add(new FloatingText("-500", (int)(Game1.GameWidth / 16), (int)(Game1.GameWidth / 2.66), drawableList));
                        player.score -= 500;
                        ka_ching.Play(0.4f, 0, 0);
                    }
                }
            });
        }

        public int checkID()
        {
            return 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            if (this.active)
            {
                spritebatch.Draw(gunstore_bg, new Rectangle(0, 0, Game1.GameWidth, Game1.GameHeight), Color.White);
                this.mp7a1_button.draw(spritebatch);
                this.rifle_button.draw(spritebatch);
                this.pistol_button.draw(spritebatch);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Pistol: $500"), new Vector2((int)(Game1.GameWidth / 80), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Pistol: $500"), new Vector2((int)(Game1.GameWidth / 80), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $100"), new Vector2((int)(Game1.GameWidth / 80), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $100"), new Vector2((int)(Game1.GameWidth / 80), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);

                spritebatch.DrawString(Game1.Font, Convert.ToString("Rifle: $1000"), new Vector2((int)(Game1.GameWidth / 3.81), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Rifle: $1000"), new Vector2((int)(Game1.GameWidth / 3.81), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $150"), new Vector2((int)(Game1.GameWidth / 3.81), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $150"), new Vector2((int)(Game1.GameWidth / 3.81), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);

                spritebatch.DrawString(Game1.Font, Convert.ToString("Mp7a1: $1800"), new Vector2((int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Mp7a1: $1800"), new Vector2((int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 10)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $250"), new Vector2((int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, Convert.ToString("Ammo: $250"), new Vector2((int)(Game1.GameWidth / 1.95), (int)(Game1.GameWidth / 6.66)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);


                this.backbutton.draw(spritebatch);

                foreach (GuiElement item in drawableList.ToList())
                {
                    item.draw(spritebatch);
                }
            }
            else
            {
                spritebatch.Draw(gunshop_img, new Rectangle(x, y, width, height), Color.White);

                if (this.player.x - this.width < this.x && this.x < this.player.x + this.player.width && this.player.y < this.y + this.height)
                {
                    spritebatch.DrawString(Game1.Font, Convert.ToString("press USE to buy a gun"), new Vector2(this.x + (this.width / 2), this.y - this.height/4), Color.White, 0, (Game1.Font.MeasureString("press USE to buy a gun") / 2), (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);
                    spritebatch.DrawString(Game1.Font, Convert.ToString("press USE to buy a gun"), new Vector2(this.x + (this.width / 2), this.y - this.height/4), Color.White, 0, (Game1.Font.MeasureString("press USE to buy a gun") / 2), (Game1.GameWidth / 800f) * 1.5f, SpriteEffects.None, 0.5f);

                }
            }
        }

        public void update()
        {
            foreach (GuiElement item in drawableList.ToList())
            {
                item.update();
            }

            if (this.active)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Volume = 0.7f;
                    MediaPlayer.Play(this.song);
                }

                this.backbutton.update();
                this.mp7a1_button.update();
                this.rifle_button.update();
                this.pistol_button.update();

                if (this.mp7a1_button.y == this.mp7a1_button.y_released)
                {
                    this.mp7a1_button.active = false;
                }
                if (this.rifle_button.y == this.rifle_button.y_released)
                {
                    this.rifle_button.active = false;
                }
                if (this.pistol_button.y == this.pistol_button.y_released)
                {
                    this.pistol_button.active = false;
                }
            }
            else
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }

                touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if (this.player.x - this.width < this.x && this.x < this.player.x + this.player.width && this.player.y < this.y + this.height)
                    {
                        if (this.knop.active)
                        {
                            this.onclick();
                            this.active = true;
                        }
                    }
                }
            }
        }
    }

    public class GameOver : GuiElement
    {
        Player playerinfo;
        Button bye, next_level;
        Texture2D blackp;
        Texture2D[] tele_anim;
        int gamex, overx, scorey, scorecounter,iteration, stacklayer,tele_anim_count, anim_pos_y;
        bool finished, go_to_next;
        Action ToNextFunc;

        public GameOver(Player player, Button exitgame, Button nextlevel, Texture2D blackp, Texture2D[] tele_anim, Action end_func)
        {
            this.playerinfo = player;
            this.gamex = -(int)(Game1.GameWidth/2);
            this.overx = (int)(Game1.GameWidth);
            this.scorey = (Game1.GameHeight);
            this.scorecounter = 0;
            this.bye = exitgame;
            this.next_level = nextlevel;
            this.finished = false;
            this.go_to_next = false;
            this.blackp = blackp;
            this.iteration = 0;
            this.stacklayer = 0;
            this.tele_anim = tele_anim;
            this.tele_anim_count = 0;
            this.anim_pos_y = 0;
            this.ToNextFunc = end_func;
        }

        public int checkID()
        {
            return 0;
        }

        public void reset()
        {
            this.gamex = -(int)(Game1.GameWidth / 2);
            this.overx = (int)(Game1.GameWidth);
            this.scorey = (Game1.GameHeight);
            this.scorecounter = 0;
            this.finished = false;
            this.iteration = 0;
            this.go_to_next = false;
            this.stacklayer = 0;
            this.tele_anim_count = 0;
        }

        public void draw(SpriteBatch spritebatch)
        {
            if (this.stacklayer < 10)
            {
                this.iteration += 1;
                if (this.iteration % 15 == 0)
                {
                    this.stacklayer += 1;
                }
            }
            for (int i = 0; i < this.stacklayer; i++)
            {
                spritebatch.Draw(blackp, new Rectangle(0,0,Game1.GameWidth, Game1.GameHeight), Color.White);
            }
            

            this.playerinfo.draw(spritebatch);

            if (this.go_to_next == false)
            {
                spritebatch.DrawString(Game1.Font, "MISSION", new Vector2(this.gamex, (int)(Game1.GameWidth / 16)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 300f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, "MISSION", new Vector2(this.gamex, (int)(Game1.GameWidth / 16)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 300f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, "SUCCES", new Vector2(this.overx, (int)(Game1.GameWidth / 8)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 300f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, "SUCCES", new Vector2(this.overx, (int)(Game1.GameWidth / 8)), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 300f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, "SCORE: " + this.scorecounter, new Vector2((int)(Game1.GameWidth / 3), this.scorey), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 500f) * 1.5f, SpriteEffects.None, 0.5f);
                spritebatch.DrawString(Game1.Font, "SCORE: " + this.scorecounter, new Vector2((int)(Game1.GameWidth / 3), this.scorey), Color.White, 0, Vector2.Zero, (Game1.GameWidth / 500f) * 1.5f, SpriteEffects.None, 0.5f);

                if (this.finished)
                {
                    this.bye.draw(spritebatch);
                    this.next_level.draw(spritebatch);
                }
            }
            else if (this.playerinfo.y > this.playerinfo.height)
            {
                spritebatch.Draw(this.tele_anim[this.tele_anim_count], new Rectangle(this.playerinfo.x - (int)(Game1.GameWidth/44.444), this.anim_pos_y + this.playerinfo.height - (int)(Game1.GameWidth / 2.76), (int)(Game1.GameWidth/8), (int)(Game1.GameWidth / 2.76)), Color.White);
            }

        }

        public void update()
        {
            if (this.go_to_next)
            {
                this.iteration++;
                if (this.iteration < 100 && this.iteration % 10 == 0)
                {
                    this.tele_anim_count++;
                    if (this.tele_anim_count > 10)
                    {
                        this.tele_anim_count = 0;
                    }
                }
                else if (this.iteration % 10 == 0)
                {
                    this.tele_anim_count++;
                    if (this.tele_anim_count > 10)
                    {
                        this.tele_anim_count = 9;
                    }
                }
                if (this.iteration > 70)
                {
                    this.playerinfo.y -= (int)(Game1.GameWidth / 400);
                    if (this.playerinfo.y + this.playerinfo.height < -(int)(Game1.GameWidth / 4))
                    {
                        this.reset();
                        this.ToNextFunc();
                    }
                }
                
            }
            else
            {
                if (this.finished)
                {
                    this.bye.update();
                    this.next_level.update();

                    if (this.next_level.active)
                    {
                        this.go_to_next = true;
                        this.iteration = 0;
                        this.anim_pos_y = this.playerinfo.y;
                    }
                }
                if (this.gamex < (int)(Game1.GameWidth / 5))
                {
                    this.gamex += (int)(Game1.GameWidth / 80);
                }
                else
                {
                    if (this.overx > (int)(Game1.GameWidth / 2))
                    {
                        this.overx -= (int)(Game1.GameWidth / 80);
                    }
                    else
                    {
                        if (this.scorey > (int)(Game1.GameHeight / 2.5))
                        {
                            this.scorey -= (int)(Game1.GameWidth / 80);
                        }
                        else
                        {
                            if (this.scorecounter < this.playerinfo.score)
                            {
                                this.scorecounter += (this.playerinfo.score / 60);
                            }
                            else
                            {
                                this.scorecounter = this.playerinfo.score;
                                this.finished = true;
                            }

                        }
                    }
                }
            }
        }
    }
}