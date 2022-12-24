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
    public partial class RankingForm : Form
    {
        public SnakeEntities db { get; set; }
        public RankingForm(SnakeEntities db)
        {
            this.db = db;
            InitializeComponent();
        }

        private void RankingForm_Load(object sender, EventArgs e)
        {


            var result = from p in db.Histroy
                         orderby p.Score descending
                         select new
                         {
                             p.Score,
                             p.Time
                         };
            dataGridView1.DataSource = result.ToList();
            for(int i=0;i < dataGridView1.ColumnCount;i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
