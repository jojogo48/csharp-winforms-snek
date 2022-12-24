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
        private List<Enemy> Enemys = new List<Enemy>();
        private SnakeEntities db;
        private Random Rander = new Random();
        private int question_size = 0;

        public Form1()
        {
            InitializeComponent();
        }

        // Game Environment & Loop
        //
        private void MainGame(Object sender, KeyPressEventArgs e)
        {

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

        public void createFood()
        {
            if (Point != null)
            {
                DestroyFood();
            }

            int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth - 10);
            int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - 10);

            Point = new Food(gameContainer, xPos, yPos);

        }


        public void createEnemys()
        {
            if(Enemys.Count<=4)
            {
                if (Helpers.RandomInt(0, 50) == 1)
                {

                    Enemy e;
                    switch (Helpers.RandomInt(0, 2))
                    {
                        case 0:
                            e = new Fubura(gameContainer);
                            while(CheckCollidesEnemy(e))
                            {
                                e.Destroy();
                                e = new Fubura(gameContainer);
                            }
                            /*while (Helpers.Collides(e.Sprite,Snake.Head))
                            {
                                e.Destroy();
                                e = new Fubura(gameContainer);
                            }*/

                            Enemys.Add(e);
                            break;
                        case 1:
                            e = new Padoru(gameContainer);
                            while (CheckCollidesEnemy(e))
                            {
                                e.Destroy();
                                e = new Padoru(gameContainer);
                            }
                            /*while (Helpers.Collides(e.Sprite, Snake.Head))
                            {
                                e.Destroy();
                                e = new Padoru(gameContainer);
                            }*/
                            Enemys.Add(e);
                            break;
                        default:
                            break;
                    }
                    
                }
            }
        }


        public void DestroyFood()
        {
            if(Point !=null)
            {
                Point.Destroy();
                Point = null;
            }
        }


        bool CollidesEvent(Enemy e)
        {
            if (Helpers.Collides(Snake.Head, e.Sprite))
            {
                Console.WriteLine("Collision detected!");
                if (getScore() < e.max_score)
                {
                    Snake.Kill();
                }
                else
                {
                    int score_t = openQuestionForm();
                    if (score_t == 0)
                    {
                        Snake.Kill();
                    }
                    else
                    {
                        for (int i = 0; i < score_t; i++)
                        {
                            Snake.Grow();
                        }
                        e.Destroy();
                        return true;
                    }
                }
            }
            return false;
        }

        void DisableEnemy(Enemy e)
        {
            e.Disable();

        }
        bool DestroyEnemy(Enemy e)
        {
            e.Destroy();
            return true;   
        }
        void EnableEnemy(Enemy e)
        {
            e.Enable();
        }
        void ReverseEnableEnemy(Enemy e)
        {
            e.ReverseEnable();
        }

        public int getScore()
        {
            return (Snake.Length - 1);
        }
        public void ShowScoreAndSave()
        {


            var result = from p in db.Histroy
                         select p.Id;

            Histroy score_t = new Histroy();
            try
            {
                score_t.Id = (result.Max()) + 1;
            }catch(Exception e)
            {
                score_t.Id = 1;
            }
            score_t.Score = getScore();
            score_t.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            db.Histroy.Add(score_t);
            db.SaveChanges();

            var score = from p in db.Histroy
                        select p.Score;

            MessageBox.Show($"獲得分數{getScore()}分\n目前最高分是{score.Max()}", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        public int openQuestionForm()
        {
            int index = Rander.Next(0 + 1, this.question_size + 1);

            var qeustion = (from Question in db.Question
                            where Question.Id == index
                            select Question).FirstOrDefault();

            QuestionForm f = new QuestionForm(qeustion);
            gameTimer.Enabled = false;
            enemyTimer.Enabled = false;
            Enemys.ForEach(DisableEnemy);
            f.ShowDialog(this);
            gameTimer.Enabled = true;
            enemyTimer.Enabled = true;
            Enemys.ForEach(EnableEnemy);
            return f.score;
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
    
            // If Snake is not alive show announcement, stop timer and return
            if (Snake.Alive == false)
            {
                gameTimer.Stop();
                enemyTimer.Stop();
                Enemys.ForEach(DisableEnemy);
                ShowScoreAndSave();
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
                DestroyFood();
                Snake.Grow();
            }

            Enemys.RemoveAll(CollidesEvent);

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
                createFood();
            }
            createEnemys();
            // If Snake is alive keep it moving

            Snake.Move();
            Snake.Update();
            scoreLabel.Text = "Score: " + getScore();
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

            Enemys.RemoveAll(DestroyEnemy);
            createFood();
            createEnemys();

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

                Enemys.ForEach(ReverseEnableEnemy);
                // Set the text in the button
                pauseBtn.Text = (gameTimer.Enabled) ? "Pause" : "Continue";
            }
            //announceLabel.Visible = !announceLabel.Visible;
            // Focus Game Container to remove the ugly border from the button
            gameContainer.Focus();
        }


        private void RankingBtn_Click(object sender, EventArgs e)
        {
            RankingForm f = new RankingForm(db);
            f.ShowDialog(this);
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
            this.db = new SnakeEntities();
            this.question_size = (from p in db.Question
                                  select p.Id).Count();

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

            Enemys.ForEach(EnemyMove);
        }

        bool CheckCollidesEnemy(Enemy e1)
        {
            foreach(var e in Enemys)
            {
                if(e!= e1 && Helpers.Collides(e.Sprite,e1.Sprite))
                {
                    return true;
                }
            }
            return false;
        }
        bool CheckCollidesSnake(Enemy e1)
        {

            foreach (var part in Snake.Tail)
            {
                if (Helpers.Collides(e1.Sprite, part))
                {
                    return true;
                }
            }
            return false;
        }
        void EnemyMove(Enemy e)
        {
            e.RandomMove();
            if(CheckCollidesEnemy(e) || CheckCollidesSnake(e))
            {
                e.RollBack();
            }
            e.Update();
            if (Point!= null && Helpers.Collides(e.Sprite, Point.Sprite))
            {
                DestroyFood();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
