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
    public partial class AddMemoryDialog : Form
    {
        public AddMemoryDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void EndDialog()
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            
        }

        public string decription
        {
            get { return textBox2.Text; }
        }

        public int address
        {
            get { return Convert.ToInt32(textBox1.Text,16); }
        }

        public string type
        {
            get { return comboBox1.SelectedItem.ToString(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void AddMemoryDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Array Of Byte");

            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
