﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ijw.Entity.Test.ConsoleApp.NET45 {
    class Program {
        static void Main(string[] args) {
            ForTest foo = new ForTest();
            foo.PropertyChanged += Foo_PropertyChanged;
            foo.Name = "jack";
            Console.ReadLine();
        }

        private static void Foo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Console.WriteLine($"Property changed: {e.PropertyName}");
        }
    }
}
