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
    public class ValidateByXSDMiddleWareTest : IClassFixture<XSDFixture>, IClassFixture<LoggerFixture>
    {
        private XSDFixture _xsdFixture;
        private LoggerFixture _loggerFixture;
        public ValidateByXSDMiddleWareTest(
            XSDFixture xsdFixture,
            LoggerFixture loggerFixture)
        {
            _xsdFixture = xsdFixture;
            _loggerFixture = loggerFixture;
        }

        [Fact(DisplayName = "Успешно проверили по XSD (Без ошибок).")]
        public async Task InvokeAsync_Success_NotError()
        {
            var testClass = new ValidateByXSDMiddleWare(
                next: ctx => Task.CompletedTask,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_VALID_XSD)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xsdFixture.GetFakeFileInfo(isValid: true));

            await testClass.InvokeAsync(context);
        }

        [Fact(DisplayName = "Успешно проверили по XSD (C ошибокой).")]
        public void InvokeAsync_Success_WithError()
        {
            var testClass = new ValidateByXSDMiddleWare(
                next: ctx => Task.CompletedTask,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    TargetNamespace = "http://NamespaceTest.com/CustomerTypes",
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_VALID_XSD)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xsdFixture.GetFakeFileInfo(isValid: false));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();
            call.Should().Throw<CFTFileXSDValidationException>();
        }
    }
}
