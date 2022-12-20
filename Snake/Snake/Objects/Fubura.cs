using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Objects
{
    class Fubura
    {
        private int image_width = 164;
        private int image_height = 110;
        public Enemy enemy { get; set; }
        public Fubura(Panel _container)
        {
            
            int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth - image_width);
            int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - image_height);

            enemy = new Enemy(_container, xPos, yPos, 164, 110, "Fubuzilla_2x_R.gif", "Fubuzilla_2x_L.gif");
        }

        public void RandomMove()
        {
            enemy.RandomMove();
        }
        
        public void Update()
        {
            enemy.Update();
        }

        public void Destroy()
        {
            enemy.Destroy();
        }
    }
}
