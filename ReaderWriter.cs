using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcsCpppp
{
    internal class ReaderWriter
    {
        public static void read(string filePath, ref List<int> items, ref int capacity)
        {
            var lines = File.ReadLines(filePath).ToList();
            capacity = int.Parse(lines[0]);
            lines.Remove(lines[0]);
            items = lines[0].Split(' ').Select(s => int.Parse(s)).ToList();
            return;
        }

        public static void write(string filePath, ref List<List<Bin>> answer)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (List<Bin> result in answer)
                {
                    writer.Write(" [");

                    foreach(Bin bin in result)
                    {
                        writer.Write(" [");
                        foreach(int item in bin.getItems())
                        {
                            writer.Write(' ');
                            writer.Write(item);
                        }
                        writer.Write(" ]");
                    }
                    writer.Write(" ]");
                    writer.WriteLine();
                }
            }
            return;
        }
    }
}
