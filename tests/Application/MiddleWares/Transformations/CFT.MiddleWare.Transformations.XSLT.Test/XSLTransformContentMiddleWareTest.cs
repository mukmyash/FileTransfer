using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using CFT.MiddleWare.Transformations.XSLT.Test.Fixtures;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MiddleWare.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CFT.MiddleWare.Transformations.XSLT.Test
{
    public class XSLTransformContentMiddleWareTest : IClassFixture<XSLTFixture>, IClassFixture<LoggerFixture>
    {
        private XSLTFixture _xsltFixture;
        private LoggerFixture _loggerFixture;

        public XSLTransformContentMiddleWareTest(
            XSLTFixture xsltFixture,
            LoggerFixture loggerFixture)
        {
            _xsltFixture = xsltFixture;
            _loggerFixture = loggerFixture;
        }

        [Theory(DisplayName = "Успешно преобразовали по XSLT.")]
        [InlineData(XSLTFixture.FILENAME_VALID_XSL)]
        [InlineData(XSLTFixture.FILENAME_VALID_XSLT)]
        public async Task InvokeAsync_Success(string xsltFileName)
        {
            var testClass = new XSLTransformContentMiddleWare(
                next: ctx => Task.CompletedTask,
                logger: _loggerFixture.GetMockLogger<XSLTransformContentMiddleWare>(),
                options: new XSLTransformContentOptions()
                {
                    XSLTPath = _xsltFixture.GetFullPath(xsltFileName)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xsltFixture.GetFakeFileInfo());

            await testClass.InvokeAsync(context);

            Encoding.Default.GetString(context.OutputFile.FileContent)
                .Should()
                .Be(XSLTFixture.CONTENT_DATA_XML_AFTER_XSLT);
        }

        [Theory(DisplayName = "Не передали путь к XSLT файлу.")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateFlowStep_XSLTPath_NotSet(string xsltFilePath)
        {
            Action callConstructor = () => new XSLTransformContentMiddleWare(
                next: ctx => Task.CompletedTask,
                logger: _loggerFixture.GetMockLogger<XSLTransformContentMiddleWare>(),
                options: new XSLTransformContentOptions()
                {
                    XSLTPath = xsltFilePath
                });
            callConstructor.Should().Throw<CFTConfigurationException>()
                .Which.InnerException.Should().BeOfType<CFTConfigurationException>();
        }
    }
}
