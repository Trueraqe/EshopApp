using EshopCore.Database;
using EshopCore.Models;
using EshopCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EshopCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            MenuRenderer mr = new();
            mr.Start();
            Console.ReadKey();
            Console.Beep();
        }
    }
}