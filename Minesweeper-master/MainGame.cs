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
    public partial class MainGame : Form    // Main class used to contain the code
    {
        Random r = new Random(); // Create an instance of the 'Random' class as 'r'
        Button[,] btn = new Button[30, 16]; // Create 2D array of buttons
        bool[,] bombArray = new bool[30, 16]; // Create 2D array of boolean values for each button
        int[,] surroundingsArray = new int[30, 16]; // Create 2D array of integers for each button
        //Label lblTurnTime = new Label();
        //int seconds = 10;
        bool gameStart; // Boolean variable to determine whether the game has started or not
        int bombs;  // Integer variable that holds how many bombs are present in the game
        int flagsLeft;  // Integer variable that determines how many flags the user has left to use
        int difficulty; // Integer variable to determine the difficulty
        int panelsX;
        int panelsY;

        private void MoveTime_Tick(object sender, EventArgs e)
        {
            //seconds--;
            //lblTurnTime.Text seconds.ToString();

        }

        public MainGame(int difficulty) // The 'Main' function to initialise and start the game
        {        

            InitializeComponent();  // Method to add/create object on the form
            gameStart = false;  // Sets the start game boolean as false by default
            loadGame(difficulty);   // Calls the 'loadGame' function and parses the difficulty variable to start game
        }

        void loadGame(int difficulty)   // 'loadGame' function initialises the game depending on which difficulty was chosen by the user
        {
            int height = 1000;  // Integer variable used to set the default height of the form window
            int width = 1000;   // Integer variable used to set the default width of the form window

            if (difficulty==0)  // Checks if difficulty is set to Easy
            {
                bombs = 10;     // Sets the number of bombs in the game to 10
                height = 339;   // Determines the height of the form
                width = 469;    // Determines the width of the form
                panelsX = 9;    // Determines there are 9 buttons along the x-axis
                panelsY = 9;    // Determines there are 9 buttons along the y-axis
            }
            if (difficulty==1)  // Checks if difficulty is set to Medium
            {
                bombs = 40;     // Sets the number of bombs in the game to 40
                height = 550;   // Determines the height of the form
                width = 496;    // Determines the width of the form
                panelsX = 16;   // Determines there are 16 buttons along the x-axis
                panelsY = 16;   // Determines there are 16 buttons along the y-axis
            }
            if (difficulty==2)  // Checks if difficulty is set to Hard
            {
                bombs = 99;     // Sets the number of bombs in the game to 99
                height = 550;
                width = 1000;
                panelsX = 30;   // Determines there are 16 buttons along the x-axis
                panelsY = 16;   // Determines there are 30 buttons along the y-axis
            }

            flagsLeft = bombs;  // Gives the player flags for the amount of bombs on the grid

            //lblTurnTime.Location = new System.Drawing.Point(400, 7);
            //lblTurnTime.Text = "10";
            int id = 0; // Integer variable 
            bool dark = true;   // Boolean variable to determine the colour of the button (dark/light green) to alternate colours with each row
            for (int x = 0; x < panelsX; x++) // Loop for x number of buttons
            {
                for (int y = 0; y < panelsY; y++) // Loop for y number of buttons
                {
                    id++;
                    btn[x, y] = new Button();   // Creates a new button given the x and y coordinates
                    string btnName = x + "," + y;   // Assigns the button coordinates to a string
                    btn[x, y].Name = btnName;   // Assigns the btnName string to the Name of the button
                    btn[x, y].SetBounds(30 * x, (30 * y) + 30, 30, 30); // Sets the size and position of the button, relative to its coordinates
                    if (dark == false)  // Checks if the button should not be dark green
                    {
                        btn[x, y].BackColor = Color.Green;  // Sets the button colour to green
                        dark = true;    // Sets the dark value back to true
                    }
                    else{   // If the colour is dark:
                        btn[x, y].BackColor = Color.DarkGreen;  // Sets the button colour to dark green
                        dark = false;   // Sets the dark value to false
                    }
                    
                    btn[x, y].FlatStyle = FlatStyle.Flat;   // Sets the visual style of the button as 'flat'
                    //btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_MouseDown);  // Adds an event handler to when the button is pressed
                    Controls.Add(btn[x, y]);    // Adds the button to the Control form

                }

            }                  
            
            this.Size = new System.Drawing.Size(width, height); //Define size of window
        }
        int generateBombs(int x, int y, int bombs,int clickX, int clickY)   // Function used to generate the bombs on the form
        {
            bool validBombs = false;    // Boolean variable to check if the bomb location is valid
            int bombsStart = bombs;     // Integer variable to store the amount of bombs in the game
            while (!validBombs)     // While the bombs in the game are not valid
            {
                while (bombs > 0)   // While the number of bombs is greater than 0
                {
                    int ranX = r.Next(x);   // Randomise the x value of the button coordinates
                    int ranY = r.Next(y);   // Randomise the y value of the button coordinates

                    if (!bombArray[ranX, ranY]) // Checks if the chosen position isn't already taken
                    {
                        if (!(clickX == ranX && clickY == ranY))    // 
                        {
                            bombArray[ranX, ranY] = true;   // Change bombArray for this bomb to true                         
                            //int panelList = (ranY * 16) + ranX;
                            //Panel[panelList].IsMine = true;
                            bombs--;    // Decrease bomb counter by one
                        }
                    }
                }
                if (checkSurroundings(clickX, clickY) == 0) // Checks if there are any surrounding bombs
                {
                    validBombs = true;  // Set valid bombs boolean to true
                }
                else   // If there are no surrounding bombs
                {
                    bombs = bombsStart; // Set the bombs variable to the bombstart counter
                    Array.Clear(bombArray, 0, bombArray.Length);
                }
            }          
            return bombs;   // Return the variable 'bombs'
        }

        void btnEvent_MouseDown(object sender, MouseEventArgs e)    // Function that decides what happens when a button is clicked
        {
            Button ctrl = ((Button)sender); // Create an instance of the button clicked

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
                    Console.WriteLine(ctrl.Name + " was clicked");  // Write the button clicked to the output window
                    if (gameStart == false) // Checks if the game has not started
                    {
                        generateBombs(panelsX, panelsY, bombs, x, y); // Generate the bombs in the game
                        for (int i = 0; i < panelsX; i++)    // Loops for the amount of bombs in the game
                        {
                            for (int j = 0; j < panelsY; j++)
                            {
                                surroundingsArray[i, j] = checkSurroundings(i, j);  // Call the method to check surrounding areas for bombs and store the results in an array
                                /*
                                if (bombArray[i,j])
                                {
                                    btn[i, j].BackColor = Color.Red;
                                }
                                */
                            }
                        }
                        gameStart = true;   // Set the game to start
                        btn[x, y].BackColor = Color.SaddleBrown;    // Change the colour of the button to brown
                        btn[x, y].Enabled = false;  // Disable the button
                        digAround(x, y);    // Call the method to remove unimportant squares surrounding the button
                    }
                    else   // If the game has started
                    {
                        int surrounding = checkSurroundings(x, y);  // Call the method to check the surrounding buttons and store the value in an integer variable
                        if (surrounding > 0)    // If there are any bombs surrounding the square:
                        {
                            btn[x, y].BackColor = Color.SaddleBrown;    // Set the colour of the square to brown
                            btn[x, y].Text = Convert.ToString(surrounding); // Display how many bombs are in the surroundings in the square
                            btn[x, y].Enabled = false;  // Disable the button
                        }
                        else  // If there are no bombs in the surrounding:
                        {
                            btn[x, y].BackColor = Color.SaddleBrown;    // Change the square colour to brown
                            btn[x, y].Enabled = false;  // Disable the button
                            digAround(x, y);    // Call the function
                        }

                    }
                }            
            }
            if (e.Button == MouseButtons.Right && gameStart==true) //ON RIGHT CLICK AND GAME HAS STARTED
            {

                    if (ctrl.BackColor == Color.Orange) // Checks if the square selected has been flagged already
                    {
                        Console.WriteLine(ctrl.Name + " was unflagged");    // Output which square was unflagged to the display
                        ctrl.BackColor = Color.Green;   // Change the colour of the square to green
                        ctrl.Text = ""; // Remove any text that was in the button
                        flagsLeft++;    // Increase the amount of flags the user has by one
                    }
                    else  // If the square has not been flagged
                    {
                        if (flagsLeft == 0) // Checks if there are no flags left to be used
                        {
                            MessageBox.Show("Please remove a flag to place another", "Max Number of Flags in Play");    // Displays message to the user
                        }
                        else  // If there is at least 1 flag remaining:
                        {
                            Console.WriteLine(ctrl.Name + " was flagged");  // Displays which button was flagged
                            ctrl.BackColor = Color.Orange;  // Changes the button colour to orange
                            ctrl.Text = "F";    // Displays an 'F' on the flagged button
                            flagsLeft--;    // Removes one usable flag from the user
                            int correctFlags = bombs;   // Stores the number of bombs on the grid in a new integer variable

                            for (int i = 0; i < panelsX; i++)    // Loops for the total amount of buttons on the grid
                            {
                                for (int j = 0; j < panelsY; j++)
                                {
                                    if (bombArray[i,j] && btn[i,j].BackColor==Color.Orange) // Checks if the button location is valid and if it has been flagged
                                    {
                                        correctFlags--; // Removes one counter from the correctFlags variable
                                    }
                                    if (correctFlags==0)    // Checks if the correctFlags counter has reached 0
                                    {
                                        MessageBox.Show("YOU WON!!!!", "WINNER!");  // Displays a message
                                        gameRestart();  // Restarts the game
                                    }
                                }
                            }
                    }

                    }
                
                
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
        }

        void digAround(int x, int y)    // Function used to remove the surrounding unimportant squares
        {
            digUp(x,y);
            digDown(x,y);
            digLeft(x,y);
            digRight(x,y);
        }
        void digUp(int x, int y)    // Function used to remove the buttons above the chosen square
        {
            while (y > 0)   // While the y-coordinate is still a viable number and not on the edge of the grid
            {
                y--;    // Move up one in the y-coordinate
                if (surroundingsArray[x,y]>0)   // Check if there are any surrounding bombs. If so:
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]); // Display how many bombs are surrounding on the square
                    btn[x, y].BackColor = Color.SaddleBrown;    // Change the colour of the sqaure to brown
                    btn[x, y].Enabled = false;  // Disable the square

                    if (x>0)    // Checks if the button is not on the edge of the grid
                    {
                        int surrounding = checkSurroundings(x - 1, y);  // Checks if there are any bombs surrounding the square to the left and stores the value in an integer variable
                        if (surrounding > 0 && !(bombArray[x - 1, y]))  // Checks if there is a bomb surrounding the square to the left and if the square isn't a bomb
                        {
                            btn[x - 1, y].BackColor = Color.SaddleBrown;    // Changes the colour of the square to the left to brown
                            btn[x - 1, y].Text = Convert.ToString(surrounding); // Changes that square to display how many bombs are surrounding it
                            btn[x - 1, y].Enabled = false;  // Disable the square
                        }
                    }
                    if (x < panelsX-1) // Checks if the square is not on the right edge of the form
                    {
                        int surrounding = checkSurroundings(x + 1, y);  // Checks the surroundings of the square to the right and stores the value in an integer variable
                        if (surrounding > 0 && !(bombArray[x + 1, y]))  // Checks if there are bombs surrounding the square to the right and checks it is not a bomb
                        {
                            btn[x + 1, y].BackColor = Color.SaddleBrown;    // Changes the colour of that square to brown
                            btn[x + 1, y].Text = Convert.ToString(surrounding); // Displays how many bombs are surrounding that square on the square
                            btn[x + 1, y].Enabled = false;  // Disables the square
                        }
                    }
                    return; // End function
                }
                else  // Checks if there are no bombs in the surrounding squares
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))    // Checks the colour is not brown
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;    // Changes the colour of the square to brown
                        btn[x, y].Enabled = false;  // Disables the button
                        digLeft(x, y);  // Calls method to dig to the left of the square
                        digRight(x, y); // Calls method to dig to the right of the square    
                    }
                }
            }
        }
        void digDown(int x, int y)  // Function used to remove unimportant squares below the button
        {
            while (y < panelsY-1)  // Checks the button is not at the bottom of the grid
            {
                y++;    // Moves to the next square below
                if (surroundingsArray[x, y] > 0)    // If there is at least one bomb surrounding the square:
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]); // Display how many bombs are surrounding the square on the square
                    btn[x, y].BackColor = Color.SaddleBrown;    // Changes the colour of the button to brown
                    btn[x, y].Enabled = false;  // Disables the button
                    if (x > 0)  // If the button is not on the left edge of the grid
                    {
                        int surrounding = checkSurroundings(x - 1, y);  // Checks the surrounding of the square to the left and stores the value in an integer variable
                        if (surrounding > 0 && !(bombArray[x - 1, y]))  // Checks if there are bombs surrounding that square and if it not a bomb
                        {
                            btn[x - 1, y].BackColor = Color.SaddleBrown;    // Changes the colour of that square to brown
                            btn[x - 1, y].Text = Convert.ToString(surrounding); // Displays the number of surrounding bombs on the square
                            btn[x - 1, y].Enabled = false;  // Disables that square
                        }
                    }
                    if (x < panelsX-1) // If the button is not on the right edge of the grid
                    {
                        int surrounding = checkSurroundings(x + 1, y);  // Checks the surroundings of the bomb to the right
                        if (surrounding > 0 && !(bombArray[x + 1, y]))  // Checks if there are bombs surrounding that square and if that square is not a bomb
                        {
                            btn[x + 1, y].BackColor = Color.SaddleBrown;    // Changes the colour of that bomb to brown
                            btn[x + 1, y].Text = Convert.ToString(surrounding); // Displays the number of surrounding bombs on the square
                            btn[x + 1, y].Enabled = false;  // Disables the square
                        }
                    }
                    return; // End the function
                }
                else  // If there are no bombs surrounding the square:
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))    // If the square colour is not brown
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;    // Change the square colour to brown
                        btn[x, y].Enabled = false;  // Disable the button
                        digLeft(x, y);  // Call the method to dig squares to the left
                        digRight(x, y); // Call the method to dig squares to the right
                    }
                }
            }
        }

        void digRight(int x, int y) // Function used to dig up unimportant squares to the right of the square
        {
            while (x < panelsX-1)  // While the square is not on the right edge of the form
            {
                x++;    // Move the square to the right
                if (surroundingsArray[x, y] > 0)    // Checks if there are any bombs surrounding the square
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]); // Displays the amount of surrounding bombs on the square
                    btn[x, y].BackColor = Color.SaddleBrown;    // Change the colour of the square to brown
                    btn[x, y].Enabled = false;  // Disable the button
                    return; // End the function
                }
                else  // If there are no bombs in the surrounding area
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))    // If the button colour is not brown:
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;    // Change the colour of the button to brown
                        btn[x, y].Enabled = false;  // Disables the button
                        digUp(x, y);    // Call the method to dig the squares above the button
                        digDown(x, y);  // Call the method to dig the sqaures below the button
                    }
                }
            }
        }

        void digLeft(int x, int y)  // Function used to remove the unimportant sqaures to the left of the square
        {
            while (x > 0)   // While the square is not on the left edge
            {
                x--;    // Remove one counter from the x-coordinate
                if (surroundingsArray[x, y] > 0)    // Checks if there are any bombs in the surrounding area
                {
                    btn[x, y].Text = Convert.ToString(surroundingsArray[x, y]); // Displays the number of surrounding bombs on the square
                    btn[x, y].BackColor = Color.SaddleBrown;    // Changes the colour of button to brown
                    btn[x, y].Enabled = false;  // Disable the button
                    return; // End the function
                }
                else   // If there are no bombs in the surrounding area
                {
                    if (!(btn[x, y].BackColor == Color.SaddleBrown))    // If the colour of the button is not brown
                    {
                        btn[x, y].BackColor = Color.SaddleBrown;    // Change the colour of the button to brown
                        btn[x, y].Enabled = false;  // Diable the button
                        digUp(x, y);    // Call the method to remove squares above the square
                        digDown(x, y);  // Call the method to remove sqaures below the sqaure
                    }
                }
            }
        }


        int checkSurroundings(int x, int y) // Function used to check the surrounding of a button to detect bombs
        {
            int surroundingBombCount = 0;   // Create an integer variable to store the number of surrounding bombs

            if ((x > 0) && (y < panelsY-1))    // Checks the button is on the bottom edge and on the left edge
            {
                //if there's a bomb bottom left
                if (bombArray[x-1,y+1])
                {
                    surroundingBombCount++; // Add a counter to the bomb counter
                }
            }
            if (y < panelsY-1) // Checks if the bomb is not on the bottom grid
            {
                //if there's a bomb below
                if (bombArray[x, y + 1])
                {
                    surroundingBombCount++; // Add a counter to the bomb counter
                }
            }
            if (x < panelsX-1 && y < panelsY-1)   // Checks if the button is on the bottom right edge of the grid
            {
                //if there's a bomb bottom right
                if (bombArray[x + 1,y + 1])
                {
                    surroundingBombCount++; // Add a counter to the bomb counter
                }
            }
            if (x > 0)  // If the button is not on left edge
            {
                //if there's a bomb left
                if (bombArray[x - 1, y])
                {
                    surroundingBombCount++; // Adds a counter to the bomb counter
                }
            }
            if (x < panelsX-1) // If the button is not on the right edge
            {
                //if there's a bomb right
                if (bombArray[x + 1, y])
                {
                    surroundingBombCount++; // Adds a counter to the bomb counter
                }
            }
            if (x > 0 && y > 0) // Check the button is at the top left
            {
                //if there's a bomb top left
                if (bombArray[x - 1, y - 1])
                {
                    surroundingBombCount++; // Adds a counter to the bomb counter
                }
            }
            if (y > 0)  // If the button is not at the top edge
            {
                //if there's a bomb above
                if (bombArray[x, y - 1])
                {
                    surroundingBombCount++; // Adds a counter to the bomb counter
                }
            }
            if (x < panelsX-1 && y > 0)    // Check if the button is on the top right
            {
                //if there's a bomb top right
                if (bombArray[x + 1, y - 1])
                {
                    surroundingBombCount++; // Adds a counter to the bomb counter
                }
            }
            //btn[x, y].BackColor = Color.White;
            //btn[x, y].Text = surroundingBombCount;

            return surroundingBombCount;    // Ends the function
        }

        private void MainGame_Load(object sender, EventArgs e) //REQUIRED
        {
        }

        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)   // Adds a Close game option
        {
            this.Close();   // Close the program
        }

        private void HighscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void ResartToolStripMenuItem_Click(object sender, EventArgs e)  // Adds an option to close the game
        {
            gameRestart();  // Restart the game
        }
        private void gameRestart()  // Function used to restart the game
        {
            this.Hide();    // Hide the game panel
            MainGame mainGame = new MainGame(difficulty);   // Call method to start the game using a chosen difficulty
            mainGame.ShowDialog();
            this.Close();   // Close the old game
        }
        private void bombCheck(int x, int y)    // Function end the game when a bomb is clicked
        {
            if (bombArray[x,y]) // Once a bomb is clicked
            {
                for(int i = 0 ; i < panelsX; i++)
                {
                    for (int j = 0; j < panelsY; j++)
                    {
                        if (bombArray[i, j])
                        {
                            btn[i, j].BackColor = Color.Red;
                        }
                    }
                }
                MessageBox.Show("GAME OVER!", "Game Over"); // Displays a message
                gameRestart();  // Calls the method to restart the game
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
            
