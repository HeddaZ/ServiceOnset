using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace mycmd
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                string s = DateTime.Now.ToString();
                using (StreamWriter w = new StreamWriter(Directory.GetCurrentDirectory() + "\\mycmd.txt", false))
                {
                    w.WriteLine(s);
                }
                Console.WriteLine(s);
                Thread.Sleep(1000);
            }
        }
    }
}
