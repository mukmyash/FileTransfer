using Microsoft.Extensions.DependencyInjection;
using MiddleWare.Abstractions;
using System;

namespace CFT.MiddleWare.Base
{
    public class CFTFileContext : ContextBase
    {
        public CFTFileContext(IServiceProvider applicationServices, ICFTInputFileInfo inputFile)
        {
            if (applicationServices == null)
                throw new ArgumentNullException(nameof(applicationServices));

            ContextServices = applicationServices.CreateScope().ServiceProvider;
            InputFile = inputFile ?? throw new ArgumentNullException(nameof(inputFile));
            OutputFile = new CFTFileInfo(inputFile);
        }

        public override IServiceProvider ContextServices { get; }
        public ICFTInputFileInfo InputFile { get; }
        public ICFTOutputFileInfo OutputFile { get; }

    }
}
