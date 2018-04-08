using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MemoryManager
{
    class FileStructure
    {
        byte[] Dos_Header = new byte[0x40];
        byte[] NT_Header = new byte[0xF8];
        byte[] byte4 = new byte[0x4];
        byte[] byte2 = new byte[0x2];
        byte[] Dos_stub;
        byte[] section;
        ArrayList sectionHeader = new ArrayList();
        FileStream fs;
        DosHeader dosHeader;
        NtHeader ntHeader;
        int nt;
        int NumberOfSection;

        public ArrayList sectionheader
        {
            get { return this.sectionHeader; }
        }
        public DosHeader dosheader
        {
            get { return this.dosHeader; }
        }
        public NtHeader ntheader
        {
            get { return this.ntHeader; }
        }

        public FileStructure(string path)
        {
            fs = new FileStream(path, FileMode.Open);
            fs.Read(Dos_Header, 0x0, 0x40);
            dosHeader = new DosHeader(Dos_Header);
            Array.Copy(Dos_Header, 0x3C, byte4, 0, 4);
            nt = BitConverter.ToInt32(byte4, 0);
            int tmp = nt - 0x40;
            this.Dos_stub = new byte[tmp];
            fs.Read(this.Dos_stub, 0, tmp);
            fs.Read(this.NT_Header, 0, 0xF8);
            ntHeader = new NtHeader(this.NT_Header);
            Array.Copy(this.NT_Header, 6, byte2, 0, 2);
            NumberOfSection = BitConverter.ToInt16(byte2, 0);
            for(int i=0; i<NumberOfSection; i++)
            {
                this.section = new byte[0x28];
                fs.Read(this.section, 0, 0x28);
                sectionHeader.Add(new SectionHeader(this.section));
            }

            fs.Close();
        }

        public int print()
        {
            return this.nt;
        }
    }

    class DosHeader
    {
        string signiture1;
        int e_lfanew1;

        public string signiture
        {
            set { this.signiture1 = value; }
            get { return this.signiture1; }
        }

        public int e_lfanew
        {
            set { this.e_lfanew1 = value; }
            get { return this.e_lfanew1; }
        }

        public DosHeader(byte[] item)
        {
            byte[] byte2 = new byte[2];
            Array.Copy(item, 0, byte2, 0, 2);
            e_lfanew = 123;
            this.signiture = Encoding.Default.GetString(byte2);
            byte[] byte4 = new byte[4];
            Array.Copy(item, 0x3C, byte4, 0, 4);
            this.e_lfanew = BitConverter.ToInt32(byte4, 0);
        }
    }

    class NtHeader
    {
        FileHeader fileHeader;
        OptionalHeader optionalHeader;
        string signiture1;
        public string signiture
        {
            get { return this.signiture1; }
            set { this.signiture1 = value; }
        }

        public OptionalHeader optionalheader
        {
            get { return this.optionalHeader; }
        }

        public FileHeader fileheader
        {
            get { return this.fileHeader; }
        }

        public NtHeader(byte[] item)
        {
            signiture = string.Format("{0}{1}",Convert.ToChar(item[0]), Convert.ToChar(item[1]));
            byte[] arry;

            arry = new byte[0x14];
            Array.Copy(item, 0x4, arry, 0, 0x14);
            fileHeader = new FileHeader(arry);
            int size = fileHeader.SizeOfOptionalHeader;

            arry = new byte[size];
            Array.Copy(item, 0x18, arry, 0, size);
            optionalHeader = new OptionalHeader(arry);
        }

    }

    class FileHeader
    {
        int Machine1;
        int NumberOfSection1;
        int TimeDataStamp1;
        int SizeOfOptionalHeader1;
        int Characteristics1;

        public int Machine
        {
            get { return this.Machine1; }
            set { this.Machine1 = value; }
        }

        public int NumberOfSection
        {
            get { return this.NumberOfSection1; }
            set { this.NumberOfSection1 = value; }
        }

        public int TimeDataStamp
        {
            get { return this.TimeDataStamp1; }
            set { this.TimeDataStamp1 = value; }
        }

        public int SizeOfOptionalHeader
        {
            get { return this.SizeOfOptionalHeader1; }
            set { this.SizeOfOptionalHeader1 = value; }
        }

        public int Characteristics
        {
            get { return this.Characteristics1; }
            set { this.Characteristics1 = value; }
        }
        

        public FileHeader(byte[] item)
        {
            byte[] tmp = new byte[4];
            Array.Copy(item, 0, tmp, 0, 4);
            this.Machine = BitConverter.ToInt16(item, 0);
            this.NumberOfSection = BitConverter.ToInt16(item, 2);
            Array.Copy(item, 4, tmp, 0, 4);
            this.TimeDataStamp = BitConverter.ToInt32(item, 4);
            Array.Copy(item, 0x10, tmp, 0, 4);
            this.SizeOfOptionalHeader = BitConverter.ToInt16(item, 16);
            this.Characteristics = BitConverter.ToInt16(item, 18);
        }
    }

    class OptionalHeader
    {
        
        int Magic1;
        int LinkerVersion1;
        int SizeOfCode1;
        int SizeOfInitializedData1;
        int SizeOfUninitializedData1;
        int AddressOfEntryPoint1;
        int BaseOfCode1;
        int BaseOfData1;
        int ImageBase1;
        int SectionAlignment1;
        int FileAlignment1;
        int OsVersion1;
        int ImageVersion1;
        int SubsystemVersion1;
        int Win32Version1;
        int SizeOfImage1;
        int SizeOfHeaders1;
        int Checksum1;
        int Subsystem1;
        int DLLCharacteristics1;
        int SizeOfStackReserve1;
        int SizeOfStackCommit1;
        int SizeOfHeapReserve1;
        int SizeOfHeapCommit1;
        int LoaderFlag1;
        int NumberOfDataDirectories1;
        ArrayList DataDirectories1;
        string[] DDname = new string[] { "EXPORT", "IMPORT", "RESOURCE", "EXCEPTION", "CERTIFICATE", "BASE REROCATION", "DEBUG", "Architecture", "GLOBAL POINTER", "TLS", "LOAD CONFIGURATION", "BOUND IMPORT", "IMPORT ADDRESS", "DELAY IMPORT", "CLI", "", "", "", "", "" };

        public int Magic { get { return Magic1; } }
        public int LinkerVersion { get { return LinkerVersion1; } }
        public int SizeOfCode { get { return SizeOfCode1; } }
        public int SizeOfInitializedData { get { return SizeOfInitializedData1; } }
        public int SizeOfUninitializedData { get { return SizeOfUninitializedData1; } }
        public int AddressOfEntryPoint { get { return AddressOfEntryPoint1; } }
        public int BaseOfCode { get { return BaseOfCode1; } }
        public int BaseOfData { get { return BaseOfData1; } }
        public int ImageBase { get { return ImageBase1; } }
        public int SectionAlignment { get { return SectionAlignment1; } }
        public int FileAlignment { get { return FileAlignment1; } }
        public int OsVersion { get { return OsVersion1; } }
        public int ImageVersion { get { return ImageVersion1; } }
        public int SubsystemVersion { get { return Subsystem1; } }
        public int Win32Version { get { return Win32Version1; } }
        public int SizeOfImage { get { return SizeOfImage1; } }
        public int SizeOfHeaders { get { return SizeOfHeaders1; } }
        public int Checksum { get { return Checksum1; } }
        public int Subsystem { get { return Subsystem1; } }
        public int DLLCharacteristics { get { return DLLCharacteristics1; } }
        public int SizeOfStackReserve { get { return SizeOfStackReserve1; } }
        public int SizeOfStackCommit { get { return SizeOfStackCommit1; } }
        public int SizeOfHeapReserve { get { return SizeOfHeapReserve1; } }
        public int SizeOfHeapCommit { get { return SizeOfHeapCommit1; } }
        public int LoaderFlag { get { return LoaderFlag1; } }
        public int NumberOfDataDirectories { get { return NumberOfDataDirectories1; } }
        public ArrayList DataDirectories { get { return DataDirectories1; } }


        public OptionalHeader(byte[] item)
        {
            DataDirectories1 = new ArrayList();
            this.Magic1 = BitConverter.ToUInt16(item, 0);
            this.LinkerVersion1 = BitConverter.ToUInt16(item, 2);
            this.SizeOfCode1 = BitConverter.ToInt32(item, 4);
            this.SizeOfInitializedData1 = BitConverter.ToInt32(item, 8);
            this.SizeOfUninitializedData1 = BitConverter.ToInt32(item, 12);
            this.AddressOfEntryPoint1 = BitConverter.ToInt32(item, 16);
            this.BaseOfCode1 = BitConverter.ToInt32(item, 20);
            this.BaseOfData1 = BitConverter.ToInt32(item, 24);
            this.ImageBase1 = BitConverter.ToInt32(item, 28);
            this.SectionAlignment1 = BitConverter.ToInt32(item, 32);
            this.FileAlignment1 = BitConverter.ToInt32(item, 36);
            this.OsVersion1 = BitConverter.ToInt32(item, 40);
            this.ImageVersion1 = BitConverter.ToInt32(item, 44);
            this.SubsystemVersion1 = BitConverter.ToInt32(item, 48);
            this.Win32Version1 = BitConverter.ToInt32(item, 52);
            this.SizeOfImage1 = BitConverter.ToInt32(item, 56);
            this.SizeOfHeaders1 = BitConverter.ToInt32(item, 60);
            this.Checksum1 = BitConverter.ToInt32(item, 64);
            this.Subsystem1 = BitConverter.ToInt16(item, 68);
            this.DLLCharacteristics1 = BitConverter.ToInt16(item, 70);
            this.SizeOfStackReserve1 = BitConverter.ToInt32(item, 72);
            this.SizeOfStackCommit1 = BitConverter.ToInt32(item, 76);
            this.SizeOfHeapReserve1 = BitConverter.ToInt32(item, 80);
            this.SizeOfHeapCommit1 = BitConverter.ToInt32(item, 84);
            this.LoaderFlag1 = BitConverter.ToInt32(item, 88);
            this.NumberOfDataDirectories1 = BitConverter.ToInt32(item, 92);

            for(int i=0;i<this.NumberOfDataDirectories1; i++)
            {
                DataDirectories1.Add(new DataDirectory(DDname[i], BitConverter.ToInt32(item, 96+i*8), BitConverter.ToInt32(item, 100+i*8)));
            }

            
        }
    }

    class DataDirectory
    {
        string desc1;
        int RVA1;
        int Size1;

        public string desc
        {
            get { return this.desc1; }
            set { this.desc1 = value; }
        }

        public int RVA
        {
            get { return this.RVA1; }
            set { this.RVA1 = value; }
        }

        public int Size
        {
            get { return this.Size1; }
            set { this.Size1 = value; }
        }


        public DataDirectory(string Descript, int RVA, int Size)
        {
            this.desc1 = Descript;
            this.RVA1 = RVA;
            this.Size1 = Size;
        }
    }

    class SectionHeader
    {
        string Name1;
        int VirtualSize1;
        int RVA1;
        int SizeOfRawData1;
        int PointerToRawData1;
        int Characteristics1;

        public string Name
        {
            get { return this.Name1; }
            set { this.Name1 = value; }
        }

        public int VirtualSize
        {
            get { return this.VirtualSize1; }
            set { this.VirtualSize1 = value; }
        }

        public int RVA
        {
            get { return this.RVA1; }
            set { this.RVA1 = value; }
        }

        public int SizeOfRawData
        {
            get { return this.SizeOfRawData1; }
            set { this.SizeOfRawData1 = value; }
        }

        public int PointerToRawData
        {
            get { return this.PointerToRawData1; }
            set { this.PointerToRawData1 = value; }
        }

        public int Characteristics
        {
            get { return this.Characteristics1; }
            set { this.Characteristics1 = value; }
        }

        public SectionHeader(byte[] item)
        {
            byte[] tmp = new byte[8];
            Array.Copy(item, 0, tmp, 0, 8);
            this.Name1 = Encoding.Default.GetString(tmp);
            tmp = new byte[4];
            Array.Copy(item, 8, tmp, 0, 4);
            this.VirtualSize1 = BitConverter.ToInt32(tmp, 0);

            Array.Copy(item, 12, tmp, 0, 4);
            this.RVA1 = BitConverter.ToInt32(tmp, 0);

            Array.Copy(item, 16, tmp, 0, 4);
            this.SizeOfRawData1 = BitConverter.ToInt32(tmp, 0);

            Array.Copy(item, 20, tmp, 0, 4);
            this.PointerToRawData1 = BitConverter.ToInt32(tmp, 0);

            Array.Copy(item, 36, tmp, 0, 4);
            this.Characteristics1 = BitConverter.ToInt32(tmp, 0);
        }
    }
}
