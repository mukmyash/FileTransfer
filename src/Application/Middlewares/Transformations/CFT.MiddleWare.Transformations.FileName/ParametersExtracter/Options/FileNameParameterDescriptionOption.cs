using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal class FileNameParameterDescriptionOption : ParameterDescriptionOptionBase
    {
        public FileNameParameterDescriptionOption(IConfigurationSection configSection)
            : base(configSection)
        {
            Separators = configSection.GetValue<IEnumerable<char>>("Separators")
                ?? new List<char>() { '-', '_' };
        }

        internal override ParameterType ParameterType => ParameterType.FileName;

        public IEnumerable<char> Separators { get; }

    }
}
