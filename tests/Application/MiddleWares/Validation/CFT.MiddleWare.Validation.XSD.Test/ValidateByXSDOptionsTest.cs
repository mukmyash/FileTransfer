using CFT.MiddleWare.Validation.XSD.Test.Fixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CFT.MiddleWare.Validation.XSD.Test
{
    [Collection("XSDCollection")]
    public class ValidateByXSDOptionsTest : IClassFixture<XSDFixture>
    {
        private XSDFixture _xsdFixture;

        public ValidateByXSDOptionsTest(XSDFixture xsdFixture)
        {
            _xsdFixture = xsdFixture ?? throw new ArgumentNullException(nameof(xsdFixture));
        }

        [Theory(DisplayName = "Не указали путь до XSD.")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_XSDPathNotNull(string xsdPath)
        {
            var options = new ValidateByXSDOptions()
            {
                XSDPath = xsdPath
            };

            Action validateMethode = () => options.ValidationParams();

            var throwChecker = validateMethode.Should()
                .Throw<XSDOptionException>();
            throwChecker.And.OptionName.Should().Be(nameof(ValidateByXSDOptions.XSDPath));
            throwChecker.And.InnerException.Should().BeOfType<ArgumentException>()
                .Which.Message.Should().Be("Не указан путь до XSD.");
        }

        [Theory(DisplayName = "Файл должен иметь расширение .xsd")]
        [InlineData("./no.xxsd")]
        [InlineData("./no.sxd")]
        [InlineData("./no.dxs")]
        [InlineData("./xsd.dxs")]
        [InlineData("./xsd")]
        public void Validate_XSDPathExtension(string xsdPath)
        {
            var options = new ValidateByXSDOptions()
            {
                XSDPath = xsdPath
            };

            Action validateMethode = () => options.ValidationParams();

            var throwChecker = validateMethode.Should()
                .Throw<XSDOptionException>();
            throwChecker.And.OptionName.Should().Be(nameof(ValidateByXSDOptions.XSDPath));
            throwChecker.And.InnerException.Should().BeOfType<XSDFileFormatException>()
                .Which.Message.Should().Be($"Файл '{new System.IO.FileInfo(xsdPath).FullName}' не является файлом XSD.");
        }

        [Fact(DisplayName = "Файл xsd существует.")]
        public void Validate_XSDPathExists()
        {
            const string xsdPath = "./schema.xsd";
            var options = new ValidateByXSDOptions()
            {
                XSDPath = xsdPath
            };

            Action validateMethode = () => options.ValidationParams();

            var throwChecker = validateMethode.Should()
                .Throw<XSDOptionException>();
            throwChecker.And.OptionName.Should().Be(nameof(ValidateByXSDOptions.XSDPath));
            throwChecker.And.InnerException.Should().BeOfType<XSDFileNotFoundException>()
                .Which.Message.Should().Be($"Файл '{new System.IO.FileInfo(xsdPath).FullName}' не существует.");
        }

        [Fact(DisplayName = "Успешно прошли все проверки.")]
        public void Validate_AllCheck()
        {
            var options = new ValidateByXSDOptions()
            {
                XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_XSD_NOT_VALID)
            };

            Action validateMethode = () => options.ValidationParams();

            validateMethode.Should()
                .NotThrow();
        }
    }
}
