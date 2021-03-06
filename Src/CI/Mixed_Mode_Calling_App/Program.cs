using System;
using System.Runtime.InteropServices;

namespace Mixed_Mode_Calling_App
{
    public class Program
    {
        [DllImport(@"C:\g\CI\Src\CI\x64\Debug\Mixed_Mode_Debugging.dll", EntryPoint = "mixed_mode_multiply", CallingConvention = CallingConvention.StdCall)]
        public static extern int Multiply(int x, int y);

        public static void Main(string[] args)
        {
            Console.WriteLine("Hang on ...");
            var result = Multiply(7, 7);
            Console.WriteLine("The answer is {0}", result);
            //Console.ReadKey();
        }
    }
}