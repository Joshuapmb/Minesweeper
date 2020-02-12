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
        //List<PanelStates> Panel;
        //Label lblTurnTime = new Label();
        //int seconds = 10;
        bool gameStart;
        int bombs;

        private void MoveTime_Tick(object sender, EventArgs e)
        {
            //seconds--;
            //lblTurnTime.Text seconds.ToString();

        }

        public MainGame()
        {        

            InitializeComponent();
            gameStart = false;
            loadGame(1);
        }

        void loadGame(int difficulty)
        {
            int height = 1000;
            int width = 1000;

            if (difficulty==0)
            {
                bombs = 10;
            }
            if (difficulty==1)
            {
                bombs = 40;
                height = 550;
                width = 496;
            }
            if (difficulty==2)
            {
                bombs = 99;
            }

            //Panel = new List<PanelStates>();
            //lblTurnTime.Location = new System.Drawing.Point(400, 7);
            //lblTurnTime.Text = "10";
            int id = 0;
            bool dark = true;
            for (int x = 0; x < btn.GetLength(0); x++) // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++) // Loop for y
                {
                    //Panel.Add(new PanelStates(id,x,y));
                    id++;
                    btn[x, y] = new Button();
                    string btnName = x + "," + y;
                    btn[x, y].Name = btnName;
                    btn[x, y].SetBounds(30 * x, (30 * y) + 30, 30, 30);
                    if (dark == false)
                    {
                        btn[x, y].BackColor = Color.Green;
                        dark = true;
                    }
                    else{
                        btn[x, y].BackColor = Color.DarkGreen;
                        dark = false;
                    }
                    
                    btn[x, y].FlatStyle = FlatStyle.Flat;
                    //btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_MouseDown);
                    Controls.Add(btn[x, y]);

                }

            }                  
            
            this.Size = new System.Drawing.Size(width, height); //Define size of window
        }
        int generateBombs(int x, int y, int bombs,int clickX, int clickY)
        {
            while (bombs > 0)
            {
                int ranX = r.Next(x);
                int ranY = r.Next(y);

                if (!bombArray[ranX, ranY])
                {
                    if (!(ranX==clickX) && !(ranY == clickX))
                    {
                        bombArray[ranX, ranY] = true;
                        btn[ranX, ranY].BackColor = Color.Red;
                        //int panelList = (ranY * 16) + ranX;
                        //Panel[panelList].IsMine = true;
                        bombs--;
                    }                  
                }
            }
            return bombs;
        }

        void btnEvent_MouseDown(object sender, MouseEventArgs e)
        {
            Button ctrl = ((Button)sender);
            if (e.Button == MouseButtons.Left)
            {
                String[] xy = ctrl.Name.Split(',');
                int x = int.Parse(xy[0]);
                int y = int.Parse(xy[1]);

                Console.WriteLine(ctrl.Name + " was clicked");
                if (gameStart==false)
                {
                    generateBombs(16,16,bombs,x,y);
                    gameStart = true;
                    btn[x,y].BackColor = Color.SaddleBrown;
                    digAround(x,y);
                }
                else
                {

                }
                
                bombCheck(x,y);              
            }
            if (e.Button == MouseButtons.Right)
            {
                if (ctrl.BackColor == Color.Orange)
                {
                    Console.WriteLine(ctrl.Name + " was unflagged");
                    ctrl.BackColor = Color.White;
                    ctrl.Text = "";
                }
                else
                {
                    Console.WriteLine(ctrl.Name + " was flagged");
                    ctrl.BackColor = Color.Orange;
                    ctrl.Text = "F";
                }
                
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
        }

        void digAround(int x, int y)
        {
            int originX = x;
            int originY = y;
            digUp(originX, originY);
            digRight(originX, originY);
            digDown(originX, originY);
            digLeft(originX, originY);            
        }
        void digUp(int x, int y)
        {
            
        }

        void digRight(int x, int y)
        {
            while (x < 15)
            {
                x++;
                if (bombArray[x, y])
                {
                    btn[x, y].BackColor = Color.Orange;
                    return;
                }
                else
                {
                    digUp(x, y);
                    digDown(x, y);
                    btn[x, y].BackColor = Color.SaddleBrown;
                }
            }
        }
        void digDown(int x, int y)
        {
            while (y < 15)
            {
                y++;
                if (bombArray[x, y])
                {
                    btn[x, y].BackColor = Color.Orange;
                    return;
                }
                else
                {
                    digUp(x, y);
                    digDown(x, y);
                    btn[x, y].BackColor = Color.SaddleBrown;
                }
            }
        }

        void digLeft(int x, int y)
        {
            while (x > 0)
            {
                x--;              
                if (bombArray[x, y])
                {
                    btn[x, y].BackColor = Color.Orange;
                    return;
                }
                else
                {
                    digUp(x,y);
                    digDown(x,y);
                    btn[x, y].BackColor = Color.SaddleBrown;
                }
            }
        }

        private void MainGame_Load(object sender, EventArgs e) //REQUIRED
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
            gameRestart();
        }
        private void gameRestart()
        {
            this.Hide();
            MainGame mainGame = new MainGame();
            mainGame.ShowDialog();
            this.Close();
        }
        private void bombCheck(int x, int y)
        {
            if (bombArray[x,y])
            {
                MessageBox.Show("GAME OVER!", "Game Over");
                gameRestart();
            }
        }

        /*
        private void Timer1_Tick(object sender, EventArgs e)
        {

        }
        */
    
    }
    /*
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
    */
}
            
