using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Minesweeper
{
    public partial class MainGame : Form
    {
        Random r = new Random();
        Button[,] btn = new Button[30, 16]; // Create 2D array of buttons
        bool[,] bombArray = new bool[30, 16];
        int[,] surroundingsArray = new int[30, 16];
        //Label lblTurnTime = new Label();
        //int seconds = 10;
        bool gameStart;
        int bombs;
        int flagsLeft;
        int difficulty;
        int panelsX;
        int panelsY;

        private void MoveTime_Tick(object sender, EventArgs e)
        {
            //seconds--;
            //lblTurnTime.Text seconds.ToString();

        }

        public MainGame(int difficulty)
        {        

            InitializeComponent();
            gameStart = false;
            loadGame(difficulty);
        }

        void loadGame(int difficulty)
        {
            int height = 1000;
            int width = 1000;

            if (difficulty==0)
            {
                SoundPlayer chicken = new SoundPlayer(Properties.Resources.chicken);
                chicken.Play();
                bombs = 10;
                height = 339;
                width = 286;
                panelsX = 9;
                panelsY = 9;
            }
            if (difficulty==1)
            {
                SoundPlayer cheers = new SoundPlayer(Properties.Resources.kidscheer);
                cheers.Play();
                bombs = 40;
                height = 550;
                width = 496;
                panelsX = 16;
                panelsY = 16;
            }
            if (difficulty==2)
            {
                SoundPlayer lightning = new SoundPlayer(Properties.Resources.lightning);
                lightning.Play();
                bombs = 99;
                height = 550;
                width = 1000;
                panelsX = 30;
                panelsY = 16;
            }

            flagsLeft = bombs;

            //lblTurnTime.Location = new System.Drawing.Point(400, 7);
            //lblTurnTime.Text = "10";
            int id = 0;
            bool dark = true;
            for (int x = 0; x < panelsX; x++) // Loop for x
            {
                for (int y = 0; y < panelsY; y++) // Loop for y
                {
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

            if (e.Button == MouseButtons.Left) // ON LEFT CLICK
            {
                bombCheck(x, y);
                //If button is flagged dont allow click
                if (!(ctrl.BackColor == Color.Orange))
                {
                    Console.WriteLine(ctrl.Name + " was clicked");
                    SoundPlayer diggy = new SoundPlayer(Properties.Resources.dig);
                    diggy.Play();
                    if (gameStart == false)
                    {
                        generateBombs(panelsX, panelsY, bombs, x, y);
                        for (int i = 0; i < panelsX; i++)
                        {
                            for (int j = 0; j < panelsY; j++)
                            {
                                surroundingsArray[i, j] = checkSurroundings(i, j);
                                /*
                                if (bombArray[i,j])
                                {
                                    btn[i, j].BackColor = Color.Red;
                                }
                                */
                            }
                        }
                        gameStart = true;
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digAround(x, y);
                    }
                    else
                    {
                        int surrounding = checkSurroundings(x, y);
                        if (surrounding > 0)
                        {
                            btn[x, y].BackColor = Color.SaddleBrown;
                            btn[x, y].Text = Convert.ToString(surrounding);
                            btn[x, y].Enabled = false;
                        }
                        else
                        {
                            btn[x, y].BackColor = Color.SaddleBrown;
                            btn[x, y].Enabled = false;
                            digAround(x, y);
                        }

                    }                    
                }            
            }
            if (e.Button == MouseButtons.Right && gameStart==true) //ON RIGHT CLICK AND GAME HAS STARTED
            {

                    if (ctrl.BackColor == Color.Orange)
                    {
                        Console.WriteLine(ctrl.Name + " was unflagged");
                        SoundPlayer unflagslap = new SoundPlayer(Properties.Resources.unflag);
                        unflagslap.Play();
                    ctrl.BackColor = Color.Green;
                        ctrl.Text = "";
                        flagsLeft++;
                    }
                    else
                    {
                        if (flagsLeft == 0)
                        {
                            MessageBox.Show("Please remove a flag to place another", "Max Number of Flags in Play");
                        }
                        else
                        {
                            Console.WriteLine(ctrl.Name + " was flagged");
                            SoundPlayer flagslap = new SoundPlayer(Properties.Resources.flag);
                            flagslap.Play();
                        ctrl.BackColor = Color.Orange;
                            ctrl.Text = "F";
                            flagsLeft--;
                            int correctFlags = bombs;

                            for (int i = 0; i < panelsX; i++)
                            {
                                for (int j = 0; j < panelsY; j++)
                                {
                                    if (bombArray[i,j] && btn[i,j].BackColor==Color.Orange)
                                    {
                                        correctFlags--;
                                    }
                                    if (correctFlags==0)
                                    {
                                        MessageBox.Show("YOU WON!!!!", "WINNER!");
                                        SoundPlayer winner = new SoundPlayer(Properties.Resources.win);
                                        winner.Play();

                                    gameRestart();
                                    }
                                }
                            }
                    }

                    }
                
                
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
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

                    if (x>0)
                    {
                        int surrounding = checkSurroundings(x - 1, y);
                        if (surrounding > 0 && !(bombArray[x - 1, y]))
                        {
                            btn[x - 1, y].BackColor = Color.SaddleBrown;
                            btn[x - 1, y].Text = Convert.ToString(surrounding);
                            btn[x - 1, y].Enabled = false;
                        }
                    }
                    if (x < (panelsX-1))
                    {
                        int surrounding = checkSurroundings(x + 1, y);
                        if (surrounding > 0 && !(bombArray[x + 1, y]))
                        {
                            btn[x + 1, y].BackColor = Color.SaddleBrown;
                            btn[x + 1, y].Text = Convert.ToString(surrounding);
                            btn[x + 1, y].Enabled = false;
                        }
                    }
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
            while (y < (panelsY-1))
            {
                y++;
                if (surroundingsArray[x, y] > 0)
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;
                    if (x > 0)
                    {
                        int surrounding = checkSurroundings(x - 1, y);
                        if (surrounding > 0 && !(bombArray[x - 1, y]))
                        {
                            btn[x - 1, y].BackColor = Color.SaddleBrown;
                            btn[x - 1, y].Text = Convert.ToString(surrounding);
                            btn[x - 1, y].Enabled = false;
                        }
                    }
                    if (x < (panelsX-1))
                    {
                        int surrounding = checkSurroundings(x + 1, y);
                        if (surrounding > 0 && !(bombArray[x + 1, y]))
                        {
                            btn[x + 1, y].BackColor = Color.SaddleBrown;
                            btn[x + 1, y].Text = Convert.ToString(surrounding);
                            btn[x + 1, y].Enabled = false;
                        }
                    }
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
            while (x < (panelsX-1))
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

            if ((x > 0) && (y < (panelsY-1)))
            {
                //if there's a bomb bottom left
                if (bombArray[x-1,y+1])
                {
                    surroundingBombCount++;
                }
            }
            if (y < (panelsY - 1))
            {
                //if there's a bomb below
                if (bombArray[x, y + 1])
                {
                    surroundingBombCount++;
                }
            }
            if (x < (panelsX - 1) && y < (panelsY - 1))
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
            if (x < (panelsX - 1))
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
            if (x < (panelsX - 1) && y > 0)
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
            MainGame mainGame = new MainGame(difficulty);
            mainGame.ShowDialog();
            this.Close();
        }
        private void bombCheck(int x, int y)
        {
            if (bombArray[x,y])
            {
                for (int i = 0 ; i < panelsX; i++)
                {
                    for (int j = 0; j < panelsY; j++)
                    {
                        if (bombArray[i, j])
                        {
                            btn[i, j].BackColor = Color.Red;
                        }
                    }
                }
                MessageBox.Show("GAME OVER!", "Game Over");
                SoundPlayer explody = new SoundPlayer(Properties.Resources.Explosion_3);
                explody.Play();
                gameRestart();
            }
        }

        private void EasyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainGame mainGame = new MainGame(0);
            mainGame.ShowDialog();
            this.Close();
        }

        private void MediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainGame mainGame = new MainGame(1);
            mainGame.ShowDialog();
            this.Close();
        }

        private void HardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainGame mainGame = new MainGame(2);
            mainGame.ShowDialog();
            this.Close();
        }
    }
}
            
