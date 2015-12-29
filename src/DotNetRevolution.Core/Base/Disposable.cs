using System;

namespace DotNetRevolution.Core.Base
{
    public abstract class Disposable : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected abstract void Dispose(bool disposing);
    }
}
