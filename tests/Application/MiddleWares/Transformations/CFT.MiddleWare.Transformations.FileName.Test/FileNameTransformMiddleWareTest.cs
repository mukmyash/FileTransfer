using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.Test.Fixtures;
using CFT.MiddleWare.Transformations.FileName.Test.Mocks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CFT.MiddleWare.Transformations.FileName.Test
{
    public class FileNameTransformMiddleWareTest : IClassFixture<MockFixture>, IClassFixture<XMLDataFixture>
    {
        private readonly MockFixture _mockFixture;
        private readonly XMLDataFixture _xmlDataFixture;

        public FileNameTransformMiddleWareTest(MockFixture mockFixture, XMLDataFixture xmlDataFixture)
        {
            _mockFixture = mockFixture;
            _xmlDataFixture = xmlDataFixture;
        }

        [Theory(DisplayName = "Успешно переименовали файл.")]
        [InlineData("@{FP1}_@{FP2}.xml", "1_2.xml")]
        [InlineData("@{FP55}_@{FP2}.xml", "@{FP55}_2.xml")]
        [InlineData("Имя_File_@{FP2}.xml", "Имя_File_2.xml")]
        [InlineData("@FP1}_@{FP2.xml", "@FP1}_@{FP2.xml")]
        public async Task FileNameTransformMiddleWare_RenameSuccess(string fileMask, string resultName)
        {
            var testClass = GetTestCalss(fileMask);

            var ctx = _xmlDataFixture.GetContext();

            ctx.InputFile.FileName.Should().Be("XML_DATA_CONTENT.xml");

            await testClass.InvokeAsync(ctx);

            ctx.InputFile.FileName.Should().Be("XML_DATA_CONTENT.xml");
            ctx.OutputFile.FileName.Should().Be(resultName);
        }

        [Fact(DisplayName = "Имя входного файла не изменилось.")]
        public async Task FileNameTransformMiddleWare_InFileName_NotChange()
        {
            var testClass = GetTestCalss();

            var ctx = _xmlDataFixture.GetContext();

            ctx.InputFile.FileName.Should().Be("XML_DATA_CONTENT.xml");

            await testClass.InvokeAsync(ctx);

            ctx.InputFile.FileName.Should().Be("XML_DATA_CONTENT.xml");
        }

        [Fact(DisplayName = "Ошибка конфигурации.")]
        public async Task FileNameTransformMiddleWare_ErrorSettings()
        {
            Action createInstance = () => GetTestCalss(null);
            createInstance.Should().Throw<CFTConfigurationException>();
        }

        private FileNameTransformMiddleWare GetTestCalss(string fileMask = "@{FP1}_@{FP2}.xml")
        {
            return new FileNameTransformMiddleWare(
                next: n => Task.CompletedTask,
                logger: _mockFixture.GetLogger<FileNameTransformMiddleWare>(),
                parameterExtracterFactory: _mockFixture.GetParameterExtracterFactory(),
                options: new FileNameTransformOptions()
                {
                    FileMask = fileMask,
                    ParametersDescription = _mockFixture.GetIConfigurationSection_ParametersDescription()
                }
            );
        }
    }
}
