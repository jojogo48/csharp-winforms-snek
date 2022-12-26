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
using System.Diagnostics;
using System.Media;

namespace Snake
{
    //測試用
    struct Pt { public int x, y; };
    struct Vr { public Pt start;public Pt end; };
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

            int xPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowWidth - 50);
            int yPos = Helpers.RandomInt(0, Properties.Settings.Default.WindowHeight - 50);

            Point = new Food(gameContainer, xPos, yPos);

        }

        public void createEnemysByFood()
        {
            Enemy e;
            switch (Helpers.RandomInt(0, 5))
            {
                case 0:
                    e = new Fubura(gameContainer);
                    if (CheckCollidesEnemy(e) || CheckCollidesSnake(e))
                    {
                        e.Destroy();
                    }
                    else
                    {
                        Enemys.Add(e);
                    }
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    e = new Padoru(gameContainer);
                    if (CheckCollidesEnemy(e) || CheckCollidesSnake(e))
                    {
                        e.Destroy();
                    }
                    else
                    {
                        Enemys.Add(e);
                    }
                    break;
                default:
                    break;
            }
        }

        public void createEnemys()
        {
            if(Enemys.Count<=4)
            {
                if (Helpers.RandomInt(0, 2) == 1)
                {

                    Enemy e;
                    switch (Helpers.RandomInt(0, 5))
                    {
                        case 0:
                            e = new Fubura(gameContainer);
                            if(CheckCollidesEnemy(e) || CheckCollidesSnake(e))
                            {
                                e.Destroy();
                            }
                            else
                            {
                                Enemys.Add(e);
                            }

           
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            e = new Padoru(gameContainer);
                            if (CheckCollidesEnemy(e) || CheckCollidesSnake(e))
                            {
                                e.Destroy();
                            }
                            else
                            {
                                Enemys.Add(e);
                            }
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
                //Console.WriteLine("Collision detected!");
                if (getScore() < e.max_score)
                {
                    Snake.Kill();
                }
                else
                {
                    int score_t = openQuestionForm(e.max_score);
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
            if(result.Count() == 0)
            {
                score_t.Id = 1;
            }
            else
            {
                score_t.Id = (result.Max()) + 1;
            }
            score_t.Score = getScore();
            score_t.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            db.Histroy.Add(score_t);
            db.SaveChanges();

            var score = from p in db.Histroy
                        select p.Score;

            MessageBox.Show($"獲得分數{getScore()}分\n目前最高分是{score.Max()}", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            changeBgm(0);
        }



        public int openQuestionForm(int enemy_score)
        {
            int index = Rander.Next(0 + 1, this.question_size + 1);

            var qeustion = (from Question in db.Question
                            where Question.Id == index
                            select Question).FirstOrDefault();

            QuestionForm f = new QuestionForm(qeustion, enemy_score);
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
                //Console.WriteLine("Snek dead. announceLabel.Visible = " + announceLabel.BackColor);
                return;
            }

            // Check for collision between snake head and point
            if (Point != null && Helpers.Collides(Snake.Head, Point.Sprite))
            {
                //Console.WriteLine("!!!!!!!!!! " + Point.foodColor);
                if(Point.foodColor == "FF2828") // 紅色 加速
                {
                    gameTimer.Interval -= 3;
                }
                else if (Point.foodColor == "FFFF4A") // 黃色 答對加分
                {
                    if (openQuestionForm(1) == 1)
                    {
                        Snake.Grow();
                    }
                }
                else if (Point.foodColor == "000000") // 黑色 吃了加分 但有機率生成一隻怪
                {
                    Console.WriteLine("Create eneny by food.");
                    createEnemysByFood();
                    Snake.Grow();
                }
                else // 藍色 單純加分沒事
                {
                    Snake.Grow();
                }
                DestroyFood();
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
            checkCircleAndKillEnemy();
        }

        public void changeBgm(int index)
        {
            
            switch(index)
            {
                case 0:
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.URL = Properties.Settings.Default.Media + "跑跑卡丁車 聖誕BGM 聖誕飄移.mp3";
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    break;
                case 1:
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.URL = Properties.Settings.Default.Media + "Padoru_Padoru.wav";
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    break;
                default:
                    break;
            }
            
            
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
            changeBgm(1);
            // Hide the announcement panel
            announceLabel.Visible = false;
            string size = "Small";
            string difficulty = "Easy";
            if (radioButton1.Checked)
            {
                size = "Small";
                difficulty = "Easy";
            }
            if (radioButton2.Checked)
            {
                size = "Medium";
                difficulty = "Medium";
            }
            if (radioButton3.Checked)
            {
                size = "Large";
                difficulty = "Hard";
            }
            // Create new Snake
            // DEV PURPOSES
            Snake = new Player(gameContainer, size);

            Enemys.RemoveAll(DestroyEnemy);
            createFood();
            createEnemys();


            // Setup and Start Game Timer
            
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
                if(gameTimer.Enabled)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
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
            changeBgm(0);
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

        public void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if(gameTimer.Enabled)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        // Form Initial Setup
        //
        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = Properties.Settings.Default.Media + "跑跑卡丁車 聖誕BGM 聖誕飄移.mp3";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.PlayStateChange += player_PlayStateChange;
            axWindowsMediaPlayer1.settings.volume = 20;
            trackBar1.Value = axWindowsMediaPlayer1.settings.volume;
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

       

        bool checkIntersection(Pt a1, Pt a2, Pt b1,Pt b2)
        {
            bool is_inter_x = false;
            Pt a_x_max, a_x_min;
            if (a2.x > a1.x)
            {
                a_x_max = a2;
                a_x_min = a1;
            }
            else
            {
                a_x_max = a1;
                a_x_min = a2;
            }



            Pt b_x_max, b_x_min;
            if (b2.x > b1.x)
            {
                b_x_max = b2;
                b_x_min = b1;
            }
            else
            {
                b_x_max = b1;
                b_x_min = b2;
            }

            int inter_x_max = (a_x_max.x > b_x_max.x) ? b_x_max.x : a_x_max.x;
            int inter_x_min = (a_x_min.x > b_x_min.x) ? a_x_min.x : b_x_min.x;
            if (inter_x_max>= inter_x_min)
            {
                is_inter_x = true;
            }

            bool is_inter_y = false;

            Pt a_y_max, a_y_min;
            if (a2.y > a1.y)
            {
                a_y_max = a2;
                a_y_min = a1;
            }
            else
            {
                a_y_max = a1;
                a_y_min = a2;
            }
            Pt b_y_max, b_y_min;
            if (b2.y > b1.y)
            {
                b_y_max = b2;
                b_y_min = b1;
            }
            else
            {
                b_y_max = b1;
                b_y_min = b2;
            }

            int inter_y_max = (a_y_max.y > b_y_max.y) ? b_y_max.y : a_y_max.y;
            int inter_y_min = (a_y_min.y > b_y_min.y) ? a_y_min.y : b_y_min.y;
            if (inter_y_max >= inter_y_min)
            {
                is_inter_y = true;
            }
            if (is_inter_x && is_inter_y) return true;
            return false;
        }

        bool checkInBound(Enemy e, List<Vr> lines)
        {
            var start_t = Stopwatch.GetTimestamp();
            Pt a1, a2;
            a1.x = e.Sprite.X;
            a1.y = e.Sprite.Y;

            a2.x = Properties.Settings.Default.WindowWidth;
            a2.y = e.Sprite.Y;



            int count = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                Pt b1, b2;
                b1.x = lines[i].start.x;
                b1.y = lines[i].start.y;
                b2.x = lines[i].end.x;
                b2.y = lines[i].end.y;

                if (checkIntersection(a1, a2, b1, b2))
                {
                    count++;

                }
            }
            /*if (count != 0)
            {
                for (int i = 0; i < lines.Count(); i++)
                {
                    Console.WriteLine($"{lines[i].start.x}  {lines[i].start.y}  {lines[i].end.x}   {lines[i].end.y}");
                }
                    gameTimer.Enabled = false;
                MessageBox.Show($"enemy {a1.x} {a1.y}   count : {count}");
                gameTimer.Enabled = true;
            }*/
            var end_t = Stopwatch.GetTimestamp();
            //Console.WriteLine("kill2.2  " + (end_t - start_t).ToString());
            if ((count %2) ==1)
            {

                e.Destroy();
                /*for (int i = 0; i < e.max_score; i++)
                {
                    Snake.Grow();
                }*/
                return true;
            }
            return false;
        }
        Pt largest_cycle;
        bool checkCircleAndKillEnemy()
        {
            for (int i=6;i< Snake.Tail.Count();i++)
            {
                if (calcDistance(Snake.Tail[i], Snake.Tail[0]) == Snake.Size)
                {
                    largest_cycle.x = -1;
                    largest_cycle.y = i;
                }
            }
            if (largest_cycle.y != -1)
            {
                largest_cycle.x += 1;
                largest_cycle.y += 1;

                if (largest_cycle.y >= Snake.Length - 1)
                {
                    largest_cycle.x = -1;
                    largest_cycle.y = -1;
                }
                //Console.WriteLine($"{largest_cycle.x}    {largest_cycle.y}");
            }
            /*for(int i=0;i<cycle.Count();i++)
            {
                cycle.RemoveAt();
            }
            for(int i =0;i< Snake.Tail.Count/2;i++)
            {

                for(int j= i+2;j< Snake.Tail.Count;j++)
                {

                    if(calcDistance(Snake.Tail[i],Snake.Tail[j]) == Snake.Size)
                    {

                        start = i;
                        end = j;
                    }
                }
            }*/

            //Console.WriteLine("circle  "+(end_t - start_t).ToString());
            
            if (largest_cycle.x != -1 && largest_cycle.y != -1)
            {
                int start = largest_cycle.x;
                int end = largest_cycle.y;
                List<Vr> lines = new List<Vr>();
                bool last_hr = false;
                bool last_vl = false;
                Vr temp_v = new Vr();
                if (start < end && Snake.Tail[start].X == Snake.Tail[start + 1].X)
                {
                    
                    temp_v.start.x = Snake.Tail[start].X;
                    temp_v.start.y = Snake.Tail[start].Y;
                    last_hr = true;
                }
                else
                {
                    temp_v.start.x = Snake.Tail[start].X;
                    temp_v.start.y = Snake.Tail[start].Y;
                    last_vl = true;
                }
                
                for(int i=start+1;i<end-1;i++)
                {
                    if((last_hr && (Snake.Tail[i].X == Snake.Tail[i + 1].X)) || (last_vl && (Snake.Tail[i].Y == Snake.Tail[i + 1].Y)))
                    {
                        continue;
                    }else
                    {
                        temp_v.end.x = Snake.Tail[i].X;
                        temp_v.end.y = Snake.Tail[i].Y;
                        lines.Add(temp_v);

                        temp_v = new Vr();
                        temp_v.start.x = Snake.Tail[i].X;
                        temp_v.start.y = Snake.Tail[i].Y;

                        last_hr = !last_hr;
                        last_vl = !last_vl;
                    }
                }

                if ((last_hr && (Snake.Tail[end].X == Snake.Tail[start].X)) || (last_vl && (Snake.Tail[end].Y == Snake.Tail[start].Y)))
                {
                    temp_v.end.x = Snake.Tail[start].X;
                    temp_v.end.y = Snake.Tail[start].Y;
                }
                else
                {
                    temp_v.end.x = Snake.Tail[end].X;
                    temp_v.end.y = Snake.Tail[end].Y;

                    lines.Add(temp_v);
                    Vr temp_t = new Vr();
                    temp_t.start.x = Snake.Tail[end].X;
                    temp_t.start.y = Snake.Tail[end].Y;
                    temp_t.end.x = Snake.Tail[start].X;
                    temp_t.end.y = Snake.Tail[start].Y;

                    lines.Add(temp_t);
                }

                //Console.WriteLine("kill1  " + (end_t - start_t).ToString());
                
                //gameTimer.Enabled = false;
                //enemyTimer.Enabled = false;

                

                Enemys.RemoveAll(enemy => checkInBound(enemy, lines));
                //gameTimer.Enabled = true;
                //enemyTimer.Enabled = true;
                //Console.WriteLine("kill2  " + (end_t - start_t).ToString());

            }
            
            return false;
        }

        int calcDistance(Square s1, Square s2)
        {
            int dx = Math.Abs(s1.X - s2.X);
            int dy = Math.Abs(s1.Y - s2.Y);
            return dx + dy;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
        }
    }
}
