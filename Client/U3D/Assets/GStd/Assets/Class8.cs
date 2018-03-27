namespace ns0
{
    using System;
    using System.IO;

    internal class Class8
    {
        private FileStream fileStream_0;
        private int int_0;
        private string string_0;
        private string string_1;
        private string string_2;

        internal Class8(string string_3, string string_4)
        {
            this.string_0 = string_3;
            this.string_2 = string_4;
            this.string_1 = string_4 + ".temp";
        }

        internal long method_0()
        {
            return (long) this.int_0;
        }

        internal void method_1(byte[] byte_0, int int_1)
        {
            this.method_4();
            this.fileStream_0.Write(byte_0, 0, int_1);
            this.int_0 += int_1;
        }

        internal void method_2()
        {
            if (this.fileStream_0 != null)
            {
                this.fileStream_0.Flush();
                this.fileStream_0.Close();
                this.fileStream_0 = null;
                if (File.Exists(this.string_2))
                {
                    File.Delete(this.string_2);
                }
                File.Move(this.string_1, this.string_2);
                string directoryName = Path.GetDirectoryName(this.string_2);
                if (Directory.Exists(directoryName))
                {
                    string fileName = Path.GetFileName(this.string_0);
                    string[] files = Directory.GetFiles(directoryName, string.Format("{0}-*", fileName));
                    string fullPath = Path.GetFullPath(this.string_2);
                    foreach (string str4 in files)
                    {
                        if (Path.GetFullPath(str4) != fullPath)
                        {
                            File.Delete(str4);
                        }
                    }
                }
            }
        }

        internal void method_3()
        {
            if (this.fileStream_0 != null)
            {
                this.fileStream_0.Flush();
                this.fileStream_0.Close();
                this.fileStream_0 = null;
                File.Delete(this.string_1);
            }
        }

        private void method_4()
        {
            if (this.fileStream_0 == null)
            {
                string directoryName = Path.GetDirectoryName(this.string_1);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                this.fileStream_0 = new FileStream(this.string_1, FileMode.Create, FileAccess.Write);
            }
        }
    }
}

