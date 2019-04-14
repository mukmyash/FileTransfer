using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.Application
{
    public class MapWhenExceptionOptions
    {
        public IEnumerable<StepOptions> Steps { get; set; }

        internal void ValidationParams()
        {
            if (Steps == null || Steps.Count() == 0)
                throw new CFTConfigurationException($"Отсутствуют шаги в случае возникновения ошибки.");
        }
    }
}
