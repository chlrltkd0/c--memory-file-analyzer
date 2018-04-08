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
using System.Collections;
using System.ComponentModel.Design;

namespace MemoryManager
{
    class EditMemory
    {
        public static bool listviewexist(ListView lv, int num)
        {
            string tmp;
            try
            {
                tmp = lv.Items[num].SubItems[0].Text;
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static bool MemoryScanAOB(IntPtr hProcess, int start, int end, string AOB, ref ArrayList al)
        {
            string[] str = AOB.Split(' ');
            int strlen = str.Length;
            byte[] src = new byte[strlen];
            byte[] dst = new byte[strlen];
            int readn;
            bool result;

            for (int i = 0; i < strlen; i++)
                src[i] = Convert.ToByte(str[i], 16);

            for (int i = start; i < end; i++)
            {
                result = Win32API.ReadProcessMemory(hProcess, (IntPtr)i, dst, strlen, out readn);
                if (result == false)
                    continue;
                for (int j = 0; j < strlen; j++)
                {
                    if (src[j] == dst[j])
                    {
                        if (j == strlen - 1)
                            al.Add(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return true;
        }

        public static Boolean WriteMemoryAOB(IntPtr hProcess, IntPtr BaseAddress, string AOB)
        {
            int written;
            string[] tempaob = AOB.Split(' ');
            int size = tempaob.Length - 1;
            byte[] write = new byte[size];
            int old = 0;
            for (int i = 0; i < size; i++)
            {
                write[i] = Convert.ToByte(tempaob[i].Trim(), 16);
            }
            bool result = Win32API.VirtualProtectEx(hProcess, BaseAddress, size, Win32API.PAGE_EXECUTE_READWRITE, out old);
            result = Win32API.WriteProcessMemory(hProcess, BaseAddress, write, size, out written);
            Win32API.VirtualProtectEx(hProcess, BaseAddress, size, old, out old);
            return result;
        }

        public static Boolean ReadMemory(IntPtr hProcess, IntPtr BaseAddress, int bufferSize, ByteViewer bv)
        {
            int read;
            byte[] rbuffer = new byte[bufferSize];
            bool result = Win32API.ReadProcessMemory(hProcess, BaseAddress, rbuffer, bufferSize, out read);
            if (result == false)
            {
                MessageBox.Show("메모리 읽기 실패", "MyFunc.ReadMemory() Error");
                return false;
            }
            bv.SetBytes(rbuffer);
            return true;
        }
        
        public static Boolean ReadMemory(IntPtr hProcess, IntPtr BaseAddress, int bufferSize, out string buffer)
        {
            int read;
            byte[] rbuffer = new byte[bufferSize];
            bool result = Win32API.ReadProcessMemory(hProcess, BaseAddress, rbuffer, bufferSize, out read);
            buffer = "";
            if (result == false)
            {
                MessageBox.Show("메모리 읽기 실패", "MyFunc.ReadMemory() Error");
                return false;
            }
            foreach (byte buf in rbuffer)
            {
                buffer += Convert.ToString(buf, 16).PadLeft(2, '0').ToUpper() + " ";
            }
            return true;
        }
        
        public static ArrayList FindModule(Process proc)
        {
            ArrayList al = new ArrayList();
            foreach (ProcessModule pm in proc.Modules)
            {
                al.Add(pm.FileName);
                al.Add(pm.BaseAddress.ToString());
                al.Add(pm.ModuleMemorySize.ToString());
                al.Add(pm.BaseAddress.ToString());
            }
            return al;
        }
        /*
        public static void FindModule(int pid, ref ArrayList al)
        {
            IntPtr hSnapShot = Win32API.CreateToolhelp32Snapshot(Win32API.TH32CS_SNAPMODULE, pid);
            Win32API.MODULEENTRY32 me = new Win32API.MODULEENTRY32();
            me.dwSize = Win32API.MODULEENTRY32.Size;
            Win32API.Module32First(hSnapShot, ref me);
            do
            {
                al.Add(me.dwSize);
                al.Add(me.GlblcntUsage);
                al.Add(me.hModule);
                al.Add(me.modBaseAddr);
                al.Add(me.modeBaseSize);
                al.Add(me.proccntUsage);
                al.Add(me.szExePath);
                al.Add(me.szModule);
                al.Add(me.th32ModuleID);
                al.Add(me.th32ProcessID);
            }
            while(Win32API.Module32Next(hSnapShot, ref me)!=IntPtr.Zero);
            return;
            
        }*/
    }
}
