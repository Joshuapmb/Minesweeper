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
        int[,] surroundingsArray = new int[16, 16];
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
            bool validBombs = false;
            int bombsStart = bombs;
            while (!validBombs)
            {
                while (bombs > 0)
                {
                    int ranX = r.Next(x);
                    int ranY = r.Next(y);

                    if (!bombArray[ranX, ranY])
                    {
                        if (!(clickX == ranX && clickY == ranY))
                        {
                            bombArray[ranX, ranY] = true;                           
                            //int panelList = (ranY * 16) + ranX;
                            //Panel[panelList].IsMine = true;
                            bombs--;
                        }
                    }
                }
                if (checkSurroundings(clickX, clickY) == 0)
                {
                    validBombs = true;                   
                }
                else
                {
                    bombs = bombsStart;
                    Array.Clear(bombArray, 0, bombArray.Length);
                }
            }          
            return bombs;
        }

        void btnEvent_MouseDown(object sender, MouseEventArgs e)
        {
            Button ctrl = ((Button)sender);

            //Extract X and Y coordinates from name
            String[] xy = ctrl.Name.Split(',');
            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);

            if (e.Button == MouseButtons.Left)
            {
                //If button is flagged dont allow click
                if (ctrl.BackColor == Color.Orange)
                {
                    MessageBox.Show("Please unflag before clicking", "Flagged");
                }
                else
                {                   

                    Console.WriteLine(ctrl.Name + " was clicked");
                    if (gameStart == false)
                    {
                        generateBombs(16, 16, bombs, x, y);
                        for (int i = 0; i < 16; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                surroundingsArray[i,j] = checkSurroundings(i, j);
                                //btn[i, j].Text = Convert.ToString(surroundingsArray[i, j]);
                                if (bombArray[i,j])
                                {
                                    btn[i, j].BackColor = Color.Red;
                                }
                            }
                        }
                        gameStart = true;
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digAround(x, y);
                        //boardCheck(16,16);
                    }
                    else
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digAround(x, y);
                    }
                }
                bombCheck(x,y);              
            }
            if (e.Button == MouseButtons.Right && gameStart==true)
            {
                if (ctrl.BackColor == Color.Orange)
                {
                    Console.WriteLine(ctrl.Name + " was unflagged");
                    ctrl.BackColor = Color.Green;
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
        void boardCheck(int boardX, int boardY)
        {
            for (int i = 1; i<(boardX-1);i++)
            {
                for (int j = 1; j < (boardY-1); j++)
                {
                    int connections = 0;
                    if (!(btn[i,j].Text == ""))
                    {
                        if (!(btn[i - 1, j].Text == ""))
                        {
                            connections++;
                        }
                        if (!(btn[i, j - 1].Text == ""))
                        {
                            connections++;
                        }
                        if (!(btn[i + 1, j].Text == ""))
                        {
                            connections++;
                        }
                        if (!(btn[i, j + 1].Text == ""))
                        {
                            connections++;
                        }

                    }

                    if (connections<2)
                    {
                        if (!(btn[i + 1, j].Text == ""))
                        {
                            int Surrounding = checkSurroundings(i + 1, j);
                            if (Surrounding > 0)
                            {
                                btn[i + 1, j].Text = Convert.ToString(Surrounding);
                            }
                        }
                        if (!(btn[i - 1, j].Text == ""))
                        {
                            int Surrounding = checkSurroundings(i - 1, j);
                            if (Surrounding > 0)
                            {
                                btn[i - 1, j].Text = Convert.ToString(Surrounding);
                            }
                        }
                        if (!(btn[i, j + 1].Text == ""))
                        {
                            int Surrounding = checkSurroundings(i, j + 1);
                            if (Surrounding > 0)
                            {
                                btn[i, j + 1].Text = Convert.ToString(Surrounding);
                            }
                        }
                        if (!(btn[i, j - 1].Text == ""))
                        {
                            int Surrounding = checkSurroundings(i, j - 1);
                            if (Surrounding > 0)
                            {
                                btn[i, j - 1].Text = Convert.ToString(Surrounding);
                            }
                        }
                    }
                }
            }
        }

        void digAround(int x, int y)
        {
            digUp(x,y);
            digDown(x,y);
            digLeft(x,y);
            digRight(x,y);
        }
        void digUp(int x, int y)
        {
            while (y > 0)
            {
                y--;
                if (surroundingsArray[x,y]>0)
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;
                    return;
                }
                else
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digLeft(x, y);
                        digRight(x, y);
                    }
                }
            }
        }
        void digDown(int x, int y)
        {
            while (y < 15)
            {
                y++;
                if (surroundingsArray[x, y] > 0)
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;
                    return;
                }
                else
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digLeft(x, y);
                        digRight(x, y);
                    }
                }
            }
        }

        void digRight(int x, int y)
        {
            while (x < 15)
            {
                x++;
                if (surroundingsArray[x, y] > 0)
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;
                    return;
                }
                else
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digUp(x, y);
                        digDown(x, y);
                    }
                }
            }
        }

        void digLeft(int x, int y)
        {
            while (x > 0)
            {
                x--;
                if (surroundingsArray[x, y] > 0)
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;
                    return;
                }
                else
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digUp(x, y);
                        digDown(x, y);
                    }
                }
            }
        }


        int checkSurroundings(int x, int y)
        {
            int surroundingBombCount = 0;

            if ((x > 0) && (y < 15))
            {
                //if there's a bomb bottom left
                if (bombArray[x-1,y+1])
                {
                    surroundingBombCount++;
                }
            }
            if (y < 15)
            {
                //if there's a bomb below
                if (bombArray[x, y + 1])
                {
                    surroundingBombCount++;
                }
            }
            if (x < 15 && y < 15)
            {
                //if there's a bomb bottom right
                if (bombArray[x + 1,y + 1])
                {
                    surroundingBombCount++;
                }
            }
            if (x > 0)
            {
                //if there's a bomb left
                if (bombArray[x - 1, y])
                {
                    surroundingBombCount++;
                }
            }
            if (x < 15)
            {
                //if there's a bomb right
                if (bombArray[x + 1, y])
                {
                    surroundingBombCount++;
                }
            }
            if (x > 0 && y > 0)
            {
                //if there's a bomb top left
                if (bombArray[x - 1, y - 1])
                {
                    surroundingBombCount++;
                }
            }
            if (y > 0)
            {
                //if there's a bomb above
                if (bombArray[x, y - 1])
                {
                    surroundingBombCount++;
                }
            }
            if (x < 15 && y > 0)
            {
                //if there's a bomb top right
                if (bombArray[x + 1, y - 1])
                {
                    surroundingBombCount++;
                }
            }
            //btn[x, y].BackColor = Color.White;
            //btn[x, y].Text = surroundingBombCount;

            return surroundingBombCount;
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
}
            
