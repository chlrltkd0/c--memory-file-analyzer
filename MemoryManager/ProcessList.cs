using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace MemoryManager
{
    public partial class ProcessList : Form
    {
        Process[] plist;
        ArrayList spalist;
        Process myp;
        string search;

        public ProcessList()
        {
            InitializeComponent();
            spalist = new ArrayList();
            search = "";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.anonymous;
            plist = Process.GetProcesses();

            foreach (Process p in plist)
            {
                listBox1.Items.Add(p.Id.ToString().PadLeft(6, ' ') + " - " + p.ProcessName);
                //richTextBox1.AppendText(p.ProcessName + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }



        public Process ProcessInfo
        {
            get { return myp; }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            if (search.Length==0)
                myp = plist[listBox1.SelectedIndex];
            else
                myp = (Process)spalist[listBox1.SelectedIndex];
            
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==8)
            {
                if (search.Length == 0)
                { 
                    this.Text = "Anonymous";
                    return;
                }
                search = search.Substring(0, (search.Length)-1);
            }
            else
            {
                search = search + Convert.ToChar(e.KeyValue);              
            }
            this.Text = search;
        }

        private void Form2_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            spalist.Clear();
            foreach (Process p in plist)
            {
                if (p.ProcessName.Length < search.Length)
                    continue;
                if (p.ProcessName.Substring(0,search.Length).ToUpper()==search.ToUpper())
                { 
                    listBox1.Items.Add(p.Id.ToString().PadLeft(6, ' ') + " - " + p.ProcessName);
                    spalist.Add(p);
                }
                //richTextBox1.AppendText(p.ProcessName + "\n");
            }
        }
    }
}
