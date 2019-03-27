using cft.Application.FlowStep.Validation;
using cft.Application.Options.FlowStep.Validation;
using cft.Application.Tests.Fixtures;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cft.Application.Tests.FlowStep.Validation
{
    public class CheckXSDStepUnitTest : IClassFixture<XSDFixture>
    {
        private XSDFixture _xsdFixture;

        public CheckXSDStepUnitTest(XSDFixture xsdFixture)
        {
            _xsdFixture = xsdFixture;
        }

        [Fact(DisplayName = "Успешно проверили файл.")]
        public async Task RunFlowStep_Success()
        {
            var testClass = new CheckXSDStep(new CheckXSDStepOptions() { XSDPath = _xsdFixture.GetFullPath(XSDFixture.FILENAME_VALID_XSD) });

            var context = new FileContext(new FileInfo(_xsdFixture.GetFullPath(XSDFixture.FILENAME_DATA_VALID_XML)));

            await testClass.RunAsync(context);
        }

    }
}
