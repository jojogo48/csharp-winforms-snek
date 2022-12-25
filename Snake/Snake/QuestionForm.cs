using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class QuestionForm : Form
    {
        public int enemy_score { get; set; }
        public int score { get; set; }
        public string ans { get; set; }
        private Question question { get; set; }
        public QuestionForm(Question question,int enmey_score)
        {
            this.enemy_score = enmey_score;
            this.question = question;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            score = 0;

            textBox1.Text = this.question.Question1;
            radioButton1.Text = this.question.Option1;
            radioButton2.Text = this.question.Option2;
            radioButton3.Text = this.question.Option3;
            radioButton4.Text = this.question.Option4;
            ans = this.question.Answer.ToString();
            Console.WriteLine(ans);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (radioButton1.Checked && ans.CompareTo("1")==0)
            {
                score = enemy_score;
            }
            if (radioButton2.Checked && ans.CompareTo("2") == 0)
            {
                score = enemy_score;
            }
            if (radioButton3.Checked && ans.CompareTo("3") == 0)
            {
                score = enemy_score;
            }
            if (radioButton4.Checked && ans.CompareTo("4") == 0)
            {
                score = enemy_score;
            }
            if(score == enemy_score)
            {
                DialogResult Result = MessageBox.Show($"恭喜答對\n獲得分數{score}分", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult Result = MessageBox.Show($"答錯瞜\n獲得分數{score}分", "結果", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
