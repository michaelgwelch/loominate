
namespace LoadFile
{
    using System.IO;
    using Loominate.Engine;

    class Program
    {
        static void Main(string[] args)
        {
            GnuCashFile gnc;
            using (FileStream f = File.OpenRead(args[0]))
            {
                gnc = GnuCashFile.ReadXmlStream(f);
            }

            using (FileStream f = File.OpenWrite(args[1]))
            {
                gnc.WriteXmlStream(f);
            }

        }
    }
}
