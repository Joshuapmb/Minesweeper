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
    public partial class Form1 : Form
    {
        Button[,] btn = new Button[16, 16]; // Create 2D array of buttons
        public Form1()
        {
            InitializeComponent();
            for (int x = 0; x < btn.GetLength(0); x++) // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++) // Loop for y
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(30 * x, 30 * y, 30, 30);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }
        }
        void btnEvent_Click(object sender, EventArgs e)
        {
               //if it's a bomb
            if ()
            {
                //explode
            }

            //else
            else
            {

                int surroundingBombCount = 0;


                   
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

                    if (x-1 >= 0 && y <= 15)
                    {
                        //if there's a bomb left
                        if (btn[x - 1, y] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x+1 >= 0 && y <= 15)
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

                    if (x >= 0 && y - 1 <= 15)
                    {
                        //if there's a bomb above
                        if (btn[x, y - 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }

                    if (x+1 >= 0 && y - 1 <= 15)
                    {
                        //if there's a bomb bottom right
                        if (btn[x + 1, y - 1] = bombBtn)
                        {
                            surroundingBombCount++;
                        }
                    }
                

                    btn[x, y].BackColor = Color.White;
                    btn[x, y].Text = surroundingBombCount;

            }
        }
        private void Form1_Load(object sender, EventArgs e) //REQUIRED
        {
        }

    }
}
            
