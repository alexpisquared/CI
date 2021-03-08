using System;
using System.Windows;

namespace AsyncSocketTester
{
    public partial class App : Application
    {
        public static readonly DateTime Started;

        static App() => Started = DateTime.Now;
    }
}
