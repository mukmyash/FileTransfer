using cft.Application.Exceptions;
using cft.Application.FlowStep.Transformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep.Transformation
{
    public class TransformFileNameStepOptions : ITransformFileNameStepOptions
    {
        public string FileMask { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(FileMask))
                throw new CFTConfigurationException("Не указана маска для преобразования.");
        }
    }
}
