using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Snake.Objects
{

    class Player
    {
        // DEFAULTS
        /* -------- */
        private int DEF_SIZE = 20;
        private int DEF_SPEED = 20;
        private string DEF_HEAD_COLOR = "e74c3c";
        private string DEF_BODY_COLOR = "2ecc71";

        // FIELDS
        /* ------ */
        private List<Square> Body = new List<Square>();

        // PROPERTIES
        /* ---------- */
        public int Size { get; set; }
        public int Speed { get; set; }
        public int SpeedX { get; set; } // [-1, 0, 1]
        public int SpeedY { get; set; } // [-1, 0, 1]
        public bool Alive { get; private set; }
        public int Length
        {
            get
            {
                return Body.Count();
            }
        }
        public Panel Container { get; set; }
        public Square Head
        {
            get
            {
                return Body.First();
            }
        }
        public List<Square> Tail
        {
            get
            {
                List<Square> tail = new List<Square>(Body);
                tail.RemoveAt(0);
                return tail;
            }
        }

        // DEV
        public string Info
        {
            get
            {
                return "Length: " + Length + "\n" +
                    "X: " + Head.X + " / Y: " + Head.Y + "\n" +
                    "Tail length: " + Tail.Count() + "\n" +
                    "Speed: " + Speed + "\n" +
                    "SpeedX: " + SpeedX + "\n" +
                    "SpeedY: " + SpeedY + "\n" +
                    "Alive: " + Alive.ToString();
            }
        }

        // CONSTRUCTORS
        /* ------------ */
        // Default
        public Player(Panel _container)
        {
            // Set container
            Container = _container;

            // Set appearance
            Size = DEF_SIZE;
            Speed = DEF_SPEED;

            // Set initial movement direction
            SpeedX = 1;
            SpeedY = 0;
            /*
            int _dir = Helpers.RandomInt(new int[] { 0, 1, 2, 3 });
            // 0 = left; 1 = right; 2 = up; 3 = down
            switch (_dir)
            {
                case 0: SpeedX = -1 * Speed; break;
                case 1: SpeedX = 1 * Speed; break;
                case 2: SpeedY = -1 * Speed; break;
                case 3: SpeedY = 1 * Speed; break;
            }
            */

            // Make up the color
            Dictionary<string, int> _hexCol = Helpers.HexToRgb(DEF_HEAD_COLOR);
            Color HeadColor = Color.FromArgb(_hexCol["Red"], _hexCol["Green"], _hexCol["Blue"]);

            // Configure the first square aka the Head 
            Dictionary<string, string> Config = new Dictionary<string, string>(); // Config container
            // Start from the center of the container
            //int _xCenter = Convert.ToInt32(Container.Width / 2 - Size / 2);
            //int _yCenter = Convert.ToInt32(Container.Height / 2 - Size / 2);
            int _X = 0; // Helpers.MakeEven(_xCenter);
            int _Y = 0; // Helpers.MakeEven(_yCenter);
            Config.Add("X", _X.ToString());
            Config.Add("Y", _Y.ToString());
            Config.Add("Width", Size.ToString());
            Config.Add("Height", Size.ToString());
            Config.Add("Color", DEF_HEAD_COLOR);

            // Create new square for the head and append to the body of the snake
            Body.Add(new Square(Container, HeadColor, Config));

            // Set alive status
            Alive = true;
        }

        // Custom Size constructor
        public Player(Panel _container, string _size)
        {
            // Get container
            Container = _container;

            // Set appearance
            Console.WriteLine(_size);
            Size = Helpers.SizeToInt(_size);
            Speed = Size;

            // Set initial movement direction
            SpeedX = 1 * Speed;
            SpeedY = 0;

            // Make up the color
            Dictionary<string, int> _hexCol = Helpers.HexToRgb(DEF_HEAD_COLOR);
            Color HeadColor = Color.FromArgb(_hexCol["Red"], _hexCol["Green"], _hexCol["Blue"]);

            // Configure the first square aka the Head 
            Dictionary<string, string> Config = new Dictionary<string, string>(); // Config container
            // Start from the center of the container
            //int _xCenter = Convert.ToInt32(Container.Width / 2 - Size / 2);
            //int _yCenter = Convert.ToInt32(Container.Height / 2 - Size / 2);
            int _X = 0; // Helpers.MakeEven(_xCenter);
            int _Y = 0; // Helpers.MakeEven(_yCenter);
            Config.Add("X", _X.ToString());
            Config.Add("Y", _Y.ToString());
            Config.Add("Width", Size.ToString());
            Config.Add("Height", Size.ToString());

            // Create new square for the head and append to the body of the snake
            Body.Add(new Square(Container, HeadColor, Config));

            // Set alive status
            Alive = true;
        }

        // METHODS
        /* ------- */
        // Move 
        // => Move the snake with accordance to the direction and speed set
        public void Move()
        {
            // If the body of the snake is not only the head
            if (Body.Count() > 1)
            {
                // Loop backwards through the body elements, except the head
                // and move each on the place of the one before it
                for (int i = Body.Count() - 1; i > 0; i--)
                {
                    Body.ElementAt(i).Move(Body.ElementAt(i - 1).Position);
                }
            }

            // Finally move the head
            Head.Move(SpeedX, SpeedY);

            // Check for collision and invoke Kill()
            if (Head.X < 0) { Head.X = 0; Head.Update(); Kill(); }
            if (Head.X > Container.Width - Head.Width) { Head.X = Container.Width - Head.Width; Head.Update(); Kill(); }
            if (Head.Y < 0) { Head.Y = 0; Head.Update(); Kill(); }
            if (Head.Y > Container.Height - Head.Height) { Head.Y = Container.Height - Head.Height; Head.Update(); Kill(); }
        }

        // Direction 
        // => Change the direction in which the snake is moving
        // _dir = ["left", "right", "up", "down", "Left", "Right", "Up", "Down"]
        public void Direction(string _dir)
        {
            // Check for valid direction before assigning
            switch (_dir)
            {
                case "left":
                case "Left":
                    if (SpeedX > 0) { break; } // Can't change Right -> Left
                    SpeedX = Speed * -1;
                    SpeedY = 0;
                    break;
                case "right":
                case "Right":
                    if (SpeedX < 0) { break; } // Can't change Left -> Right
                    SpeedX = Speed * 1;
                    SpeedY = 0;
                    break;
                case "up":
                case "Up":
                    if (SpeedY > 0) { break; } // Can't change Down -> Up
                    SpeedX = 0;
                    SpeedY = Speed * -1;
                    break;
                case "down":
                case "Down":
                    if (SpeedY < 0) { break; } // Can't change Up -> Down
                    SpeedX = 0;
                    SpeedY = Speed * 1;
                    break;
            }
        }

        // Grow
        // => Add one more square to (the back of) the body of the snake
        public void Grow()
        {
            // ****************
            // DEVELOPMENT ONLY
            // Set colors available for the tail parts
            // string[] Colors = new string[] { "Blue", "Black", "Red", "Pink" };
            // DEVELOPMENT ONLY
            // ****************

            // Define some necessary variables
            int newX = 0; // New body part's X
            int newY = 0; // New body part's Y

            // If this is the girst part of the tail handle it special
            if (Tail.Count() == 0)
            {
                if (SpeedX > 0) // Snake moves right
                { newX = Head.X - Head.Width; newY = Head.Y; }
                else if (SpeedX < 0) // Snake moves left
                { newX = Head.X + Head.Width; newY = Head.Y; }
                else if (SpeedY > 0) // Snake moves down
                { newY = Head.Y - Head.Height; newX = Head.X; }
                else if (SpeedY < 0) // Snake moves up
                { newY = Head.Y + Head.Height; newX = Head.X; }
            }
            else
            {
                // Calculate where the new part should be added
                int deltaX;
                int deltaY;
                deltaX = Body.Last().X - Body.ElementAt(Body.Count() - 1).X;
                deltaY = Body.Last().Y - Body.ElementAt(Body.Count() - 1).Y;
                /* DeltaX & DeltaY
                 * ---------------
                 * deltaX == 0 => the last two parts have the same X position
                 * deltaX  < 0 => the last part is to the left of the before last
                 * deltaX  > 0 => the last part is to the right of the before last
                 * deltaY == 0 => the last two parts have the same Y position
                 * deltaY  > 0 => the last part is below the before last
                 * deltaY  < 0 => the last part is above the before last
                 */
                if (deltaX == 0) { newX = Body.Last().X; }
                else if (deltaX < 0) { newX = Body.Last().X - Body.Last().Width; }
                else if (deltaX > 0) { newX = Body.Last().X + Body.Last().Width; }
                if (deltaY == 0) { newY = Body.Last().Y; }
                else if (deltaY < 0) { newY = Body.Last().Y - Body.Last().Height; }
                else if (deltaY > 0) { newY = Body.Last().Y + Body.Last().Height; }
            }

            // Make up new color
            Dictionary<string, int> _hexCol = Helpers.HexToRgb(DEF_BODY_COLOR);
            Color BodyColor = Color.FromArgb(_hexCol["Red"], _hexCol["Green"], _hexCol["Blue"]);

            // Make a configuration for the new part
            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("X", newX.ToString());
            Config.Add("Y", newY.ToString());
            Config.Add("Width", Size.ToString());
            Config.Add("Height", Size.ToString());

            // Create the new body part
            Body.Add(new Square(Container, BodyColor, Config));
        }

        // Update
        // => Call to update the position of every square in the body
        // and ensure correct drawing on the screen
        public void Update()
        {
            foreach (var Part in Body)
            {
                Part.Update();
            }
        }

        // Kill
        // => Stop the game and display result aka Length
        public void Kill()
        {
            // Movement depends on the Speed property
            Speed = 0; // No speed = no movement. Check
            Alive = false; // Declare dead

            // Add text with score
            Label ResultLabel = new Label
            {
                ForeColor = Color.Red,
                Text = "Game Over! Your result is: " + Length,
                AutoSize = true
            };
            ResultLabel.Location = new Point(Container.Width / 2 - ResultLabel.Width / 2, Container.Height / 2 - ResultLabel.Height / 2);

            Container.Controls.Add(ResultLabel);
            Container.Refresh();
        }
    }
}
