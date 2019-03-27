using cft.Application.Exceptions;
using cft.Application.FlowStep.Transformation;
using cft.Application.Options.FlowStep.Transformation;
using cft.Application.Tests.Fixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cft.Application.Tests.FlowStep.Transformation
{
    public class TransformXSLContentStepUnitTest : IClassFixture<XSLTFixture>
    {
        private XSLTFixture _xsltFixture;

        public TransformXSLContentStepUnitTest(XSLTFixture xsltFixture)
        {
            _xsltFixture = xsltFixture;
        }

        [Theory(DisplayName = "Не передали путь к XSLT файлу.")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateFlowStep_XSLTPath_NotSet(string path)
        {
            Action callConstructor = () => new TransformXSLContentStep(new TransformXSLContentStepOptions() { XSLTPath = path });
            callConstructor.Should().Throw<CFTConfigurationException>()
                .Which.InnerException.Should().BeOfType<CFTConfigurationException>();
        }

        [Fact(DisplayName = "XSLT файл не существует.")]
        public void CreateFlowStep_FileNotFound()
        {
            Action callConstructor = () => new TransformXSLContentStep(
                new TransformXSLContentStepOptions() { XSLTPath = Path.Combine(".", "no.xslt") });
            callConstructor.Should().Throw<CFTConfigurationException>().Which.InnerException.Should().BeOfType<CFTFileNotFoundException>();
        }

        [Fact(DisplayName = "Неверный формат файла XSLT.")]
        public void CreateFlowStep_FileBadFormat()
        {
            Action callConstructor = () => new TransformXSLContentStep(
                new TransformXSLContentStepOptions() { XSLTPath = _xsltFixture.GetFullPath(XSLTFixture.FILENAME_BAD_EXTENSION_TXT) });
            callConstructor.Should().Throw<CFTConfigurationException>().Which.InnerException.Should().BeOfType<CFTFileBadFormatException>();
        }


        [Theory(DisplayName = "Успешно создали XSLT шаг")]
        [InlineData(XSLTFixture.FILENAME_VALID_XSLT)]
        [InlineData(XSLTFixture.FILENAME_VALID_XSL)]
        public void CreateFlowStep_Success(string fileName)
        {
            Action callConstructor = () => new TransformXSLContentStep(new TransformXSLContentStepOptions() { XSLTPath = _xsltFixture.GetFullPath(fileName) });
            callConstructor.Should().NotThrow();
        }

        [Theory(DisplayName = "Успешно преобразовали по XSLT.")]
        [InlineData(XSLTFixture.FILENAME_VALID_XSL)]
        [InlineData(XSLTFixture.FILENAME_VALID_XSLT)]
        public async Task RunFlowStep_Success(string fileName)
        {
            var pathXsl = _xsltFixture.GetFullPath(fileName);
            var mainDataPath = _xsltFixture.GetFullPath(XSLTFixture.FILENAME_DATA_XML);
            var pathDataXml = mainDataPath + XSLTFixture.FILENAME_DATA_XML;

            try
            {
                File.Copy(mainDataPath, pathDataXml);

                var testClass = new TransformXSLContentStep(new TransformXSLContentStepOptions() { XSLTPath = pathXsl });

                var context = new FileContext(new FileInfo(pathDataXml));
                await testClass.RunAsync(context);

                File.ReadAllText(pathDataXml).Should().Be(XSLTFixture.CONTENT_DATA_XML_AFTER_XSLT);
            }
            finally
            {
                if (File.Exists(pathDataXml))
                    File.Delete(pathDataXml);
            }
        }


        [Fact(DisplayName = "Ошибка в XSLT файле.")]
        public void RunFlowStep_BadXSLT()
        {
            var pathXsl = _xsltFixture.GetFullPath(XSLTFixture.FILENAME_NOT_VALID_XSLT);
            var pathDataXml = _xsltFixture.GetFullPath(XSLTFixture.FILENAME_DATA_XML);

            var testClass = new TransformXSLContentStep(new TransformXSLContentStepOptions() { XSLTPath = pathXsl });

            var context = new FileContext(new FileInfo(pathDataXml));

            Action callMethode = () => testClass.RunAsync(context).GetAwaiter().GetResult();

            callMethode.Should().Throw<CFTApplicationException>();
        }
    }
}
