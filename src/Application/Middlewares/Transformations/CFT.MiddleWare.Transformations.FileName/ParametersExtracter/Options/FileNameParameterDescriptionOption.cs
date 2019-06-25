using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal class FileNameParameterDescriptionOption : ParameterDescriptionOptionBase
    {
        public FileNameParameterDescriptionOption(IConfigurationSection configSection)
            : base(configSection)
        {
            Init(configSection.GetValue<IEnumerable<char>>("Separators"));
        }

        public FileNameParameterDescriptionOption(
            IEnumerable<char> separators,
            string parameterName,
            ExtractFileType fileType,
            string defaultValue)
            : base(parameterName, fileType, defaultValue)
        {
            Init(separators);
        }

        private void Init(IEnumerable<char> separators)
        {
            Separators = separators == null || !separators.Any() ?
                new List<char>() { '-', '_' }
                : separators;
        }

        internal override ParameterType ParameterType => ParameterType.FileName;

        public IEnumerable<char> Separators { get; private set; }

    }
}
