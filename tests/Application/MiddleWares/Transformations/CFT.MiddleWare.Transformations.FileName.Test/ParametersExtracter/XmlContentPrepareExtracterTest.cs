using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.Test.Fixtures;
using CFT.MiddleWare.Transformations.FileName.Test.Mocks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using Xunit;

namespace CFT.MiddleWare.Transformations.FileName.Test.ParametersExtracter
{
    public class XmlContentPrepareExtracterTest : IClassFixture<XMLDataFixture>
    {
        private readonly XMLDataFixture _xmlDataFixture;

        public XmlContentPrepareExtracterTest(XMLDataFixture xmlDataFixture)
        {
            _xmlDataFixture = xmlDataFixture;
        }

        [Fact(DisplayName = "Проверили корректность парсинга.")]
        public void XmlContentPrepareExtracter_Extract_CheckOutputXml()
        {
            var testClass = new XmlContentPrepareExtracter(null);

            var ctx = _xmlDataFixture.GetParameterContext();

            ctx.XmlRootInput.Should().BeNull();
            ctx.XmlRootOutput.Should().BeNull();

            testClass.Extract(ctx);

            _xmlDataFixture.CheckXmlElement(ctx.XmlRootOutput);
            _xmlDataFixture.CheckXmlElement(ctx.XmlRootInput);
        }

        [Fact]
        public void XmlContentPrepareExtracter_Extract_CheckResulDictionary()
        {
            var testClass = new XmlContentPrepareExtracter(null);

            var ctx = _xmlDataFixture.GetParameterContext();

            ctx.XmlRootInput.Should().BeNull();
            ctx.XmlRootOutput.Should().BeNull();

            var result = testClass.Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.BeEmpty();
        }

        [Fact(DisplayName = "Значения из нижележащего Extracter не потерялись")]
        public void XmlContentPrepareExtracter_Extract_CheckCallNextDictionary()
        {
            var ctx = _xmlDataFixture.GetParameterContext();

            var nextMock = new MockParameterExtracterBase(null);

            var testClass = new XmlContentPrepareExtracter(nextMock);


            ctx.XmlRootInput.Should().BeNull();
            ctx.XmlRootOutput.Should().BeNull();

            var result = testClass.Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(3);
            result["FP1"].Should().Be("1");
            result["FP2"].Should().Be("2");
            result["FP3"].Should().Be("3");
        }

    }
}
