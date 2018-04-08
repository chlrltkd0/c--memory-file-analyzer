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
    public partial class MemoryAllocDialog : Form
    {
        public MemoryAllocDialog()
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
        public int address
        {
            get { return Convert.ToInt32(textBox1.Text); }
        }

        public int size
        {
            get { return Convert.ToInt32(textBox2.Text); }
        }

    }
}
