using System;

namespace BOLTStack
{
    public static class DBG
    {
        private static readonly object LockInstance = new object();

        public static void Dump()
        {
            lock (LockInstance)
            {
                Console.WriteLine();
            }
        }

        public static void DumpError(string info)
        {
            lock (LockInstance)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(info);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}