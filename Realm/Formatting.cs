﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Realm
{
    public static class Formatting
    {
        public static void type(string src, int speed)
        {
            Console.WriteLine("\r\n");
            foreach (char c in src)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
        }

        public static void type(string src)
        {
            Console.WriteLine("\r\n");
            foreach (char c in src)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }
        }

        public static void drawmap()
        {
            Formatting.type("_____________________________________________________________", 1);
            Formatting.type("I--WESTERN--I-NORTHERN--I-----------I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I-----------I-----------I-----------I", 1);
            Formatting.type("I--KINGDOM--I--KINGDOM--I-----------I-----------I-----------I", 1);
            Formatting.type("I___________________________________________________________I", 1);
            Formatting.type("I-----------I-----------I-----------I--NORTHERN-I-----------I", 1);
            Formatting.type("I-----------I-RIVERWELL-I-----------I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I-----------I-MOUNTAINS-I-----------I", 1);
            Formatting.type("I___________________________________________________________I", 1);
            Formatting.type("I-ILLUSION--I-----------I-----------I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I-----------I--NEWPORT--I-----------I", 1);
            Formatting.type("I--FOREST---I-----------I-----------I-----------I-----------I", 1);
            Formatting.type("I___________________________________________________________I", 1);
            Formatting.type("I-----------I-----------I--CENTRAL--I-----------I--EASTERN--I", 1);
            Formatting.type("I-----------I-VALLEYBURGI-----------I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I--KINGDOM--I-----------I--KINGDOM--I", 1);
            Formatting.type("I___________________________________________________________I", 1);
            Formatting.type("I-----------I-----------I-----------I---TWIN----I-----------I", 1);
            Formatting.type("I--SEAPORT--I-----------I--NOMADS---I-----------I-RAVENKEEP-I", 1);
            Formatting.type("I-----------I-----------I-----------I---PATHS---I-----------I", 1);
            Formatting.type("I___________________________________________________________I", 1);
            Formatting.type("I-----------I-----------I-SOUTHERN--I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I-----------I-----------I-----------I", 1);
            Formatting.type("I-----------I-----------I--KINGDOM--I-----------I-----------I", 1);
            Formatting.type("_____________________________________________________________", 1);
        }
    }
}
