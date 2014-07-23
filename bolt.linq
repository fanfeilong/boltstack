<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="Bin\Release\BOLTStack.dll">D:\dev\code_git\boltstack\Bin\Release\BOLTStack.dll</Reference>
  <Namespace>BOLTStack</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
	var xarSearchPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath),@"Bin");
	var xarName = "View";
	xarSearchPath.Dump();
	BOLTStack.Application.Run(xarSearchPath, xarName, () =>{
		Utils.Register();
		MyClass.Register();
	});
}

[LuaClass(CreatePolicy.Singleton)]
internal sealed class Utils : LuaBaseClass<Utils, Utils>
{
   [LuaClassMethod]
   private static int WriteLine(IntPtr L)
   {
       var instance = GetInstance(L);  
       var str = L.GetString(2);      
       instance.WriteLine(str);        
       return 0;
   }

   [LuaClassMethod]
   private static int Write(IntPtr L)
   {
       var instance = GetInstance(L);
       var str = L.GetString(2);
       instance.Write(str);
       return 0;
   }

   [LuaClassMethod]
   private static int Quit(IntPtr L)
   {
       var instance = GetInstance(L);
       instance.Quit();
       return 0;
   }
   
   internal void WriteLine(string str)
   {
       Console.WriteLine(str);
   }

   internal void Write(string str)
   {
       Console.Write(str);
   }

   internal void Quit()
   {
       Environment.Exit(0);
   }
}

[LuaClass(CreatePolicy.Factory)]
internal sealed class MyClass : LuaBaseClass<MyClass, MyClass>
{
   [LuaClassMethod]
   private static int Add(IntPtr L)
   {
       var instance = GetInstance(L);
       var left = L.GetInt32(2);
       var right = L.GetInt32(3);
       var result = instance.Add(left, right);
       L.PushInt32(result); 
       return 1;
   }

   [LuaClassMethod]
   private static int AttachResultListener(IntPtr L)
   {
       var instance = GetInstance(L);        
       if (!L.IsLuaFunction(-1)) return 0;

       var function = L.ToAction<int>(result =>
       {
           L.PushInt32(result);
           L.Call(1, 0);
       });

       instance.OnAddFinish += function; 
       return 0;
   }
   
   public event Action<int> OnAddFinish;
   public int Add(int lhs, int rhs)
   {
       int result = lhs + rhs;
       if (OnAddFinish != null)
       {
           OnAddFinish(result);
       }
       return result;
   }
}
