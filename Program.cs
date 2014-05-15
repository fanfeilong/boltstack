using System;
using System.IO;
using ComicDown.UI.Core.Bolt;


namespace HelloBolt.NET
{
    static class Program
    {
        static XLBolt bolt;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //
            //获取XLBOLT单例对象
            //
            bolt = XLBolt.Instance();

            //
            //BOLT查找路径
            //
            var xarSearchPath = Path.Combine(System.Windows.Forms.Application.StartupPath, @"..\");

            //
            //XAR文件夹或者包的名字
            //
            var xarName = "View";

            //
            //启动XLBOLT
            //
            bolt.Run(xarSearchPath, xarName, () =>
            {
                //
                //请在此处注册C#类或对象给BOLT环境
                //
                LuaApplication.Register();
                LuaMyClass.Register();
            });
        }
    }
}
