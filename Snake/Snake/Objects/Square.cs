using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Objects
{
    class Square
    {
        // DEFAULTS
        /* -------- */
        protected int DEF_POS_X = 0;
        protected int DEF_POS_Y = 0;
        protected int DEF_WIDTH = 10;
        protected int DEF_HEIGHT = 10;
        protected Color DEF_COLOR = Color.Black;

        // PROPERTIES
        /* ---------- */
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }
        public PictureBox Sprite { get; set; }
        public Panel Container { get; set; }
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

        // CONSTRUCTORS
        /* ------------ */
        // Default constructor
        // => Expects one parameter defining
        // the parent container of the square
        public Square(Panel _container)
        {
            // Set container
            Container = _container;
            // Create sprite
            Sprite = new PictureBox();
            // Configure sprite
            X = DEF_POS_X;
            Y = DEF_POS_Y;
            Width = DEF_WIDTH;
            Height = DEF_HEIGHT;
            // Update sprite
            Update();
            // Add sprite to container
            Container.Controls.Add(Sprite);
        }

        // Customize constructor
        // => Expects two parameters:
        // _container = Panel control, to host the sprite
        // _color = Color of type Color
        //   > System.Color.Red
        //   > System.Color.FromARGB(255, 0, 0)
        // _options = Dictionary with options
        //   > _options =>
        //   > X, Y, Width, Height => "1", "200", etc.
        //   > Color => "Black", "Green", etc.
        public Square(Panel _container, Dictionary<string, string> _options)
        {
            // Set container
            Container = _container;

            // Create sprite
            Sprite = new PictureBox();

            // Configure sprite
            X = (_options.ContainsKey("X") && _options["X"] != null) ? Convert.ToInt32(_options["X"]) : DEF_POS_X;
            Y = (_options.ContainsKey("Y") && _options["Y"] != null) ? Convert.ToInt32(_options["Y"]) : DEF_POS_Y;
            Color = (_options.ContainsKey("Color") && _options["Color"] != null) ? Color.FromName(_options["Color"]) : DEF_COLOR;
            Width = (_options.ContainsKey("Width") && _options["Width"] != null) ? Convert.ToInt32(_options["Width"]) : DEF_WIDTH;
            Height = (_options.ContainsKey("Height") && _options["Height"] != null) ? Convert.ToInt32(_options["Height"]) : DEF_HEIGHT;

            // Update sprite
            Update();

            // Add sprite to container
            Container.Controls.Add(Sprite);
        }

        // Customize constructor
        // => Expects two parameters:
        // _container = Panel control, to host the sprite
        // _color = Color of type Color
        //   > System.Color.Red
        //   > System.Color.FromARGB(255, 0, 0)
        // _options = Dictionary with options
        //   > _options =>
        //   > X, Y, Width, Height => "1", "200", etc.
        //   > Color => "Black", "Green", etc.
        public Square(Panel _container, Color _color, Dictionary<string, string> _options)
        {
            // Set container
            Container = _container;

            // Create sprite
            Sprite = new PictureBox();

            // Set color
            Console.WriteLine(_color);
            Color = _color;

            // Configure sprite
            X = (_options.ContainsKey("X") && _options["X"] != null) ? Convert.ToInt32(_options["X"]) : DEF_POS_X;
            Y = (_options.ContainsKey("Y") && _options["Y"] != null) ? Convert.ToInt32(_options["Y"]) : DEF_POS_Y;
            Width = (_options.ContainsKey("Width") && _options["Width"] != null) ? Convert.ToInt32(_options["Width"]) : DEF_WIDTH;
            Height = (_options.ContainsKey("Height") && _options["Height"] != null) ? Convert.ToInt32(_options["Height"]) : DEF_HEIGHT;
            
            // Update sprite
            Update();

            // Add sprite to container
            Container.Controls.Add(Sprite);
        }

        // DESTRUCTOR
        /* ---------- */
        ~Square()
        {
        }

        // METHODS
        /* ------- */
        public void Show()
        {
            if (Sprite == null || Container == null)
            {
                return;
            }

            Sprite.Show();
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

        public void Update()
        {
            if (Sprite == null || Container == null)
            {
                return;
            }

            if (Sprite.BackColor != Color) { Sprite.BackColor = Color; }
            if (Sprite.Location.X != X || Sprite.Location.Y != Y) { Sprite.Location = new Point(X, Y); }
            if (Width != Sprite.Size.Width || Height != Sprite.Size.Height) { Sprite.Size = new Size(Width, Height); }
        }
    }
}
