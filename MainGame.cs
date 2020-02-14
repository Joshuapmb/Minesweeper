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
        bool[,] bombArray = new bool[30, 16]; //Create 2D array of buttons that will be bombs (true or false)
        int[,] surroundingsArray = new int[30, 16]; //Array to hold value of surrounding bombs

        //true after the first left click
        bool gameStart;

        //number of bombs on the board
        int bombs;

        //remaining flags the player can place
        int flagsLeft;

        //panels keep track of number of boxes 
        int panelsX;
        int panelsY;

        //Initialise sound effects
        SoundPlayer lightning = new SoundPlayer(Properties.Resources.lightning); //for when hard mode starts
        SoundPlayer chicken = new SoundPlayer(Properties.Resources.chicken); // for when easy mode starts
        SoundPlayer cheers = new SoundPlayer(Properties.Resources.kidscheer); //for when normal mode starts
        SoundPlayer diggy = new SoundPlayer(Properties.Resources.dig); //for when you dig a box
        SoundPlayer unflagslap = new SoundPlayer(Properties.Resources.unflag); //for when you remove a flag
        SoundPlayer flagslap = new SoundPlayer(Properties.Resources.flag); // for when you place a flag
        SoundPlayer winner = new SoundPlayer(Properties.Resources.win); //for when you win
        SoundPlayer explody = new SoundPlayer(Properties.Resources.Explosion_3);//for when you lose


        //The constructor receives a difficulty and starts the game. It sets start game to false so that the game can begin after a left click
        public MainGame(int difficulty)
        {
            InitializeComponent();
            gameStart = false;
            loadGame(difficulty);
        }

        //Allows the player to select a difficulty. Depending on which they select, the size of arrays and number of bombs will change.
        void loadGame(int difficulty)
        {
            int height = 1000;
            int width = 1000;

            if (difficulty==0)
            {
                chicken.Play();
                bombs = 10;
                height = 339;
                width = 286;
                panelsX = 9;
                panelsY = 9;
            }
            if (difficulty==1)
            {
                cheers.Play();
                bombs = 40;
                height = 549;
                width = 496;
                panelsX = 16;
                panelsY = 16;
            }
            if (difficulty==2)
            {
                lightning.Play();
                bombs = 99;
                height = 549;
                width = 916;
                panelsX = 30;
                panelsY = 16;
            }

            //the player can only place as many flags as there are bombs
            flagsLeft = bombs;

            //will create a nice design of the boxes so that they are easier to see
            bool dark = true;
            //this will create the physical buttons that the player may select
            for (int x = 0; x < panelsX; x++) // Loop for x
            {
                for (int y = 0; y < panelsY; y++) // Loop for y
                {
                    //creates new button
                    btn[x, y] = new Button();
                    //names it according to it's coordinates
                    string btnName = x + "," + y;
                    btn[x, y].Name = btnName;
                    btn[x, y].Font = new Font("Consolas", 9F, FontStyle.Bold);
                    btn[x, y].SetBounds(30 * x, (30 * y) + 30, 30, 30);

                    //this creates the light and dark green stripes along the board
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

                    //Create event handler for mouse click
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_MouseDown);
                    Controls.Add(btn[x, y]);

                }

            }
            //Define size of window
            this.Size = new System.Drawing.Size(width, height); 
        }

        //This method generates the bombs
        int generateBombs(int x, int y, int bombs,int clickX, int clickY)
        {
            //a boolean to say whether or not all bombs placed are in valid buttons
            bool validBombs = false;

            //keeps track of initial amount of bombs
            int bombsStart = bombs;

            //Loop while there are bombs around the player's first click
            while (!validBombs)
            {
                //While there are still bombs to place
                while (bombs > 0)
                {
                    //two variables to describe random x and y in the board
                    int ranX = r.Next(x);
                    int ranY = r.Next(y);

                    //If there isn't a bomb already there
                    if (!bombArray[ranX, ranY])
                    {
                        //Make sure that the player's first click isn't a bomb
                        if (!(clickX == ranX && clickY == ranY))
                        {
                            //Place bomb and decrement the count
                            bombArray[ranX, ranY] = true;                           
                            bombs--;
                        }
                    }
                }

                //If where the player clicks has 0 surrounding bombs, all bomb placement is valid
                if (checkSurroundings(clickX, clickY) == 0)
                {
                    validBombs = true;                   
                }
                //Otherwise, reset bomb array and bomb count, so that the rest of the method repeats until a valid generation of bombs is present
                else
                {
                    bombs = bombsStart;
                    Array.Clear(bombArray, 0, bombArray.Length);
                }
            }          
            return bombs;
        }

        //This is the event handler for when the player clicks
        void btnEvent_MouseDown(object sender, MouseEventArgs e)
        {
            //object for click event
            Button ctrl = ((Button)sender);

            //Extract X and Y coordinates from name
            String[] xy = ctrl.Name.Split(',');
            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);

            // ON LEFT CLICK
            if (e.Button == MouseButtons.Left) 
            {                
                //If button is flagged dont allow click
                if (!(ctrl.BackColor == Color.Orange))
                {
                    Console.WriteLine(ctrl.Name + " was clicked");
                    diggy.Play();
                    //Check if clicked box has a bomb
                    bombCheck(x, y);
                    //If this is the first player's click
                    if (gameStart == false)
                    {
                        //Then generate bombs
                        generateBombs(panelsX, panelsY, bombs, x, y);
                        //Fill sourroundings array
                        for (int i = 0; i < panelsX; i++)
                        {
                            for (int j = 0; j < panelsY; j++)
                            {
                                surroundingsArray[i, j] = checkSurroundings(i, j);

                                /*
                                 * THIS SHOWS ALL BOMBS FOR DEBUGGING
                                if (bombArray[i,j])
                                {
                                    btn[i, j].BackColor = Color.Red;
                                }
                                */
                            }
                        }
                        //Set the game to have started
                        gameStart = true;

                        //Open an option for the player to concede
                        concedeToolStripMenuItem.Visible = true;
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;

                        //Dig around the clicked box by calling the Dig() method
                        digAround(x, y);
                    }
                    //Otherwise if this isn't the player's first click...
                    else
                    {
                        //Check the surrounding boxes
                        int surrounding = checkSurroundings(x, y);

                        //If there are surrounding bombs
                        if (surrounding > 0)
                        {
                            //Then change colour of box
                            btn[x, y].BackColor = Color.SaddleBrown;

                            //print the number of surrounding bombs
                            btn[x, y].Text = Convert.ToString(surrounding);

                            //don't allow the player to click the box again
                            btn[x, y].Enabled = false;
                        }

                        //Otherwise, if there aren't bombs surrounding where the player clicked...
                        else
                        {
                            //Then change colour of box
                            btn[x, y].BackColor = Color.SaddleBrown;

                            //print the number of surrounding bombs
                            btn[x, y].Enabled = false;

                            //keep digging until you find boxes with surrounding bombs
                            digAround(x, y);
                        }

                    }                    
                }               
            }

            //If the player right clicks
            if (e.Button == MouseButtons.Right && gameStart==true) //ON RIGHT CLICK AND GAME HAS STARTED
            {
                    //If the player has already right clicked (the colour of the button is orange)
                    if (ctrl.BackColor == Color.Orange)
                    {
                        //Unflag the box and return it to normal
                        Console.WriteLine(ctrl.Name + " was unflagged");
                        unflagslap.Play();
                        ctrl.BackColor = Color.Green;
                        ctrl.Text = "";
                        flagsLeft++;
                    }

                    //Otherwise, if the player hasn't already flagged the box...
                    else
                    {
                        //If the player has placed too many flags
                        if (flagsLeft == 0)
                        {
                            //display a message to the user
                            MessageBox.Show("Please remove a flag to place another", "Max Number of Flags in Play");
                        }
                        //Otherwise, if the player has flags to spare
                        else
                        {
                            //Flag the box
                            flagslap.Play();
                            Console.WriteLine(ctrl.Name + " was flagged");
                            ctrl.BackColor = Color.Orange;
                            ctrl.Text = "F";

                            //Decrement number of flags
                            flagsLeft--;

                            //Create an int that tracks how many flags there should be
                            int correctFlags = bombs;

                            //Count how many correct flags have been placed, going down
                            for (int i = 0; i < panelsX; i++)
                            {
                                for (int j = 0; j < panelsY; j++)
                                {
                                    if (bombArray[i,j] && btn[i,j].BackColor==Color.Orange)
                                    {
                                        //Decrement the count 
                                        correctFlags--;
                                    }                                   
                                }
                            }

                            //If all flags have been placed correctly
                            if (correctFlags == 0)
                            {
                                //The player wins, do winning messages and sounds
                                winner.Play();
                                MessageBox.Show("YOU WON!!!!", "WINNER!");

                                //Restart the game
                                gameRestart(1);
                            }
                    }

                    }
                
                
            }
            
        }

        //A method that calls the other dig methods
        void digAround(int x, int y)
        {
            digUp(x,y);
            digDown(x,y);
            digLeft(x,y);
            digRight(x,y);
        }

        //This method digs upwards of the box selected
        void digUp(int x, int y)
        {
            //While there is a box above the one clicked
            while (y > 0)
            {
                //Look at the box above
                y--;

                //If there are bombs surrounding the box above
                if (surroundingsArray[x,y]>0)
                {
                    //Display how many bombs surround the box above the one that has been clicked
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]);
                    btn[x, y].BackColor = Color.SaddleBrown;
                    btn[x, y].Enabled = false;

                    //CHECK LEFT- if there is a box to the left
                    if (x>0)
                    {
                        //check how many bombs are in the box to the left of the one that has been clicked
                        int surrounding = checkSurroundings(x - 1, y);

                        //If there are bombs surrounding the box to the left of the one that has been clicked
                        if (surrounding > 0 && !(bombArray[x - 1, y]))
                        {
                            //display how many bombs surround the box to the left of the one that has been clicked
                            btn[x - 1, y].BackColor = Color.SaddleBrown;
                            btn[x - 1, y].Text = Convert.ToString(surrounding);
                            btn[x - 1, y].Enabled = false;
                        }

                        //Otherwise, (if there are no bombs surrounding the left one) keep digging
                        else
                        {
                            digLeft(x, y);
                        }
                    }

                    //CHECK RIGHT - if there is a box to the right
                    if (x < (panelsX-1))
                    {
                        //check how many bombs are in the box to the right of the one that has been clicked
                        int surrounding = checkSurroundings(x + 1, y);

                        //If there are bombs surrounding the box to the right of the one that has been clicked
                        if (surrounding > 0 && !(bombArray[x + 1, y]))
                        {
                            //display how many bombs surround the box to the right of the one that has been clicked
                            btn[x + 1, y].BackColor = Color.SaddleBrown;
                            btn[x + 1, y].Text = Convert.ToString(surrounding);
                            btn[x + 1, y].Enabled = false;
                        }
                        //Otherwise, (if there are no bombs surrounding the right one) keep digging
                        else
                        {
                            digRight(x, y);
                        }
                    }
                    return;
                }

                //Otherwise, if there are no bombs surrounding the one above
                else
                {
                    //If the colour is not brown (box isn't dig dugged)
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))
                    {
                        //Change the colour to brown (dig it)
                        btn[x, y].BackColor = Color.SaddleBrown;
                        btn[x, y].Enabled = false;
                        digLeft(x, y);
                        digRight(x, y);                    
                    }
                }
            }
        }

        //THIS METHOD IS SIMILAR TO DIGUP(), PLEASE REFER TO ITS COMMENTS
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
                        else
                        {
                            digLeft(x, y);
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
                        else
                        {
                            digRight(x, y);
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

        //THIS METHOD IS SIMILAR TO DIGUP(), PLEASE REFER TO ITS COMMENTS
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

        //THIS METHOD IS SIMILAR TO DIGUP(), PLEASE REFER TO ITS COMMENTS
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

        //This method checks the number of bombs around the x,y coordinates
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
            Help helpMenu = new Help();
            helpMenu.ShowDialog();
        }

        private void ResartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameRestart(1);
        }
        private void gameRestart(int difficulty)
        {
            this.Hide();
            var mainGame = new MainGame(difficulty);
            mainGame.Closed += (s, args) => this.Close();
            mainGame.Show();
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
                explody.Play();
                MessageBox.Show("GAME OVER!", "Game Over");
                gameRestart(1);
            }
        }

        private void EasyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainGame = new MainGame(0);
            mainGame.Closed += (s, args) => this.Close();
            mainGame.Show();
        }

        private void MediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainGame = new MainGame(1);
            mainGame.Closed += (s, args) => this.Close();
            mainGame.Show();
        }

        private void HardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainGame = new MainGame(2);
            mainGame.Closed += (s, args) => this.Close();
            mainGame.Show();
        }

        //Method to show 'concede' option
        private void ConcedeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If the text on the option says "restart" and is clicked, restart the game
            if (concedeToolStripMenuItem.Text == "Restart")
            {
                gameRestart(1);
            }

            //Otherwise
            else
            {
                //Show message
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to give up?", "Concede?", MessageBoxButtons.YesNo);

                //If user chooses "yes"
                if (dialogResult == DialogResult.Yes)
                {
                    //Change text to "restart"
                    concedeToolStripMenuItem.Text = "Restart";

                    //Disable all boxes and show all bombs within the board
                    for (int i = 0; i < panelsX; i++)
                    {
                        for (int j = 0; j < panelsY; j++)
                        {
                            btn[i, j].Enabled = false;
                            if (bombArray[i, j])
                            {
                                btn[i, j].BackColor = Color.Red;
                            }
                        }
                    }
                }
            }          
        }
    }
}
            
