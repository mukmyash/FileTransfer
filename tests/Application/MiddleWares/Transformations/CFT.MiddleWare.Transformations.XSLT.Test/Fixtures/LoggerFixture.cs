using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.XSLT.Test.Fixtures
{
    public class LoggerFixture : IDisposable
    {
        public ILogger<T> GetMockLogger<T>()
        {
            return A.Fake<ILogger<T>>();
        }

        public void Dispose()
        {
        }
    }
}
