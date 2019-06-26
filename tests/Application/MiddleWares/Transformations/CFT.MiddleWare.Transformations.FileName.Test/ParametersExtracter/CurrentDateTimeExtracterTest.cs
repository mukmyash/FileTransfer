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
    public class CurrentDateTimeExtracterTest : IClassFixture<XMLDataFixture>
    {
        private readonly XMLDataFixture _xmlDataFixture;

        public CurrentDateTimeExtracterTest(XMLDataFixture xmlDataFixture)
        {
            _xmlDataFixture = xmlDataFixture;
        }

        public static IEnumerable<object[]> CorrectData()
        {
            yield return new object[] { "curDate", "yyyyMMdd", $"{DateTime.Now.Year}{PadLeft(DateTime.Now.Month)}{PadLeft(DateTime.Now.Day)}" };
            yield return new object[] { "curTime", "HH-mm", $"{PadLeft(DateTime.Now.Hour)}-{PadLeft(DateTime.Now.Minute)}" };
            yield return new object[] { "curTime", "HH_mm", $"{PadLeft(DateTime.Now.Hour)}_{PadLeft(DateTime.Now.Minute)}" };
            yield return new object[] { "curDate", "yyyy_MM-dd", $"{DateTime.Now.Year}_{PadLeft(DateTime.Now.Month)}-{PadLeft(DateTime.Now.Day)}" };
            yield return new object[] { "curDateTime", "yyyy_MM-dd HH_mm", $"{DateTime.Now.Year}_{PadLeft(DateTime.Now.Month)}-{PadLeft(DateTime.Now.Day)} {PadLeft(DateTime.Now.Hour)}_{PadLeft(DateTime.Now.Minute)}" };
        }

        private static string PadLeft(int value)
        {
            return value.ToString().PadLeft(2, '0');
        }

        [Theory(DisplayName = "Распарсили имя файл")]
        [MemberData(nameof(CorrectData))]
        public void CurrentDateTimeExtracter_Extract(string paramName, string format, string expectedResult)
        {
            var options = new CurrentDateTimeDescriptionOptions(format, paramName, ExtractFileType.Input, "");
            var ctx = _xmlDataFixture.GetParameterContext();
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var result = new CurrentDateTimeExtracter(options, null).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(1);
            result[paramName].Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Значения из нижележащего Extracter не потерялись")]
        public void CurrentDateTimeExtracter_Extract_CheckCallNextDictionary()
        {
            var options = new CurrentDateTimeDescriptionOptions("yyyyMMdd", "Param", ExtractFileType.Input, "0");
            var ctx = _xmlDataFixture.GetParameterContext();
            ctx.XmlRootInput = _xmlDataFixture.GetXmlElement();
            var nextMock = new MockParameterExtracterBase(null);

            var result = new CurrentDateTimeExtracter(options, nextMock).Extract(ctx);

            result.Should().BeOfType<Dictionary<string, string>>()
                .And.HaveCount(4);
            result["FP1"].Should().Be("1");
            result["FP2"].Should().Be("2");
            result["FP3"].Should().Be("3");
        }
    }
}
