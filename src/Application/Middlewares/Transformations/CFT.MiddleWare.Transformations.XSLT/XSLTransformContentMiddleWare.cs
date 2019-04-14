using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace CFT.MiddleWare.Transformations.XSLT
{
    internal class XSLTransformContentMiddleWare : LogMiddlewareBase
    {
        readonly XslCompiledTransform _transform;
        readonly XSLTransformContentOptions _options;

        public XSLTransformContentMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<XSLTransformContentMiddleWare> logger,
            XSLTransformContentOptions options)
            : base(next, logger)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля XSLT преобразования.", e);
            }

            _transform = new XslCompiledTransform();
            try
            {
                _transform.Load(options.XSLTPath);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Что-то пошло не так во время загрузки XSLT файла.", e);
            }
            _options = options;
        }

        protected override Task ExecAsync(CFTFileContext context)
        {
            try
            {
                using (var readStream = new MemoryStream(context.OutputFile.FileContent))
                {
                    using (var xmlReader = XmlReader.Create(readStream))
                    {
                        using (var writeStream = new MemoryStream())
                        {
                            using (var xmlWriter = XmlWriter.Create(
                                writeStream,
                                new XmlWriterSettings()
                                {
                                    // Что бы не было BOM.
                                    Encoding = new UTF8Encoding()
                                }))
                            {
                                _transform.Transform(xmlReader, xmlWriter);
                                context.OutputFile.FileContent = writeStream.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Что-то пошло не так во время XSLT-преобразования файла.", e);
            }

            return Task.CompletedTask;
        }


        protected override string StartMessage => "Начинаем XSLT преобразование.";

        protected override string EndSuccessMessage => "Успешно завершили XSLT преобразование.";

        protected override string EndErrorMessage => "Ошибка XSLT преобразования.";

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            return Task.CompletedTask;
        }
    }
}
