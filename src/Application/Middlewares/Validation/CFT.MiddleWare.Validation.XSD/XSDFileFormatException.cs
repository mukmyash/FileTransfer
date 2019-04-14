using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Ошибка формата XSD документа.
    /// </summary>
    public class XSDFileFormatException : Exception
    {
        /// <summary>
        /// Создает экземпляр класса <see cref="XSDFileFormatException"/>
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public XSDFileFormatException(string fileName)
            : this(fileName, null)
        {
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDFileFormatException"/>
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public XSDFileFormatException(string fileName, Exception innerException)
            : base($"Файл '{fileName}' не является файлом XSD.", innerException)
        {
        }
    }
}
