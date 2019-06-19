using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using CFT.MiddleWare.Transformations.FileName.Test.Fixtures;
using CFT.MiddleWare.Transformations.FileName.Test.Mocks;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CFT.MiddleWare.Transformations.FileName.Test.ParametersExtracter
{
    public class XmlContentParameterExtracterTest : IClassFixture<XMLDataFixture>
    {
        private readonly XMLDataFixture _xmlDataFixture;

        public XmlContentParameterExtracterTest(XMLDataFixture xmlDataFixture)
        {
            _xmlDataFixture = xmlDataFixture;
        }

        [Theory]
        [InlineData("xmlAtr", XMLDataFixture.XPATH_ATTRIBUTE, XMLDataFixture.XPATH_ATTRIBUTE_VALUE)]
        [InlineData("xmlEl", XMLDataFixture.XPATH_ELEMENT, XMLDataFixture.XPATH_ELEMENT_VALUE)]
        public void XmlContentParameterExtracter_Extract(string paramName, string xpath, string value)
        {
            var options = new XmlContentParameterDescriptionOption(xpath, paramName, ExtractFileType.Input, "0");
            var ctx = _xmlDataFixture.GetParameterContext();
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var result = new XmlContentParameterExtracter(options, null).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(1);
            result[paramName].Should().Be(value);
        }

        [Fact]
        public void XmlContentParameterExtracter_Extract_DefaulValue()
        {
            var options = new XmlContentParameterDescriptionOption("/test/xml/path", "testParam", ExtractFileType.Output, "default value");
            var ctx = _xmlDataFixture.GetParameterContext();
            ctx.XmlRootOutput= _xmlDataFixture.GetXmlElement();
            var result = new XmlContentParameterExtracter(options, null).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(1);
            result["testParam"].Should().Be("default value");
        }


        [Fact]
        public void XmlContentParameterExtracter_Extract_CheckCallNextDictionary()
        {
            var options = new XmlContentParameterDescriptionOption("/el1/el2/el3", "xmlParam", ExtractFileType.Input, "0");
            var ctx = _xmlDataFixture.GetParameterContext();
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var nextMock = new MockParameterExtracterBase(null);

            var result = new XmlContentParameterExtracter(options, nextMock).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(4);
            result["FP1"].Should().Be("1");
            result["FP2"].Should().Be("2");
            result["FP3"].Should().Be("3");
        }
    }
}
