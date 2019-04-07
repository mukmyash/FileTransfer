using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using CFT.MiddleWare.Validation.XSD.Test.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CFT.MiddleWare.Validation.XSD.Test
{
    public class ValidateByXSDMiddleWareTest : IClassFixture<XSDFixture>
    {
        private XSDFixture _xsdFixture;

        public ValidateByXSDMiddleWareTest(XSDFixture xsdFixture)
        {
            _xsdFixture = xsdFixture;
        }

        [Fact(DisplayName = "Успешно проверили по XSD (Без ошибок).")]
        public async Task InvokeAsync_Success_NotError()
        {
            var testClass = new ValidateByXSDMiddleWare(
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_VALID_XSD)
                },
                next: ctx => Task.CompletedTask);

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xsdFixture.GetFakeFileInfo(isValid: true));

            await testClass.InvokeAsync(context);
        }

        [Fact(DisplayName = "Успешно проверили по XSD (C ошибокой).")]
        public async Task InvokeAsync_Success_WithError()
        {
            var testClass = new ValidateByXSDMiddleWare(
                options: new ValidateByXSDOptions()
                {
                    TargetNamespace = "http://NamespaceTest.com/CustomerTypes",
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_VALID_XSD)
                },
                next: ctx => Task.CompletedTask);

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xsdFixture.GetFakeFileInfo(isValid: false));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();
            call.Should().Throw<CFTFileXSDValidationException>();
        }
    }
}
