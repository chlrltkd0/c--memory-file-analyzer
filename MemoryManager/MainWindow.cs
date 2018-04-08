using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.ComponentModel.Design;

namespace MemoryManager
{

    public partial class MainWindow : Form
    {
        IntPtr hProcess;
        Process MyProc;
        ArrayList mysavedata;
        ArrayList findaddress;
        FileStructure Fstruct;
        ByteViewer bv;

        public MainWindow()
        {
            InitializeComponent();
            this.mysavedata = new ArrayList();
            this.findaddress = new ArrayList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessList dlg = new MemoryManager.ProcessList();
            //dlg.Show();
            if (dlg.ShowDialog()==System.Windows.Forms.DialogResult.OK)// dlg.ProcessInfo;
            {
                MyProc = dlg.ProcessInfo;

                hProcess = Win32API.OpenProcess(Win32API.PROCESS_ALL_ACCESS, false, MyProc.Id);
                
                if (hProcess!=IntPtr.Zero)
                {
                    label1.Text = MyProc.Id + " - " + MyProc.ProcessName;
                    this.Text = "Anonymous " + "    [attach : " + MyProc.Id + " : " + MyProc.ProcessName + "]";
                    label_pid.Text = MyProc.Id.ToString();
                    label_ph.Text = hProcess.ToString();
                    panel2.Enabled = true;
                    mydataupdate.Enabled = true;

                }
                else
                {
                    this.Text = "Anonymous";
                    label1.Text = " Process Open Failed ";
                    label_pid.Text = "None";
                    label_ph.Text = "None";
                    richTextBox1.Text = Marshal.GetLastWin32Error().ToString();
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.anonymous;
            listView1.Columns.Add("Active", 50);
            listView1.Columns.Add("Description", 100);
            listView1.Columns.Add("Address", 100);
            listView1.Columns.Add("Type", 100);
            listView1.Columns.Add("Value", 190);
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.LabelEdit = true;
            comboBox1.Items.Add("Array Of Byte");
            comboBox1.SelectedIndex = 0;

            listView_section.Columns.Add("", 10, HorizontalAlignment.Right);
            listView_section.Columns.Add("Name", 50, HorizontalAlignment.Right);
            listView_section.Columns.Add("VirtualSize", 100, HorizontalAlignment.Right);
            listView_section.Columns.Add("VirtualOffset", 100, HorizontalAlignment.Right);
            listView_section.Columns.Add("RawSize", 100, HorizontalAlignment.Right);
            listView_section.Columns.Add("RawOffset", 100, HorizontalAlignment.Right);
            listView_section.Columns.Add("Characteristics", 100, HorizontalAlignment.Right);
            listView_section.View = View.Details;
            listView_section.GridLines = true;
            listView_section.FullRowSelect = true;
            listView_section.LabelEdit = true;

            listView_datadirectories.Columns.Add("DESC", 90, HorizontalAlignment.Right);
            listView_datadirectories.Columns.Add("RVA", 85, HorizontalAlignment.Right);
            listView_datadirectories.Columns.Add("Size", 85, HorizontalAlignment.Right);
            listView_datadirectories.View = View.Details;
            listView_datadirectories.GridLines = true;
            listView_datadirectories.FullRowSelect = true;
            listView_datadirectories.LabelEdit = true;

            bv = new ByteViewer();
            bv.Height = 267;
            bv.Width = 712;
            bv.Location = new Point(10, 364);
            //bv.
            tabPage2.Controls.Add(bv);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listView1.Items[0].SubItems[1].Edit = "123";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_threadinject_Click(object sender, EventArgs e)
        {
            binaryinjectionWindow biw = new binaryinjectionWindow(MyProc);
            biw.Show();
        }

        private void openFileDialog1_FileOk(object sender, EventArgs e)
        {

        }

        private void btn_manualmapping_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox1.Text = openFileDialog.FileName;
                foreach (ProcessThread pt in MyProc.Threads)
                {
                    richTextBox1.AppendText(pt.Id.ToString() + "  ");
                    richTextBox1.AppendText(pt.ThreadState.ToString() + "  ");
                    richTextBox1.AppendText(pt.StartAddress.ToInt32().ToString() + "\n");

                }
            }
        }

        private void btn_dllinject_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                dllInjectionWindow form4 = new dllInjectionWindow(MyProc, openFileDialog.FileName);
                form4.Show();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ProcessInfo form3 = new ProcessInfo(MyProc);
            form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddMemoryDialog amd = new AddMemoryDialog();
            if( amd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                string buffer;
                bool result = EditMemory.ReadMemory(MyProc.Handle, (IntPtr)amd.address, 0x10, out buffer);
                if (result == false)
                    return;
                
                mysavedata.Add(new mydata((IntPtr)amd.address, amd.type, amd.decription));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(websitetext.Text);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            treeView1.Nodes.Clear();
            if (webBrowser1.Document!=null)
            {
                HtmlElementCollection hec = webBrowser1.Document.GetElementsByTagName("form");
                if (hec.Count!=0)
                {
                    foreach (HtmlElement he2 in hec)
                    {
                        string pageSource = he2.OuterHtml;
                        TreeNode tn = new TreeNode(pageSource.Substring(0, pageSource.IndexOf('>')+1));
                        while(true)
                        {
                            int tmp = pageSource.IndexOf("<input");
                            if (tmp == -1)
                                break;
                            pageSource = pageSource.Substring(tmp+1);
                            tn.Nodes.Add("<" + pageSource.Substring(0, pageSource.IndexOf('>')+1));
                        }
                        treeView1.Nodes.Add(tn);
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mydataupdate_Tick(object sender, EventArgs e)
        {
            MyProc.Refresh();
            if (MyProc.HasExited == true)
                return;

            for(int i=0; i<mysavedata.Count; i++)
            {
                string tmp;
                mydata md = mysavedata[i] as mydata;
                EditMemory.ReadMemory(MyProc.Handle, md.address, 10, out tmp);
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(md.desc);
                lvi.SubItems.Add(Convert.ToString((int)md.address, 16).PadLeft(8, '0').ToUpper());
                lvi.SubItems.Add(md.type);
                lvi.SubItems.Add(tmp);

                if (EditMemory.listviewexist(listView1, i))
                { 
                    if(Convert.ToString((int)md.address,16).PadLeft(8,'0').ToUpper()!=listView1.Items[i].SubItems[2].Text)
                    {
                        listView1.Items.RemoveAt(i);
                        listView1.Items.Insert(i, lvi);
                    }
                    if(md.desc != listView1.Items[i].SubItems[1].Text)
                    {
                        listView1.Items.RemoveAt(i);
                        listView1.Items.Insert(i, lvi);
                    }
                    if(md.type != listView1.Items[i].SubItems[3].Text)
                    {
                        listView1.Items.RemoveAt(i);
                        listView1.Items.Insert(i, lvi);
                    }
                    if(tmp!= listView1.Items[i].SubItems[4].Text)
                    {
                        listView1.Items.RemoveAt(i);
                        listView1.Items.Insert(i, lvi);
                    }
                }
                else
                    listView1.Items.Insert(i, lvi);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MemoryWriteDialog mwd = new MemoryWriteDialog(listView1.SelectedItems[0].SubItems[4].Text);
            if(mwd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                bool result = EditMemory.WriteMemoryAOB(MyProc.Handle, (IntPtr)Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text, 16), mwd.AOB);
                if (result == false)
                    MessageBox.Show("WriteMemoryAOB Error");
            }
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                MemoryWriteDialog mwd = new MemoryWriteDialog(listView1.SelectedItems[0].SubItems[4].Text);
                mwd.ShowDialog();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.findaddress.Clear();
            EditMemory.MemoryScanAOB(MyProc.Handle, Convert.ToInt32(textBox2.Text, 16), Convert.ToInt32(textBox3.Text, 16), textBox1.Text, ref this.findaddress);
            foreach(int tmp in this.findaddress)
            {
                listBox1.Items.Add(Convert.ToString(tmp, 16).PadLeft(8, '0').ToUpper());
            }
            
        }

        private void btn_openfile_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[40];
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label_openfilepath.Text = openFileDialog.FileName;
                openfile(openFileDialog.FileName);
                bv.SetFile(openFileDialog.FileName);
                bv.SetDisplayMode(DisplayMode.Auto);
                
                //bv.SetRow(10);
            }
        }

        private void openfile(string path)
        {

            Fstruct = new FileStructure(openFileDialog.FileName);

            textBox_emagic.Text = Fstruct.dosheader.signiture.ToString();
            textBox_elfanew.Text = string.Format("{0:X4}", Fstruct.dosheader.e_lfanew);

            textBox_entrypoint.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.AddressOfEntryPoint);
            textBox_imagebase.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.ImageBase);
            textBox_sizeofimage.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.SizeOfImage);
            textBox_sectionalignment.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.SectionAlignment);
            textBox_filealignment.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.FileAlignment);
            textBox_numberofsections.Text = string.Format("{0:X8}", Fstruct.ntheader.fileheader.NumberOfSection);
            textBox_characteristics.Text = string.Format("{0:X8}", Fstruct.ntheader.fileheader.Characteristics);
            textBox_sizeofheaders.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.SizeOfHeaders);
            textBox_checksum.Text = string.Format("{0:X8}", Fstruct.ntheader.optionalheader.Checksum);
            textBox_machine.Text = string.Format("{0:X8}", Fstruct.ntheader.fileheader.Machine);

            foreach (DataDirectory dd in Fstruct.ntheader.optionalheader.DataDirectories)
            {
                ListViewItem lvi = new ListViewItem(dd.desc);
                lvi.SubItems.Add(string.Format("{0:X8}", dd.RVA));
                lvi.SubItems.Add(string.Format("{0:X8}", dd.Size));
                listView_datadirectories.Items.Add(lvi);
            }

            listView_section.Items.Clear();
            foreach (SectionHeader sh in Fstruct.sectionheader)
            {

                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(sh.Name);
                lvi.SubItems.Add(string.Format("{0:X8}", sh.VirtualSize));
                lvi.SubItems.Add(string.Format("{0:X8}", sh.RVA));
                lvi.SubItems.Add(string.Format("{0:X8}", sh.SizeOfRawData));
                lvi.SubItems.Add(string.Format("{0:X8}", sh.PointerToRawData));
                lvi.SubItems.Add(string.Format("{0:X8}", sh.Characteristics));

                listView_section.Items.Add(lvi);

            }

        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void listView_section_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            openfile(FileList[0]);
        }

        private void listView_section_Enter(object sender, EventArgs e)
        {
            listView_section.Width = 600;
        }

        private void listView_section_Leave(object sender, EventArgs e)
        {
            listView_section.Width = 160;
        }
    }
    class mydata
    {
        IntPtr daddr;
        string dtype;
        string ddesc;

        public mydata(IntPtr addr, string type, string desc)
        {
            this.daddr = addr;
            this.dtype = type;
            this.ddesc = desc;
        }

        public IntPtr address
        {
            get { return this.daddr; }
            set { this.daddr = value; }
        }

        public string type
        {
            get { return this.dtype; }
            set { this.dtype = value; }
        }

        public string desc
        {
            get { return this.ddesc; }
            set { this.ddesc = value; }
        }
    }

 
    

}
