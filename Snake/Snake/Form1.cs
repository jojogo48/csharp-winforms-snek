using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snake.Objects;

namespace Snake
{
    //測試用
    public partial class Form1 : Form
    {
        private Player Snake;
        private Food Point;
        private Fubura enemy;
        public Form1()
        {
            InitializeComponent();
        }

        // Game Environment & Loop
        //
        private void MainGame(Object sender, KeyPressEventArgs e)
        {
            if (Snake.Alive == false) { gameTimer.Stop(); return; }

            switch (e.KeyChar)
            {
                case 'w':
                    Snake.Direction("up");
                    break;
                case 's':
                    Snake.Direction("down");
                    break;
                case 'a':
                    Snake.Direction("left");
                    break;
                case 'd':
                    Snake.Direction("right");
                    break;
            }

            Snake.Update();
            //label1.Text = Snake.Info();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
    
            // If Snake is not alive show announcement, stop timer and return
            if (Snake.Alive == false)
            {
                gameTimer.Stop();
                enemyTimer.Stop();
                announceLabel.Text = Helpers.AnnouncementText;
                announceLabel.BackColor = Helpers.AnnouncementBGC;
                announceLabel.ForeColor = Helpers.AnnouncementFGC;
                announceLabel.Visible = true;
                Console.WriteLine("Snek dead. announceLabel.Visible = " + announceLabel.BackColor);
                return;
            }

            // Check for collision between snake head and point
            if (Point != null && Helpers.Collides(Snake.Head, Point.Sprite))
            {
                Console.WriteLine("Collision detected!");
                Console.WriteLine("Collision at: Snake(X: {0}, Y: {1}) and Point(X: {2}, Y: {3})", Snake.Head.X, Snake.Head.Y, Point.Sprite.X, Point.Sprite.Y);
                Point.Destroy();
                Point = null;
                Snake.Grow();
            }

            if (enemy != null && Helpers.Collides(Snake.Head, enemy.enemy.Sprite))
            {
                Console.WriteLine("Collision detected!");
                Console.WriteLine("Collision at: Snake(X: {0}, Y: {1}) and Point(X: {2}, Y: {3})", Snake.Head.X, Snake.Head.Y, Point.Sprite.X, Point.Sprite.Y);
                
                Form2 f = new Form2();
                gameTimer.Enabled = false;
                enemyTimer.Enabled = false;
                enemy.enemy.Sprite.Sprite.Enabled = false;
                //使用ShowDialog()方法使 f 以強制回應形式顯示表單
                f.ShowDialog(this);
                if (f.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {

                    Console.WriteLine("cancel");
                }
                gameTimer.Enabled = true;
                enemyTimer.Enabled = true;

                enemy.enemy.Sprite.Sprite.Enabled = true;
                enemy.Destroy();
                enemy = null;

            }


            // Check for collision between snake head and snake body
            if (Snake.Tail.Count() != 0)
            {
                foreach (var part in Snake.Tail)
                {
                    if (Helpers.Collides(Snake.Head, part))
                    {
                        Snake.Kill();
                    }
                }
            }

            // If there is no point create one
            if (Point == null)
            {
                int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth-10);
                int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - 10);
                Point = new Food(gameContainer, xPos, yPos);
            }

            if (enemy == null)
            {
                enemy = new Fubura(gameContainer);
            }
            // If Snake is alive keep it moving

            Snake.Move();
            Snake.Update();
            scoreLabel.Text = "Score: " + (Snake.Length - 1);
        }

        // Button Click Handlers
        //
        private void StartBtn_Click(object sender, EventArgs e)
        {
            // Clear the Game Container
            for (int i = gameContainer.Controls.Count - 1; i >= 0; i--)
            {
                if (gameContainer.Controls[i] as PictureBox != null) gameContainer.Controls[i].Dispose();
            }

            // Hide the announcement panel
            announceLabel.Visible = false;

            // Create new Snake
            string size = "Medium"; // DEV PURPOSES
            Snake = new Player(gameContainer, size);

            // Create new Food
            int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth - 10);
            int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - 10);
            Point = new Food(gameContainer, xPos, yPos);
            enemy = new Fubura(gameContainer);
            // Setup and Start Game Timer
            string difficulty = "Medium";
            gameTimer.Interval = Helpers.DifficultyToInt(difficulty);
            enemyTimer.Interval = Helpers.DifficultyToInt(difficulty);
            gameTimer.Start();
            enemyTimer.Start();
            // Add KeyPressEvent
            (gameContainer as Control).KeyPress += new KeyPressEventHandler(MainGame);

            // Focus Game Container
            gameContainer.Focus();
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            // If the snake exists and is alive then you can pause/continue
            if (Snake != null && Snake.Alive)
            {
                // Pause/Continue the timer
                gameTimer.Enabled = !gameTimer.Enabled;
                enemyTimer.Enabled = !enemyTimer.Enabled;

                enemy.enemy.Sprite.Sprite.Enabled = !enemy.enemy.Sprite.Sprite.Enabled;
                // Set the text in the button
                pauseBtn.Text = (gameTimer.Enabled) ? "Pause" : "Continue";
            }
            announceLabel.Visible = !announceLabel.Visible;
            // Focus Game Container to remove the ugly border from the button
            gameContainer.Focus();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            // Stop Game Timer
            gameTimer.Stop();
            enemyTimer.Stop();
            // Dispose Snake
            Snake = null;

            // Clear the Game Container
            for (int i = gameContainer.Controls.Count - 1; i >= 0; i--)
            {
                if (gameContainer.Controls[i] as PictureBox != null) gameContainer.Controls[i].Dispose();
            }

            // Remove KeyPressEvent
            (gameContainer as Control).KeyPress -= new KeyPressEventHandler(MainGame);

            // Focus Game Container to remove the ugly border from the button
            gameContainer.Focus();
        }

        // Form Initial Setup
        //
        private void Form1_Load(object sender, EventArgs e)
        {
            // Set Game Container Backgrouond Color
            string hexCol = "2ecc71"; 
            Dictionary<string, int> bgCol = Helpers.HexToRgb(hexCol);
            Color backColor = Color.FromArgb(bgCol["Red"], bgCol["Green"], bgCol["Blue"]);
            gameContainer.BackColor = backColor;

            // Hide announcement
            announceLabel.Hide();
        }

        private void gameContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void enemyTimer_Tick(object sender, EventArgs e)
        {
            if(enemy!=null)
            {
                do
                {
                    enemy.RandomMove();
                } while (Helpers.Collides(enemy.enemy.Sprite, Point.Sprite));
                enemy.Update();
            }
        }
    }
}
