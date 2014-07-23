using System;

namespace BOLTStack
{
    public abstract class LuaBase : IDisposable
    {
        private bool _disposed;
        ~LuaBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposeManagedResources)
        {
            if (_disposed) return;
            if (disposeManagedResources)
            {
                OnDisposeManagedResources();
            }
            OnDisposeUnmangedResources();
            _disposed = true;
        }

        protected virtual void OnDisposeUnmangedResources()
        {

        }

        protected virtual void OnDisposeManagedResources()
        {

        }
    }
}
