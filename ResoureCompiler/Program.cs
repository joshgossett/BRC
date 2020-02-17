using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResoureCompiler
{
    class Program
    {
        /// <summary>
        /// format = brc input_file output_file var_name
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Format = brc input_filename output_filename variable_name");
                return;
            }
            try
            {
                using (FileStream fs = new FileStream(args[0], FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        using (FileStream ofs = new FileStream(args[1], FileMode.Create))
                        {
                            StringBuilder sb = new StringBuilder($"#pragma once\nchar {args[2]}[] = {{");
                            byte[] data = br.ReadBytes((int)fs.Length);
                            for (int i = 0; i < data.Length - 1; i++)
                            {
                                if (i % 16 == 0)
                                    sb.Append("\n");
                                sb.AppendFormat("0x{0:X2}", data[i]);
                                sb.Append(',');
                            }
                            if ((data.Length - 1) % 16 == 0)
                                sb.Append("\n");
                            sb.AppendFormat("0x{0:X2}", data[data.Length - 1]);
                            sb.Append("\n};");
                            byte[] s = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                            ofs.Write(s, 0, s.Length);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error compiling resource: {ex.Message}");
            }
        }
    }
}
