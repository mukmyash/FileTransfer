using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using CFT.MiddleWare.Validation.XSD.Test.Fixtures;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MiddleWare.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CFT.MiddleWare.Validation.XSD.Test
{
    [Collection("XSDCollection")]
    public class ValidateByXSDMiddleWareTest :
        IClassFixture<XSDFixture>,
        IClassFixture<LoggerFixture>,
        IClassFixture<XMLFixture>
    {
        private XMLFixture _xmlFixture;
        private XSDFixture _xsdFixture;
        private LoggerFixture _loggerFixture;
        public ValidateByXSDMiddleWareTest(
            XSDFixture xsdFixture,
            XMLFixture xmlFixture,
            LoggerFixture loggerFixture)
        {
            _xsdFixture = xsdFixture;
            _xmlFixture = xmlFixture;
            _loggerFixture = loggerFixture;
        }

        [Fact(DisplayName = "Успешно проверили по XSD (Namespace).")]
        public async Task InvokeAsync_Success_Namespace()
        {
            var next = GetNextDelegate();

            var testClass = new ValidateByXSDMiddleWare(
                next: next,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_XSD_NAMESPACE)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xmlFixture.GetFakeFileInfo(XMLFixture.XMLType.NAMESPACE));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();

            call.Should().NotThrow();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.That.Matches((ctx) => ctx == context)))
            .MustHaveHappenedOnceExactly();
        }

        [Fact(DisplayName = "Успешно проверили по XSD (Есть ошибка. Namespace).")]
        public void InvokeAsync_Success_Namespace_Error()
        {
            var next = GetNextDelegate();

            var testClass = new ValidateByXSDMiddleWare(
                next: next,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_XSD_NAMESPACE)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xmlFixture.GetFakeFileInfo(XMLFixture.XMLType.SIMPLE));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();
            call.Should().Throw<XSDValidationException>();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.That.Matches((ctx) => ctx == context)))
            .MustNotHaveHappened();
        }

        [Fact(DisplayName = "Успешно проверили по XSD (AnyType).")]
        public async Task InvokeAsync_Success_AnyType()
        {
            var next = GetNextDelegate();

            var testClass = new ValidateByXSDMiddleWare(
                next: next,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_XSD_ANY_TYPE)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xmlFixture.GetFakeFileInfo(XMLFixture.XMLType.SIMPLE));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();

            call.Should().NotThrow();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.That.Matches((ctx) => ctx == context)))
            .MustHaveHappenedOnceExactly();
        }

        [Fact(DisplayName = "Ошибка валидации опций.")]
        public void CreateInstance_ErrorParam()
        {
            var options = A.Fake<IValidateByXSDOptions>();
            A.CallTo(() => options.ValidationParams())
                .Throws<Exception>();

            Action createInstance = () => new ValidateByXSDMiddleWare(
                            next: GetNextDelegate(),
                            logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                            options: options);

            createInstance.Should()
                .Throw<XSDModuleConfigurationException>()
                .And.InnerException.Should().BeOfType<Exception>();
        }

        [Fact(DisplayName = "Ошибка загрузки XSD.")]
        public void CreateInstance_ErrorLoadXSD()
        {
            var options = A.Fake<IValidateByXSDOptions>();
            A.CallTo(() => options.XSDPath)
                .Returns(".\no.xsd");

            Action createInstance = () => new ValidateByXSDMiddleWare(
                            next: GetNextDelegate(),
                            logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                            options: options);

            createInstance.Should()
                .Throw<XSDModuleConfigurationException>()
                .And
                .InnerException.Should().BeOfType<XSDFileFormatException>()
                .Which
                //Хранит ошибку загрузки документа. 
                .InnerException.Should().NotBeNull();
        }

        [Fact(DisplayName = "Не передали следующую мидлвару.")]
        public void CreateInstance_ErrorNotSetNext()
        {
            var options = A.Fake<IValidateByXSDOptions>();
            A.CallTo(() => options.XSDPath)
                .Returns(".\no.xsd");

            Action createInstance = () => new ValidateByXSDMiddleWare(
                            next: null,
                            logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                            options: options);

            createInstance.Should()
                .Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("next");
        }

        private MiddlewareDelegate<CFTFileContext> GetNextDelegate()
        {
            var next = A.Fake<MiddlewareDelegate<CFTFileContext>>();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.Ignored))
                .Returns(Task.CompletedTask);

            return next;
        }
    }
}
