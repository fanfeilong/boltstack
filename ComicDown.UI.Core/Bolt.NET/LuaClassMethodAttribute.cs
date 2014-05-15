using System;

namespace ComicDown.UI.Core.Bolt
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class LuaClassMethodAttribute : System.Attribute
    {
        public string Name { get; private set; }
        public int Permission { get; private set; }
        public bool DeleteOld { get; private set; }
        public bool HasName { get; private set; }

        public LuaClassMethodAttribute(string name, int permission, bool deleteOld)
        {
            Name = name;
            Permission = permission;
            DeleteOld = deleteOld;
            HasName = !string.IsNullOrEmpty(name);
        }
        public LuaClassMethodAttribute(string name)
        {
            Name = name;
            Permission = 0;
            DeleteOld = false;
            HasName = !string.IsNullOrEmpty(name);
        }
        public LuaClassMethodAttribute()
        {
            Permission = 0;
            DeleteOld = false;
            HasName = false;
        }
    }
}