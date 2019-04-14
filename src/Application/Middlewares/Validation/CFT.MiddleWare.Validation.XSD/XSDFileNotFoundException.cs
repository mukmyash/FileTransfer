using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Ошибка отсутствия XSD документа.
    /// </summary>
    public class XSDFileNotFoundException : Exception
    {
        /// <summary>
        /// Создает экземпляр класса <see cref="XSDFileNotFoundException"/>
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public XSDFileNotFoundException(string fileName) : base($"Файл '{fileName}' не существует.")
        {
        }
    }
}
