﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thong.Net;
namespace ServerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server ser = new Server(10000);
            ser.Start();
            Console.ReadKey();
        }
    }
}
