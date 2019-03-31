using cft.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace cft.Application.FlowStep.Transformation
{
    internal class TransformXSLContentStep : IFlowStep
    {
        readonly string _xsltPath;

        public TransformXSLContentStep(ITransformXSLContentStepOptions options)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля XSLT преобразования.", e);
            }

            _xsltPath = options.XSLTPath;
        }

        public Task RunAsync(FileContext context)
        {
            var transform = new XslCompiledTransform();
            try
            {
                transform.Load(_xsltPath);
            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Что-то пошло не так во время загрузки XSLT файла.", e);
            }

            FileInfo tmpFile = new FileInfo($"{context.FileInfo.FullName}.trunsformed");
            try
            {
                using (var mainStream = tmpFile.Create())
                {
                    using (var wstream = XmlWriter.Create(mainStream))
                    {
                        transform.Transform(context.FileInfo.FullName, wstream);
                    }
                }
                context.FileInfo = tmpFile.CopyTo(context.FileInfo.FullName, true);
            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Что-то пошло не так во время XSLT-преобразования файла.", e);
            }
            finally
            {
                if (tmpFile.Exists)
                    tmpFile.Delete();
            }

            return Task.CompletedTask;
        }
    }
}
