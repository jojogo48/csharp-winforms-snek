using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Snake.Objects
{
    class Food
    {
        // DEFAULTS
        /* -------- */
        private int DEF_SIZE = Helpers.SizeToInt("Medium");
        private string DEF_COLOR = "8e44ad";

        // PROPERTIES
        /* ---------- */
        public int Size { get; set; }
        public Square Sprite { get; set; }
        public Panel Container { get; set; }
        private int Index { get; set; }

        // CONSTRUCTORS
        /* ------------ */
        // Default
        public Food(Panel _container, int _x, int _y)
        {
            // Set container
            Container = _container;

            // Set appearance
            Size = DEF_SIZE;

            // Make configuration for sprite
            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("X", _x.ToString());
            Config.Add("Y", _y.ToString());
            Config.Add("Width", Size.ToString());
            Config.Add("Height", Size.ToString());
            Config.Add("Color", DEF_COLOR);

            // Make up the color
            Dictionary<string, int> _hexCol = Helpers.HexToRgb(DEF_COLOR);
            Color FoodColor = Color.FromArgb(_hexCol["Red"], _hexCol["Green"], _hexCol["Blue"]);

            // Create new square for the head and append to the body of the snake
            Sprite = new Square(Container, FoodColor, Config);
            Index = Container.Controls.IndexOf(Sprite.Sprite);
            Console.WriteLine("food" + Index);
        }

        // Customize constructor color
        public Food(Panel _container, int _x, int _y, Color _color)
        {
            // Set container
            Container = _container;

            // Set appearance
            Size = DEF_SIZE;

            // Make configuration for sprite
            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("X", _x.ToString());
            Config.Add("Y", _y.ToString());
            Config.Add("Width", Size.ToString());
            Config.Add("Height", Size.ToString());
            Config.Add("Color", _color.ToKnownColor().ToString());

            // Create new square for the head and append to the body of the snake
            Sprite = new Square(Container, Config);
            Index = Container.Controls.IndexOf(Sprite.Sprite);
            Console.WriteLine("food" + Index);
        }

        // Customize constructor 2
        public Food(Panel _container, Color _color, int _size)
        {

        }

        // METHODS
        /* ------- */
        public void Destroy()
        {
            Console.WriteLine("food" + Index);
            Container.Controls.Remove(Sprite.Sprite);
            Container.Refresh();
        }


    }
}
