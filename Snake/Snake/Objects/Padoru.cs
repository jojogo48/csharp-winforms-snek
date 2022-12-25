using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Objects
{
    class Padoru:Enemy
    {
        private int image_width = 121;
        private int image_height = 126;
        public Padoru(Panel _container):base(_container)
        {

            int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth - image_width);
            int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - image_height);
            max_score = 1;
            base.createCharacter(xPos, yPos, image_width, image_height, "padoru_R.gif", "padoru_L.gif");
        }
    }
}
