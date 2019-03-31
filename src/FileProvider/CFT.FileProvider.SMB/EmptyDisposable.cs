using System;

namespace CFT.FileProvider.SMB
{
    internal class EmptyDisposable : IDisposable
    {
        internal static EmptyDisposable Instance = new EmptyDisposable();

        public EmptyDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
}