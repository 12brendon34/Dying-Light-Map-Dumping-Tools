using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SO_Dumper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> arguments = new List<string>(args);

            if (arguments.Count < 1 || arguments.Count > 2)
            {
                Console.WriteLine("Usage: {0} input_file.sobj", System.AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                return;
            }

            string inputfile = arguments[0];

            using (var input = File.OpenRead(inputfile))
            {
                string MagicID = Util.ReadString(input, Encoding.ASCII, 4);

                if (MagicID == "SO13")
                {
                    Console.WriteLine($"File: {inputfile}, MagicID: {MagicID} — Simple Object version 13 not supported.");
                }
                if (MagicID == "SO16")
                {
                    Console.WriteLine($"File: {inputfile}, MagicID: {MagicID} — Simple Object version 16 not supported.");
                }
                if (MagicID == "SO18")
                {
                    var offsets = new SObjFileOffsets();
                    offsets.Deserialize(input);

                    var SObjects = new CSimpleObjects();
                    SObjects.Deserialize(input);

                    Console.Write(SObjects.ToString());
                }
            }
        }
    }
}
