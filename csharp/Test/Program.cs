using System;
using System.IO;


namespace UserQuery
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var xarSearchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\");
            Console.WriteLine(xarSearchPath);
            var xarName = "View";
            BOLTStack.Application.Run(xarSearchPath, xarName, () =>
            {
                Utils.Register();
                MyClass.Register();
            });
        }
    }
}