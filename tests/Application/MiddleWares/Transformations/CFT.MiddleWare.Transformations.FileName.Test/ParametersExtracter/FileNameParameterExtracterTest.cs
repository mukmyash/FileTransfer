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
    public class FileNameParameterExtracterTest : IClassFixture<XMLDataFixture>
    {
        private readonly XMLDataFixture _xmlDataFixture;

        public FileNameParameterExtracterTest(XMLDataFixture xmlDataFixture)
        {
            _xmlDataFixture = xmlDataFixture;
        }

        [Theory(DisplayName = "Распарсили имя файл")]
        [InlineData("FP", "ab-cd_ef-", new[] { '-' }, new[] { "ab", "cd_ef", "" })]
        [InlineData("FPSD", "ab-cd_ef-", null, new[] { "ab", "cd", "ef", "" })]
        [InlineData("F", "ab/cd-ef|gh", new[] { '/', '-', '|' }, new[] { "ab", "cd", "ef", "gh" })]
        [InlineData("FPBSD", "ab/cd-efgh", new[] { '/', '-', '|' }, new[] { "ab", "cd", "efgh" })]
        [InlineData("FPESD", "ab/cd-ef||gh.xml", new[] { '/', '-', '|' }, new[] { "ab", "cd", "ef", "", "gh.xml" })]
        public void FileNameParameterExtracter_Extract(string paramName, string fileName, char[] separators, string[] resultSet)
        {
            var options = new FileNameParameterDescriptionOption(separators, paramName, ExtractFileType.Input, "");
            var ctx = _xmlDataFixture.GetParameterContext(fileName: fileName);
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var result = new FileNameParameterExtracter(options, null).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(resultSet.Length);
            for (int i = 1; i <= resultSet.Length; i++)
                result[paramName + i].Should().Be(resultSet[i - 1]);
        }

        [Fact(DisplayName = "Подставили дефолтное значение.")]
        public void FileNameParameterExtracter_Extract_DefaulValue()
        {
            var options = new FileNameParameterDescriptionOption(null, "testParam", ExtractFileType.Output, "default value");
            var ctx = _xmlDataFixture.GetParameterContext(fileName: "ab_cd-");
            ctx.XmlRootOutput = _xmlDataFixture.GetXmlElement();
            var result = new FileNameParameterExtracter(options, null).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(3);
            result["testParam3"].Should().Be("default value");
        }

        [Fact(DisplayName = "Значения из нижележащего Extracter не потерялись")]
        public void FileNameParameterExtracter_Extract_CheckCallNextDictionary()
        {
            var options = new FileNameParameterDescriptionOption(new[] { '-' }, "Param", ExtractFileType.Input, "0");
            var ctx = _xmlDataFixture.GetParameterContext(fileName: "ab-cd");
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var nextMock = new MockParameterExtracterBase(null);

            var result = new FileNameParameterExtracter(options, nextMock).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(5);
            result["FP1"].Should().Be("1");
            result["FP2"].Should().Be("2");
            result["FP3"].Should().Be("3");
        }
    }
}
