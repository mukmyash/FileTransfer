using CFT.Application.Abstractions.Exceptions;
using CFT.FileProvider.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CFT.MiddleWare.Base
{
    public class CFTFileContext : ContextBase
    {
        public CFTFileContext(IServiceProvider applicationServices, ICFTInputFileInfo inputFile)
        {
            if (applicationServices == null)
                throw new ArgumentNullException(nameof(applicationServices));
            if (inputFile == null)
                throw new ArgumentNullException(nameof(inputFile));

            ContextServices = applicationServices.CreateScope().ServiceProvider;
            InputFile = inputFile;
            OutputFile = new CFTFileInfo(inputFile);
        }

        public override IServiceProvider ContextServices { get; }
        public ICFTInputFileInfo InputFile { get; }
        public ICFTOutputFileInfo OutputFile { get; }

    }
}
