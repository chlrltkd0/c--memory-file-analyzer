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
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace MemoryManager
{
    public partial class ProcessInfo : Form
    {
        ByteViewer bv;
        Process MyProc;
        int modulecount;
        int threadcount;

        [DllImport("kernel32.dll")]
        public static extern Int32 IsWow64Process(IntPtr hProcess, out Boolean bWow64Process);

        public ProcessInfo(Process proc)
        {
            InitializeComponent();
            this.MyProc = proc;
            this.modulecount = this.MyProc.Modules.Count;
            this.threadcount = this.MyProc.Threads.Count;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            this.Icon = Properties.Resources.anonymous;
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("모듈이름", 100);
            listView1.Columns.Add("베이스주소", 100);
            listView1.Columns.Add("끝주소", 100);
            listView1.Columns.Add("크기", 100);
            listView1.Columns.Add("엔트리포인트", 100);
            listView1.Columns.Add("파일경로", 300);
            
            timer1.Enabled = true;

            bv = new ByteViewer();
            bv.Height = 317;
            bv.Width = 650;
            bv.Location = new Point(10,10);
 
            panel2.Controls.Add(bv);





            Boolean result = false;
            if (IsWow64Process(MyProc.Handle, out result)!=0)
            {
                if(result==true)
                {
                    richTextBox1.Text = "32";
                }

                else
                {
                    richTextBox1.Text = "64";
                }
            }
            updatemodule();
            pinfoupdate();
            updatethread();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NonPagedSystemMemory로 현재 프로세스가 쓰고있는 메모리 영역중 절대 페이징되지않는 영역을 지칭한다. \n 페이징이란 데이터가 램에서 하드디스크 영역으로 가는것이므로 실시간으로 접근해야 하는 데이터는 반드시 NonPagedMemory로 등록되어 있어야한다.","설명충 등판");
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Working Set 이란 실제 Physical RAM Memory에 올라가있는 프로세스를 말한다.\n 즉 최대로 올라 갈수있는 메모리를 뜻함.  ", "설명충 등판");
        }

        private void label15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Working Set 이란 실제 Physical RAM Memory에 올라가있는 프로세스를 말한다.\n 즉 최소로 올라 갈수있는 메모리를 뜻함.  ", "설명충 등판");
        }

        private void label19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("현재 프로세스가 사용하는 메모리공간중 페이징된 공간의 크기", "설명충 등판");
        }

        private void label21_Click(object sender, EventArgs e)
        {
            MessageBox.Show("현재 프로세스가 생성된 이후 가장많이 페이지 파일이 할당된 순간의 크기", "설명충 등판");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MyProc.Refresh();
            if (this.modulecount != MyProc.Modules.Count)
            { 
                updatemodule();
                this.modulecount = MyProc.Modules.Count;
            }
            if (this.threadcount != MyProc.Threads.Count)
            {
                updatethread();
                this.threadcount = MyProc.Threads.Count;
            }
            pinfoupdate();
            
        }
        private void updatemodule()
        {
            listView1.Items.Clear();
            foreach (ProcessModule pm in MyProc.Modules)
            {
                ListViewItem lvi = new ListViewItem(new string[] {pm.ModuleName
                    , Convert.ToString(pm.BaseAddress.ToInt64(), 16).PadLeft(8, '0').ToUpper()
                    , Convert.ToString(pm.BaseAddress.ToInt64() + pm.ModuleMemorySize, 16).PadLeft(8, '0').ToUpper()
                    , Convert.ToString(pm.ModuleMemorySize, 16).PadLeft(8, '0').ToUpper()
                    , Convert.ToString(pm.EntryPointAddress.ToInt64(), 16).PadLeft(8, '0').ToUpper()
                    , pm.FileName});
                listView1.Items.Add(lvi);
            }
        }
        private void updatethread()
        {
            listBox1.Items.Clear();
            foreach (ProcessThread pt in MyProc.Threads)
            {
                listBox1.Items.Add(Convert.ToString(pt.Id, 16).PadLeft(8, '0') + " - " + pt.ThreadState);
            }
        }
        private void pinfoupdate()
        { 
            label2.Text = Convert.ToString(MyProc.BasePriority, 16).PadLeft(8, '0').ToUpper();
            label4.Text = Convert.ToString(MyProc.Handle.ToInt32(), 16).PadLeft(8, '0').ToUpper();
            label6.Text = Convert.ToString(MyProc.HandleCount, 16).PadLeft(8, '0').ToUpper();
            label8.Text = Convert.ToString(MyProc.Id, 16).PadLeft(8, '0').ToUpper();
            label10.Text = Convert.ToString(MyProc.MainWindowHandle.ToInt32(), 16).PadLeft(8, '0').ToUpper();
            label12.Text = MyProc.MainWindowTitle;
            label14.Text = Convert.ToString(MyProc.MaxWorkingSet.ToInt64(), 16).PadLeft(8, '0').ToUpper();
            label16.Text = Convert.ToString(MyProc.MinWorkingSet.ToInt64(), 16).PadLeft(8, '0').ToUpper();
            label18.Text = Convert.ToString(MyProc.NonpagedSystemMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label20.Text = Convert.ToString(MyProc.PagedMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label22.Text = Convert.ToString(MyProc.PeakPagedMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label24.Text = Convert.ToString(MyProc.PeakVirtualMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label26.Text = Convert.ToString(MyProc.PrivateMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label28.Text = Convert.ToString(MyProc.VirtualMemorySize64, 16).PadLeft(8, '0').ToUpper();
            label30.Text = Convert.ToString(MyProc.Threads.Count, 16).PadLeft(8, '0').ToUpper();
            label32.Text = Convert.ToString(MyProc.WorkingSet64, 16).PadLeft(8, '0').ToUpper();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            int b = listView1.SelectedItems[0].Index;
            ProcessModule tpm = MyProc.Modules[b];
            EditMemory.ReadMemory(MyProc.Handle, tpm.BaseAddress, 0x4000, bv);
            //EditMemory.ReadMemory(MyProc.Handle, tpm.BaseAddress, 0x4000, listView2);

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            MemoryReadDialog mrd = new MemoryReadDialog();
            if(mrd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                int startAddress = mrd.range1;
                int endAddress = mrd.range2;
                int size = endAddress - startAddress;
                EditMemory.ReadMemory(MyProc.Handle, (IntPtr)startAddress, size, bv);
            }
        }

        private void label33_Click(object sender, EventArgs e)
        {
            MemoryAllocDialog mad = new MemoryAllocDialog();
            if (mad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IntPtr tmp = Win32API.VirtualAllocEx(MyProc.Handle, (IntPtr)mad.address, mad.size, Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
                if (tmp == IntPtr.Zero)
                    MessageBox.Show("할당 실패");
                else
                { 
                    EditMemory.ReadMemory(MyProc.Handle, tmp, mad.size, bv);
                    listBox2.Items.Add(Convert.ToString((int)tmp, 16).PadLeft(8, '0').ToUpper() + "  size : " + mad.size);
                }
            }
        }
    }
}
