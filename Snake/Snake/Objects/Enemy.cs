using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace Snake.Objects
{
    class Enemy
    {
        public int Size { get; set; }
        public Panel Container { get; set; }
        public Character Sprite { get; set; }
        public int max_score { get; set; }

        public Enemy(Panel _container)
        {
            Container = _container;
        }
        public Enemy(Panel _container, int _x, int _y,int _width,int _height,string _image_r,string _image_l)
        {
            Container = _container;


            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("X", _x.ToString());
            Config.Add("Y", _y.ToString());
            Config.Add("Width", _width.ToString());
            Config.Add("Height", _height.ToString());
            Config.Add("ImageR", _image_r.ToString());
            Config.Add("ImageL", _image_l.ToString());
            // Create new square for the head and append to the body of the snake
            Sprite = new Character(Container, Config);
        }

        public void createCharacter(int _x, int _y, int _width, int _height, string _image_r, string _image_l)
        {

            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("X", _x.ToString());
            Config.Add("Y", _y.ToString());
            Config.Add("Width", _width.ToString());
            Config.Add("Height", _height.ToString());
            Config.Add("ImageR", _image_r.ToString());
            Config.Add("ImageL", _image_l.ToString());
            // Create new square for the head and append to the body of the snake
            Sprite = new Character(Container, Config);
        }

        public void RandomMove()
        {
            Sprite.RandomMove();
        }
        public void Update()
        {
            Sprite.Update();
        }
        public void RollBack()
        {
            Sprite.RollBack();
        }

        public void Disable()
        {
            Sprite.Sprite.Enabled = false;
        }
        public void Enable()
        {
            Sprite.Sprite.Enabled = true;
        }

        public void ReverseEnable()
        {
            Sprite.Sprite.Enabled = !Sprite.Sprite.Enabled;
        }

        public void Destroy()
        {
            Container.Controls.Remove(Sprite.Sprite);
            Container.Refresh();
        }

    }
}
