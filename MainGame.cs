using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MainGame : Form
    {
        Random r = new Random();
        Button[,] btn = new Button[16, 16]; // Create 2D array of buttons
        bool[,] bombArray = new bool[16, 16];
        List<PanelStates> Panel;
        Label lblTurnTime = new Label();
        int seconds = 10;

        private void MoveTime_Tick(object sender, EventArgs e)
        {
            //seconds--;
            //lblTurnTime.Text seconds.ToString();

        }

        public MainGame()
        {        

            InitializeComponent();
            Panel = new List<PanelStates>();
            lblTurnTime.Location = new System.Drawing.Point(400, 7);
            lblTurnTime.Text = "10";
            int id = 0;
            for (int x = 0; x < btn.GetLength(0); x++) // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++) // Loop for y
                {
                    Panel.Add(new PanelStates(id,x,y));
                    id++;
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(30 * x, (30 * y) + 30, 30, 30);
                    btn[x, y].BackColor = Color.White;
                    btn[x, y].FlatStyle = FlatStyle.Flat;
                    //btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_MouseDown);
                    Controls.Add(btn[x, y]);

                }
              
            }
            int bombs = 40;
            while (bombs > 0)
            {
                int ranX = r.Next(16);
                int ranY = r.Next(16);

                if (!Panel[((ranY * 16) + ranX)].IsMine == true)
                {
                    //bombArray[ranX, ranY] = true;
                    btn[ranX, ranY].BackColor = Color.Red;
                    int panelList = (ranY * 16) + ranX;
                    Panel[panelList].IsMine = true;
                    bombs--;
                }
            }
            this.Size = new System.Drawing.Size(496, 550);
        }

        void btnEvent_MouseDown(object sender, MouseEventArgs e)
        {
            Control ctrl = ((Control)sender);
            if (e.Button == MouseButtons.Left)
            {
                ctrl.BackColor = Color.Black;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (ctrl.BackColor == Color.Orange)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.Text = "";
                }
                else
                {
                    ctrl.BackColor = Color.Orange;
                    ctrl.Text = "F";
                }
                
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
        }

        private void MainGame_Load(object sender, EventArgs e) //REQUIRED
        {
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HighscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void ResartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainGame mainGame = new MainGame();
            mainGame.ShowDialog();
            this.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        
    }
    public class PanelStates
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }

        public PanelStates(int id, int x, int y)
        {
            IsMine = false;
            this.ID = id;
            this.X = x;
            this.Y = y;

        }
    }
}
            
