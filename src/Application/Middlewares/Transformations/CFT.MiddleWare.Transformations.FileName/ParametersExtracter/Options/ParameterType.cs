using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    /// <summary>
    /// Откуда извлекать параметр.
    /// </summary>
    internal enum ParameterType
    {
        /// <summary>
        /// Содержимое XML файла
        /// </summary>
        XMLContent,

        /// <summary>
        /// Имя файла
        /// </summary>
        FileName,

        /// <summary>
        /// Текущая дата и время
        /// </summary>
        CurrentDateTime
    }
}
