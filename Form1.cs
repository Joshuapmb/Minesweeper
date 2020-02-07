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
        Random r = new Random();
        Button[,] btn = new Button[16, 16]; // Create 2D array of buttons
        bool[,] bombArray = new bool[16, 16]; // Create 2D boolean bomb array

       
        public Form1()
        {        
            InitializeComponent();
            for (int x = 0; x < btn.GetLength(0); x++) // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++) // Loop for y
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(30 * x, (30 * y)+30, 30, 30);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
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
                ctrl.BackColor = Color.Black;
            }
            if (e.Button == MouseButtons.Right)
            {
                ctrl.BackColor = Color.Beige;
            }
            
            //Console.WriteLine(((Button)sender).Text); // SAME handler as before
        }

        private void Form1_Load(object sender, EventArgs e) //REQUIRED
        {
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HighscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
            
