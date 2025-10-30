using System;
using Zenject;

namespace DoubleDCore.Jurisdiction.Base
{
    public interface IBuild : IInstaller, IDisposable
    {
        public void Run();
    }
}