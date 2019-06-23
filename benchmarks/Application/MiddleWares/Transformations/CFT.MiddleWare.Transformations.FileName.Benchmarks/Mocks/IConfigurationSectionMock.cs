using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Benchmarks.Mocks
{
    class IConfigurationSectionMock : IConfigurationSection
    {
        public string this[string key] { get => null; set { } }

        public string Key => null;

        public string Path => null;

        public string Value { get => null; set { } }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }

        public IChangeToken GetReloadToken()
        {
            return null;
        }

        public IConfigurationSection GetSection(string key)
        {
            return null;
        }
    }
}
