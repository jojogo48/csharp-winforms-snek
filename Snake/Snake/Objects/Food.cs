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
        private int DEF_SIZE = 50;
        //private string DEF_COLOR = "8e44ad";
        private List<string> DEF_COLOR = new List<string>() { "FFFF4A", "0033FF", "FF2828", "000000" }; // 黃 藍 紅 黑
        Random rnd = new Random();
        private int tmp = 0;
        private int colorNum = 0;
        //private string DEF_IMAGE = "food.png";
        // PROPERTIES
        /* ---------- */
        public int Size { get; set; }
        public Square Sprite { get; set; }
        public string foodColor { get; set; }
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
            tmp = rnd.Next(10);

            if(tmp >= 0 && tmp <= 4)
            {
                colorNum = 1;
            }
            else if(tmp >= 5 && tmp <= 6)
            {
                colorNum = 0;
            }
            else if (tmp >= 7 && tmp <= 8)
            {
                colorNum = 3;
            }
            else if (tmp == 9)
            {
                colorNum = 2;
            }

            Config.Add("Color", DEF_COLOR[colorNum]);
            //Config.Add("Image", DEF_IMAGE);
            // Make up the color
            Dictionary<string, int> _hexCol = Helpers.HexToRgb(DEF_COLOR[colorNum]);
            Color FoodColor = Color.FromArgb(_hexCol["Red"], _hexCol["Green"], _hexCol["Blue"]);

            // Create new square for the head and append to the body of the snake
            Sprite = new Square(Container, FoodColor, Config);
            foodColor = DEF_COLOR[colorNum];
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

            //Config.Add("Image", DEF_IMAGE);
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
