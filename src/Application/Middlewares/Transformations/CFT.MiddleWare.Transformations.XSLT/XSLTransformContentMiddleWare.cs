//using CFT.Application.Abstractions.Exceptions;
//using CFT.MiddleWare.Base;
//using MiddleWare.Abstractions;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Xsl;

//namespace CFT.MiddleWare.Transformations.XSLT
//{
//    internal class XSLTransformContentMiddleWare
//    {
//        XSLTransformContentOptions _options;
//        MiddlewareDelegate<CFTFileContext> _next;

//        public XSLTransformContentMiddleWare(
//            XSLTransformContentOptions options,
//            MiddlewareDelegate<CFTFileContext> next)
//        {
//            try
//            {
//                options.ValidationParams();
//            }
//            catch (Exception e)
//            {
//                throw new CFTConfigurationException("Ошибка при конфигурации модуля XSLT преобразования.", e);
//            }

//            _options = options;
//            _next = next;
//        }

//        public Task InvokeAsync(CFTFileContext context)
//        {
//            var transform = new XslCompiledTransform();
//            try
//            {
//                transform.Load(_options.XSLTPath);
//            }
//            catch (Exception e)
//            {
//                throw new CFTApplicationException("Что-то пошло не так во время загрузки XSLT файла.", e);
//            }

//            FileInfo tmpFile = new FileInfo($"{context.FileInfo.FullName}.trunsformed");
//            try
//            {
//                using (var mainStream = tmpFile.Create())
//                {
//                    using (var wstream = XmlWriter.Create(mainStream))
//                    {
//                        transform.Transform(context.FileInfo.FullName, wstream);
//                    }
//                }
//                context.FileInfo = tmpFile.CopyTo(context.FileInfo.FullName, true);
//            }
//            catch (Exception e)
//            {
//                throw new CFTApplicationException("Что-то пошло не так во время XSLT-преобразования файла.", e);
//            }
//            finally
//            {
//                if (tmpFile.Exists)
//                    tmpFile.Delete();
//            }

//            return _next(context);
//        }
//    }
//}
