using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Snake.Objects
{
    static class Helpers
    {
        private static Random Rander = new Random();

        private static Dictionary<string, int> Sizes = new Dictionary<string, int>()
        {
            { "Small", 10 },
            { "Medium", 15 },
            { "Large", 20 }
        };

        public static string[] SizeStrings
        {
            get
            {
                return Sizes.Keys.ToArray();
            }
        }

        private static Dictionary<string, int> Difficulties = new Dictionary<string, int>()
        {
            { "Easy", 200 },
            { "Medium", 100 },
            { "Hard", 50 }
        };

        public static string[] DifficultyStrings
        {
            get
            {
                return Difficulties.Keys.ToArray();
            }
        }

        public static string AnnouncementText = "";

        public static Color AnnouncementBGC;

        public static Color AnnouncementFGC;

        // RandomString
        // => Return random string from given array of strings
        public static string RandomString(string[] Options)
        {
            int index = Rander.Next(0, Options.Length);
            return Options[index];
        }

        // RandomInt
        // => Return random int from given array of ints
        public static int RandomInt(int[] Options)
        {
            int index = Rander.Next(0, Options.Length);
            return Options[index];
        }

        // RandomInt
        // => Return random int given inclusive min and max
        public static int RandomInt(int Min, int Max)
        {
            return Rander.Next(Min, Max + 1);
        }

        // MakeEven
        // => Check if a given number is even and if it is not
        // return it as an even number according to Options
        // Options => [ -1, 1 ]
        public static int MakeEven(int Number, int Options = 1)
        {
            // If Options != [ -1, 1 ] return 0 (Invalid input)
            if (Options != -1 && Options != 1) { return 0;  }

            // If number is even return it
            if (Number % 2 == 0) { return Number; }

            // If number is not even make it even by adding Options and return
            return Number + Options;
        }

        // SizeToInt
        // => Use predefined Dictionary with sizes and return the
        // corresponding integer value of a given size name
        public static int SizeToInt(string Size)
        {
            return Sizes[Size];
        }

        // DifficultyToInt
        // => Use predefined Dictionary with difficulties and return the
        // corresponding integer value of a given difficulty name
        public static int DifficultyToInt(string Difficulty)
        {
            return Difficulties[Difficulty];
        }

        // Collides
        // => Check if [Obj1] collides with [Obj2] and return true/false
        public static bool Collides(Square Obj1, Square Obj2)
        {

            int x1_min = Obj1.X;
            int x1_max = Obj1.X + Obj1.Width;
            int y1_min = Obj1.Y;
            int y1_max = Obj1.Y + Obj1.Height;


            int x2_min = Obj2.X;
            int x2_max = Obj2.X + Obj2.Width;
            int y2_min = Obj2.Y;
            int y2_max = Obj2.Y + Obj2.Height;

            if (x1_max > x2_min && x2_max > x1_min && y1_max > y2_min && y2_max > y1_min)
            {
                return true;
            }
            return false;

        }

        public static bool Collides(Square Obj1, Character Obj2)
        {
            

            int x1_min = Obj1.X ;
            int x1_max = Obj1.X + Obj1.Width;
            int y1_min = Obj1.Y;
            int y1_max = Obj1.Y + Obj1.Height;


            int x2_min = Obj2.X;
            int x2_max = Obj2.X + Obj2.Width;
            int y2_min = Obj2.Y;
            int y2_max = Obj2.Y + Obj2.Height;
            
            if(x1_max>x2_min && x2_max > x1_min && y1_max > y2_min && y2_max > y1_min)
            {
                return true;
            }
            return false;


        }
        public static bool Collides(Character Obj1,Square  Obj2)
        {
            int x1_min = Obj1.X;
            int x1_max = Obj1.X + Obj1.Width;
            int y1_min = Obj1.Y;
            int y1_max = Obj1.Y + Obj1.Height;


            int x2_min = Obj2.X;
            int x2_max = Obj2.X + Obj2.Width;
            int y2_min = Obj2.Y;
            int y2_max = Obj2.Y + Obj2.Height;

            if (x1_max > x2_min && x2_max > x1_min && y1_max > y2_min && y2_max > y1_min)
            {
                return true;
            }
            return false;
        }


        public static bool Collides(Character Obj1, Character Obj2)
        {
            int x1_min = Obj1.X;
            int x1_max = Obj1.X + Obj1.Width;
            int y1_min = Obj1.Y;
            int y1_max = Obj1.Y + Obj1.Height;


            int x2_min = Obj2.X;
            int x2_max = Obj2.X + Obj2.Width;
            int y2_min = Obj2.Y;
            int y2_max = Obj2.Y + Obj2.Height;

            if (x1_max > x2_min && x2_max > x1_min && y1_max > y2_min && y2_max > y1_min)
            {
                return true;
            }
            return false;
        }
        // HexToRgb
        // => Transoform a HEX string (w/o the #) to RGB values
        // => return as a dictionary for easier access:
        // HexToRgb("00FF44")["Red"], HexToRgb("00FF44")["Green"], HexToRgb("00FF44")["Blue"],
        public static Dictionary<string, int> HexToRgb(string hexCol)
        {
            // Get the RGB values from the given string
            string _red = hexCol.Substring(0, 2);
            string _green = hexCol.Substring(2, 2);
            string _blue = hexCol.Substring(4, 2);

            // Return RGB converted from hexadecimal to int values
            return new Dictionary<string, int>()
            {
                { "Red", Convert.ToInt32(_red, 16) },
                { "Green", Convert.ToInt32(_green, 16) },
                { "Blue", Convert.ToInt32(_blue, 16) }
            };
        }

        // Announcement
        // => Make a new panel announcement text inside the given container
        public static void Announcement(string Text, string BackColor = "000000", string ForeColor = "ffffff")
        {
            var newBColor = HexToRgb(BackColor); //"f44336"
            var newFColor = HexToRgb(ForeColor);

            AnnouncementText = Text;
            AnnouncementBGC = Color.FromArgb(newBColor["Red"], newBColor["Green"], newBColor["Blue"]);
            AnnouncementFGC = Color.FromArgb(newFColor["Red"], newFColor["Green"], newFColor["Blue"]);
        }
    }
}
