using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Objects
{
    class Character
    {

        protected int DEF_POS_X = Properties.Settings.Default.DefaultX;
        protected int DEF_POS_Y = Properties.Settings.Default.DefaultY;
        protected int DEF_WIDTH = Properties.Settings.Default.DefaultWidth;
        protected int DEF_HEIGHT = Properties.Settings.Default.DefaultHeight;
        protected int DEF_MOVE_SIZE = Properties.Settings.Default.DefaultMoveSize;
        public int WalkCounter { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String PictureR { get; set; }
        public String PictureL { get; set; }
        public PictureBox Sprite { get; set; }
        public Panel Container { get; set; }

        private int last_move_x { get; set; }
        private int last_move_y { get; set; }
        public Dictionary<string, int> Size
        {
            get
            {
                Dictionary<string, int> _size = new Dictionary<string, int>
                {
                    { "Width", Width },
                    { "Height", Height }
                };
                return _size;
            }
            set
            {
                Width = value["Width"];
                Height = value["Height"];
            }
        }
        public Dictionary<string, int> Position
        {
            get
            {
                Dictionary<string, int> _pos = new Dictionary<string, int>
                {
                    { "X", X },
                    { "Y", Y }
                };
                return _pos;
            }
            set
            {
                X = value["X"];
                Y = value["Y"];
            }
        }

        public Character(Panel _container, Dictionary<string, string> _options)
        {
            // Set container
            Container = _container;

            // Create sprite
            Sprite = new PictureBox();

            // Configure sprite
            X = (_options.ContainsKey("X") && _options["X"] != null) ? Convert.ToInt32(_options["X"]) : DEF_POS_X;
            Y = (_options.ContainsKey("Y") && _options["Y"] != null) ? Convert.ToInt32(_options["Y"]) : DEF_POS_Y;
            PictureR = (_options.ContainsKey("ImageR") && _options["ImageR"] != null) ?Properties.Settings.Default.Media + _options["ImageR"] : null;

            PictureL = (_options.ContainsKey("ImageL") && _options["ImageL"] != null) ? Properties.Settings.Default.Media + _options["ImageL"] : null;
            //Picture = (_options.ContainsKey("Image") && _options["Image"] != null) ? Image.FromStream(fs) : null;
            Width = (_options.ContainsKey("Width") && _options["Width"] != null) ? Convert.ToInt32(_options["Width"]) : DEF_WIDTH;
            Height = (_options.ContainsKey("Height") && _options["Height"] != null) ? Convert.ToInt32(_options["Height"]) : DEF_HEIGHT;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            Sprite.ImageLocation = PictureR;
            // Update sprite
            Update();

            // Add sprite to container
            Container.Controls.Add(Sprite);
        }


        public void Move(Dictionary<string, int> _pos)
        {
            Position = _pos;
        }

        public void Move(int _x, int _y)
        {
            X += _x;
            Y += _y;
        }

        public void RandomMove()
        {

            int origin_x = X;
            int origin_y = Y;
            if (WalkCounter != 0)
            {
                X += Dx;
                Y += Dy;
                WalkCounter--;
            }
            else
            {

                int direction = Helpers.RandomInt(0, 8);
                WalkCounter = Helpers.RandomInt(0, 10);
                switch (direction)
                {
                    case 0:
                        Dx = DEF_MOVE_SIZE;
                        Dy = 0;
                        Sprite.ImageLocation = PictureR;
                        break;
                    case 1:
                        Dx = DEF_MOVE_SIZE;
                        Dy = DEF_MOVE_SIZE;
                        Sprite.ImageLocation = PictureR;
                        
                        break;
                    case 2:
                        Dx = 0;
                        Dy = DEF_MOVE_SIZE;
                        break;
                    case 3:
                        Dx = -DEF_MOVE_SIZE;
                        Dy = DEF_MOVE_SIZE;
                        Sprite.ImageLocation = PictureL;
                        break;
                    case 4:
                        Dx = -DEF_MOVE_SIZE;
                        Dy = 0;
                        Sprite.ImageLocation = PictureL;
                        break;
                    case 5:
                        Dx = -DEF_MOVE_SIZE;
                        Dy = -DEF_MOVE_SIZE;
                        Sprite.ImageLocation = PictureL;
                        break;
                    case 6:
                        Dx = 0;
                        Dy = -DEF_MOVE_SIZE;
                        break;
                    case 7:
                        Dx = DEF_MOVE_SIZE;
                        Dy = -DEF_MOVE_SIZE;
                        Sprite.ImageLocation = PictureR;
                        break;
                    default: break;
                }

            }

            if (X+ Width >= Properties.Settings.Default.WindowWidth) X = Properties.Settings.Default.WindowWidth- Width;
            if (X < 0) X = 0;
            if (Y+Height >= Properties.Settings.Default.WindowHeight) Y = Properties.Settings.Default.WindowHeight-Height;
            if (Y < 0) Y = 0;

            last_move_x = X - origin_x;
            last_move_y = Y - origin_y;
        }

        public void RollBack()
        {
            X -= last_move_x;
            Y -= last_move_y;
        }
        public void Update()
        {
            if (Sprite == null || Container == null)
            {
                return;
            }

            if (Sprite.Location.X != X || Sprite.Location.Y != Y) { Sprite.Location = new Point(X, Y); }
            if (Width != Sprite.Size.Width || Height != Sprite.Size.Height) { Sprite.Size = new Size(Width, Height); }
        }

        public void Show()
        {
            if (Sprite == null || Container == null)
            {
                return;
            }

            Sprite.Show();
        }
    }
}
