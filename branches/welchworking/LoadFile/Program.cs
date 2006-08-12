
namespace LoadFile
{
    using System.IO;
    using Loominate.Engine;

    class Program
    {
        static void Main(string[] args)
        {

            using (FileStream f = File.OpenRead(args[0]))
            {
                GnuCashFile gnc = GnuCashFile.ReadXmlStream(f);
            }

        }
    }
}
