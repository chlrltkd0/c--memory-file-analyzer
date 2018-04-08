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

namespace MemoryManager
{
    public partial class dllInjectionWindow : Form
    {
        Process MyProc;
        byte[] dllpath2;
        string dllpath;
        public dllInjectionWindow(Process MyProc, string dllpath)
        {
            InitializeComponent();
            this.MyProc = MyProc;
            this.dllpath = dllpath;
            dllpath2 = Encoding.UTF8.GetBytes(dllpath);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.anonymous;            
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bi;

            IntPtr kernel = Win32API.GetModuleHandle("kernel32.dll");
            if (kernel == IntPtr.Zero)
                label2.Text = "ERROR";
            else
            {
                label2.Text = Convert.ToString(kernel.ToInt64(), 16);
                progressBar1.PerformStep();
            }

            IntPtr lploadlibrary = Win32API.GetProcAddress(kernel, "LoadLibraryA");
            if (lploadlibrary == IntPtr.Zero)
                label4.Text = "ERROR";
            else
            {
                label4.Text = Convert.ToString(lploadlibrary.ToInt64(), 16);
                progressBar1.PerformStep();
            }

            IntPtr param = Win32API.VirtualAllocEx(MyProc.Handle, (IntPtr)0, 100, Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            if (param == IntPtr.Zero)
                label6.Text = "ERROR";
            else
            {
                label6.Text = Convert.ToString(param.ToInt64(), 16);
                progressBar1.PerformStep();
            }
            bool wpm = Win32API.WriteProcessMemory(MyProc.Handle, param, this.dllpath2, this.dllpath.Length, out bi);
            if (wpm == false)
                label8.Text = "ERROR";
            else
            {
                label8.Text = Convert.ToString(wpm);
                progressBar1.PerformStep();
            }
            IntPtr remoteThread = Win32API.CreateRemoteThread(MyProc.Handle, 0, 0, lploadlibrary, param, 0, (IntPtr)0);
            if (remoteThread == IntPtr.Zero)
                label10.Text = "ERROR";
            else
            {
                label10.Text = Convert.ToString(remoteThread.ToInt64(), 16);
                progressBar1.PerformStep();
                Win32API.CloseHandle(remoteThread);
            }
        }
    }
}
