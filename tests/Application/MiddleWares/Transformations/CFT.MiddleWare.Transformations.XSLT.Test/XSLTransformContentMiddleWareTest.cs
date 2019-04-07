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
    public class XSLTransformContentMiddleWareTest : IClassFixture<XSLTFixture>
    {
        private XSLTFixture _xsltFixture;

        public XSLTransformContentMiddleWareTest(XSLTFixture xsltFixture)
        {
            _xsltFixture = xsltFixture;
        }

        [Theory(DisplayName = "Успешно преобразовали по XSLT.")]
        [InlineData(XSLTFixture.FILENAME_VALID_XSL)]
        [InlineData(XSLTFixture.FILENAME_VALID_XSLT)]
        public async Task InvokeAsync_Success(string xsltFileName)
        {
            var testClass = new XSLTransformContentMiddleWare(
                options: new XSLTransformContentOptions()
                {
                    XSLTPath = _xsltFixture.GetFullPath(xsltFileName)
                },
                next: ctx => Task.CompletedTask);

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
                options: new XSLTransformContentOptions()
                {
                    XSLTPath = xsltFilePath
                },
                next: ctx => Task.CompletedTask);
            callConstructor.Should().Throw<CFTConfigurationException>()
                .Which.InnerException.Should().BeOfType<CFTConfigurationException>();
        }
    }
}
