using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using CFT.MiddleWare.Transformations.FileName.Test.Mocks;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Test.Fixtures
{
    public class MockFixture : IDisposable
    {
        internal ILogger<T> GetLogger<T>()
        {
            return A.Fake<ILogger<T>>();
        }

        internal IConfigurationSection GetIConfigurationSection_ParametersDescription()
        {
            return A.Fake<IConfigurationSection>();
        }

        internal IParameterExtracterFactory GetParameterExtracterFactory()
        {
            var parameterExtracterFactoryFake = A.Fake<IParameterExtracterFactory>();
            A.CallTo(
                () => parameterExtracterFactoryFake.GetParameterExtracterFlow(A<IConfigurationSection>.Ignored))
                .Returns(new MockParameterExtracterBase(null));
            A.CallTo(
                () => parameterExtracterFactoryFake.GetParameterExtracterFlow(A<IEnumerable<ParameterDescriptionOptionBase>>.Ignored))
                .Returns(new MockParameterExtracterBase(null));

            return parameterExtracterFactoryFake;
        }

        public void Dispose()
        {
        }
    }
}
