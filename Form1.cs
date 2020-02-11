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
        bool[,] bombArray = new bool[16, 16]; // Create 2D boolean bomb array
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
            lblTurnTime.Location = new System.Drawing.Point(400, 7);
            lblTurnTime.Text = "10";

            for (int x = 0; x < btn.GetLength(0); x++) // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++) // Loop for y
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(30 * x, (30 * y) + 30, 30, 30);
                    btn[x, y].BackColor = Color.Green;
                    btn[x, y].FlatStyle = FlatStyle.Flat;
                    //btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_MouseDown);
                    Controls.Add(btn[x, y]);

                }
              
            }
            int bombs = 40;
            while (bombs>0)
            {
                int ranX = r.Next(16);
                int ranY = r.Next(16);

                if (!bombArray[ranX, ranY])
                {
                    btn[ranX, ranY].BackColor = Color.Red;
                    bombArray[ranX, ranY] = true;
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
                ctrl.BackColor = Color.White;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (ctrl.BackColor == Color.Yellow)
                {
                    ctrl.BackColor = Color.Green;
                    ctrl.Text = "";
                }
                else
                {
                    ctrl.BackColor = Color.Yellow;
                    ctrl.Text = "F";
                }
                
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
        }

        /*
        int checkSurroundings()
        {

             int surroundingBombCount = 0;

            //if it's a bomb
            if (btn[x, y] = bombBtn)
            {
                //explode
                MessageBox.Show("You exploded. Game over.");
                ResartToolStripMenuItem_Click();
            }

            //else
            else
            {

                   
                    if (x - 1 => 0 && y+1 <= 15)
                    {
                        //if there's a bomb bottom left
                        if (btn[x - 1, y + 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (y + 1 <= 15)
                    {
                        //if there's a bomb below
                        if (btn[x, y + 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x+1 >= 0 && y + 1 <= 15)
                    {
                        //if there's a bomb bottom right
                        if (btn[x + 1, y + 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x-1 >= 0)
                    {
                        //if there's a bomb left
                        if (btn[x - 1, y] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x+1 >= 0)
                    {
                        //if there's a bomb right
                        if (btn[x + 1, y] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x-1 >= 0 && y - 1 <= 15)
                    {
                        //if there's a bomb top left
                        if (btn[x - 1, y - 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (y - 1 <= 15)
                    {
                        //if there's a bomb above
                        if (btn[x, y - 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x+1 >= 0 && y - 1 <= 15)
                    {
                        //if there's a bomb top right
                        if (btn[x + 1, y - 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    btn[x, y].BackColor = Color.White;
                    btn[x, y].Text = surroundingBombCount;

            }

            return surroundingBombCount;

        }
        */


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
             MessageBox.Show("You are presented with a board of squares. Some squares contain mines (bombs), others don't. If you click on a square containing a bomb, you lose. If you manage to click all the squares (without clicking on any bombs) you win. Clicking a square which doesn't have a bomb reveals the number of neighbouring squares containing bombs. Use this information plus some guess work to avoid the bombs. To open a square, point at the square and click on it. To mark a square you think is a bomb, point and right-click (or hover with the mouse and press Space).");
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
}
            
