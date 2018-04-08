using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryManager
{
    public partial class MemoryReadDialog : Form
    {
        public MemoryReadDialog()
        {
            InitializeComponent();
        }

        private void MemoryReadDialog_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.anonymous;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EndDialog();
        }
        private void EndDialog()
        {
            int r1;
            int r2;

            try
            {
                r1 = Convert.ToInt32(textBox1.Text, 16);
                if (textBox2.Text == "")
                {
                    r2 = r1 + 0x1000;
                    textBox2.Text = Convert.ToString(r2, 16);
                }
                else
                    r2 = Convert.ToInt32(textBox1.Text, 16);
                
                if (r1 > r2)
                    return;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch
            {
                return;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public int range1
        {
            get { return Convert.ToInt32(textBox1.Text, 16); }
        }
        public int range2
        {
            get { return Convert.ToInt32(textBox2.Text, 16); }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==13)
            {
                EndDialog();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                EndDialog();
            }
        }
    }
}
