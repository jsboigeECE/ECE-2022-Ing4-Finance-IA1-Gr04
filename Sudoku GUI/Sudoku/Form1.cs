using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
  
        public Form1()
        {
            InitializeComponent();
          
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Point p = new Point(40, 49);
            Control it = groupBox1.GetChildAtPoint(p);
            int[] tab = new int[81];

            for (int i = 0; i < 81; i++)
            {
                if ( (it.Text == "1") || (it.Text == "2") || (it.Text == "3") || (it.Text == "4") || (it.Text == "5") || (it.Text == "6") || (it.Text == "7") || (it.Text == "8") || (it.Text == "9"))
                {
                    tab[i] = int.Parse(it.Text);
                }else {
                    tab[i] = 0;
                }
                it = groupBox1.GetNextControl(it, true);
                
            }

            for (int j = 0; j < 81; j++)
            {
                Console.WriteLine(tab[j]);
            }
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }
    }
}
