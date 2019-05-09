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

        [Fact(DisplayName = "������� ��������� �� XSD (DTD, Namespace).")]
        public async Task InvokeAsync_Success_DTD_Namespace()
        {
            var next = GetNextDelegate(isThrow: false);

            var testClass = new ValidateByXSDMiddleWare(
                next: next,
                logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                options: new ValidateByXSDOptions()
                {
                    XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_XSD_NAMESPACE)
                });

            var context = new CFTFileContext(
                applicationServices: new ServiceCollection().BuildServiceProvider(),
                inputFile: _xmlFixture.GetFakeFileInfo(XMLFixture.XMLType.DTD_NAMESPACE));

            Action call = () => testClass.InvokeAsync(context).GetAwaiter().GetResult();

            call.Should().NotThrow();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.That.Matches((ctx) => ctx == context)))
            .MustHaveHappenedOnceExactly();
        }

        [Fact(DisplayName = "������� ��������� �� XSD (Namespace).")]
        public async Task InvokeAsync_Success_Namespace()
        {
            var next = GetNextDelegate(isThrow: false);

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

        [Fact(DisplayName = "������� ��������� �� XSD (���� ������. Namespace).")]
        public void InvokeAsync_Success_Namespace_Error()
        {
            var next = GetNextDelegate(isThrow: false);

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

        [Fact(DisplayName = "���������� ���������� �� next ������.")]
        public void InvokeAsync_Next_Error()
        {
            var next = GetNextDelegate(isThrow: true);

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

            call.Should().Throw<FakeException>();
            A.CallTo(() => next.Invoke(A<CFTFileContext>.That.Matches((ctx) => ctx == context)))
            .MustHaveHappenedOnceExactly();
        }

        [Fact(DisplayName = "������� ��������� �� XSD (AnyType).")]
        public async Task InvokeAsync_Success_AnyType()
        {
            var next = GetNextDelegate(isThrow: false);

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

        [Fact(DisplayName = "������ ��������� �����.")]
        public void CreateInstance_ErrorParam()
        {
            var options = A.Fake<IValidateByXSDOptions>();
            A.CallTo(() => options.ValidationParams())
                .Throws<Exception>();

            Action createInstance = () => new ValidateByXSDMiddleWare(
                            next: GetNextDelegate(isThrow: false),
                            logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                            options: options);

            createInstance.Should()
                .Throw<XSDModuleConfigurationException>()
                .And.InnerException.Should().BeOfType<Exception>();
        }

        [Fact(DisplayName = "������ �������� XSD.")]
        public void CreateInstance_ErrorLoadXSD()
        {
            var options = A.Fake<IValidateByXSDOptions>();
            A.CallTo(() => options.XSDPath)
                .Returns(".\no.xsd");

            Action createInstance = () => new ValidateByXSDMiddleWare(
                            next: GetNextDelegate(isThrow: false),
                            logger: _loggerFixture.GetMockLogger<ValidateByXSDMiddleWare>(),
                            options: options);

            createInstance.Should()
                .Throw<XSDModuleConfigurationException>()
                .And
                .InnerException.Should().BeOfType<XSDFileFormatException>()
                .Which
                //������ ������ �������� ���������. 
                .InnerException.Should().NotBeNull();
        }

        [Fact(DisplayName = "�� �������� ��������� ��������.")]
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

        private MiddlewareDelegate<CFTFileContext> GetNextDelegate(bool isThrow)
        {
            var next = A.Fake<MiddlewareDelegate<CFTFileContext>>();
            var NextInvoke = A.CallTo(() => next.Invoke(A<CFTFileContext>.Ignored));

            if (isThrow)
                NextInvoke.Throws<FakeException>();
            else
                NextInvoke.Returns(Task.CompletedTask);

            return next;
        }
    }
}
