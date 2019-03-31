using cft.Application.Exceptions;
using cft.Application.FlowStep.Transformation;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cft.Application.Tests.FlowStep.Transformation
{
    public class TransformFileNameStepUnitTest
    {
        [Theory(DisplayName = "Не передали маску для преобразования.")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateFlowStep_Mask_NotSet(string mask)
        {
            Action callConstructor = () => new TransformFileNameStep(new TransformFileNameStepOptions() { FileMask = mask });
            callConstructor.Should().Throw<CFTConfigurationException>()
                .Which.InnerException.Should().BeOfType<CFTConfigurationException>();
        }


        [Theory(DisplayName = "Успешно переименовали файл.")]
        [InlineData("BPC_3_4_1.txt", "CPV_@{FP2}_@{FP1}_{3}.hz", "CPV_3_BPC_{3}.hz")]
        [InlineData("BPC_3_4_1.txt", "CPV_@{FP2}_@{FP11}_{3}.hz", "CPV_3_@{FP11}_{3}.hz")]
        public async Task CreateFlowStep_Success(string mainFileName, string mask, string expectedFileName)
        {
            const string content = "<root><data>DDD</data></root>";
            var step = new TransformFileNameStep(new TransformFileNameStepOptions() { FileMask = mask });

            string folder = Path.Combine(".", "change-name", "CreateFlowStep_Success");
            string mainFilePath = Path.Combine(folder, mainFileName);
            string expectedFilePath = Path.Combine(folder, expectedFileName);
            var mainFileInfo = new FileInfo(mainFilePath);
            var expectedFileInfo = new FileInfo(expectedFilePath);

            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (var stream = File.CreateText(mainFilePath))
                {
                    await stream.WriteAsync(content);
                }

                var context = new FileContext(mainFileInfo);
                await step.RunAsync(context);


                File.Exists(expectedFilePath)
                    .Should().BeTrue("Файл не переименован.");
                context.FileInfo.FullName
                    .Should().Be(expectedFileInfo.FullName, "В контексте не изменилась ссылка на файл.");
                mainFileInfo.FullName
                    .Should().Be(expectedFileInfo.FullName, "Изменилась ссылка на FileInfo.");
                File.Exists(mainFilePath)
                    .Should().BeFalse("Исходный файл не удален.");
                File.ReadAllText(expectedFilePath)
                    .Should().Be(content, "Содержимое файла изменилось.");
            }
            finally
            {
                if (File.Exists(expectedFilePath))
                    File.Delete(expectedFilePath);
                if (File.Exists(mainFilePath))
                    File.Delete(mainFilePath);
            }
        }

    }
}
