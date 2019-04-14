using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.Application
{
    public class MapWhenFileNameOptions
    {
        public string Mask { get; set; }
        public IEnumerable<StepOptions> Steps { get; set; }

        internal void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(Mask))
                throw new CFTConfigurationException("Не указана маска файла.");

            if (Steps == null || Steps.Count() == 0)
                throw new CFTConfigurationException($"Отсутствуют шаги для маски файла '{Mask}'.");
        }
    }
}
