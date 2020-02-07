using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhackAMoleExample
{
    public partial class Form1 : Form
    {
        Button [,] btn = new Button[16,16];
        ContextMenuStrip menu = new ContextMenuStrip();
        Random r = new Random();
        
        public Form1()
        {
            InitializeComponent();
            //Controls.Add(menu);
            for (int x = 0; x < btn.GetLength(0); x++)
            {
                for (int y = 0; y < btn.GetLength(1); y++)
                {
                    btn[x,y] = new Button();
                    btn[x,y].SetBounds(25 * x, 25 * y, 26, 26);
                    btn[x,y].BackColor = Color.PowderBlue;
                    //btn[x,y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x,y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x,y]);
                }
            }
            
            int bombCount = 40;
            while (bombCount > 0)
            {
                int x = r.Next(16);
                int y = r.Next(16);

                
                if (btn[x,y].BackColor != Color.Red)
                {
                    btn[x, y].BackColor = Color.Red;
                    bombCount -= 1;
                }
                
            }
        }
        void btnEvent_Click(Object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Red)
            {
                ((Button)sender).BackColor = Color.PowderBlue;
                btn[r.Next(5), r.Next(5)].BackColor = Color.Red;
                Console.WriteLine("WHACKED!");
            }
            else
            {
                Console.WriteLine("Missed!");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
