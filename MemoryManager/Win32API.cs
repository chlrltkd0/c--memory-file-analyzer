using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MemoryManager
{
    class Win32API
    {
        public const Int32 PROCESS_ALL_ACCESS = 0xFFFF;
        public const Int32 TH32CS_SNAPMODULE = 0x00000008;
        public const Int32 MEM_COMMIT = 0x1000;
        public const Int32 PAGE_READWRITE = 0x04;
        public const Int32 PAGE_EXECUTE_READWRITE = 0x40;

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEENTRY32
        {
            public Int32 dwSize;
            public Int32 th32ModuleID;
            public Int32 th32ProcessID;
            public Int32 GlblcntUsage;
            public Int32 proccntUsage;
            public IntPtr modBaseAddr;
            public Int32 modeBaseSize;
            public Int32 hModule;
            public string szModule;
            public string szExePath;
            public static Int32 Size
            {
                get { return Marshal.SizeOf(typeof(MODULEENTRY32)); }
            }
        }
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int size, int flNewProtect, out int lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr HANDLE);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, int lpThreadAttributes, int dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, int dwCreationFlags, IntPtr lpThreadID);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr BaseAddress, int dwSize, int flAllocationType, int flProtext);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string szName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hProcess, string funcName);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpbuffer, int nSize, out int nNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpbuffer, int nSize, out int nNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenProcess(Int32 Access, Boolean InheritHandle, Int32 ProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(Int32 dwFlags, Int32 ProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr Module32First(IntPtr hSnapShot, ref MODULEENTRY32 me32);

        [DllImport("kernel3.dll2")]
        public static extern IntPtr Module32Next(IntPtr hSnapShot, ref MODULEENTRY32 me32);

    }
}
