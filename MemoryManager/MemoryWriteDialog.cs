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
    public partial class MemoryWriteDialog : Form
    {
        public MemoryWriteDialog(string str)
        {
            InitializeComponent();
            this.textBox1.Text = str;
        }

        private void MemoryWriteDialog_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.anonymous;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public string AOB
        {
            get { return textBox1.Text; }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)13)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
