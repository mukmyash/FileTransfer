using CFT.MiddleWare.Base;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD.Test.Fixtures
{
    public class XSDFixture
    {
        private readonly string PATH = Path.Combine(".", "xsd");

        public const string FILENAME_DATA_VALID_XML = "data_valid.xml";
        public const string FILENAME_DATA_NOT_VALID_XML = "data_not_valid.xml";
        public const string FILENAME_VALID_XSD = "schema_valid.xsd";
        public const string FILENAME_NOT_VALID_XSD = "schema_not_valid.xsd";

        public const string CONTENT_XSD_VALID =
    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
        xmlns:tns='http://NamespaceTest.com/CustomerTypes'
        targetNamespace='http://NamespaceTest.com/CustomerTypes'>  
       <xsd:element name='Root' type='tns:rootType'/>
        <xsd:complexType name='rootType'>  
         <xsd:sequence>  
          <xsd:element name='Child1' minOccurs='1' maxOccurs='1' type='tns:stringns'/>  
          <xsd:element name='Child2' minOccurs='1' maxOccurs='1' type='tns:stringns'/>  
         </xsd:sequence>  
        </xsd:complexType>
      <xsd:simpleType name='stringns'>
        <xsd:restriction base='xsd:string'/>
      </xsd:simpleType>
      </xsd:schema>";

        public const string CONTENT_XSD_NOT_VALID =
    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  
       <xsd:element name='Root'>  
        <xsd:complexType>  
         <xsd:sequence>  
          <xsd:element name='Child1' minOccurs='1' maxOccurs='1'/>  
          <xsd:element name='Child2' minOccurs='1' maxOccurs='1'/>";

        public const string CONTENT_DATA_XML_VALID =
    @"<ns1:Root xmlns:ns1='http://NamespaceTest.com/CustomerTypes'>
        <Child1>content1</Child1>
        <Child2>content1</Child2>
    </ns1:Root>";
        public const string CONTENT_DATA_XML_NOT_VALID =
    @"<Root xmlns:ns1='http://NamespaceTest.com/CustomerTypes'>
        <Child1>content1</Child1>
        <Child3>content1</Child3>
    </Root>";

        public ICFTInputFileInfo GetFakeFileInfo(bool isValid)
        {
            var fakeFileInfo = A.Fake<ICFTInputFileInfo>();
            A.CallTo(() => fakeFileInfo.FileContent)
                .Returns(Encoding.Default.GetBytes(
                    isValid ? CONTENT_DATA_XML_VALID : CONTENT_DATA_XML_NOT_VALID));
            return fakeFileInfo;
        }

        public string GetFullPath(string fileName)
        {
            switch (fileName)
            {
                case FILENAME_DATA_VALID_XML:
                case FILENAME_DATA_NOT_VALID_XML:
                case FILENAME_VALID_XSD:
                case FILENAME_NOT_VALID_XSD:
                    break;
                default:
                    throw new Exception($"Файл '{fileName}' не создавался.");
            }

            return Path.Combine(PATH, fileName);
        }

        Dictionary<string, string> FilesWithContent = new Dictionary<string, string>
        {
            { FILENAME_VALID_XSD, CONTENT_XSD_VALID },
            { FILENAME_DATA_VALID_XML,  CONTENT_DATA_XML_VALID },
            { FILENAME_DATA_NOT_VALID_XML,  CONTENT_DATA_XML_NOT_VALID },
            { FILENAME_NOT_VALID_XSD, CONTENT_XSD_NOT_VALID},
        };

        public XSDFixture()
        {
            if (!Directory.Exists(PATH))
                Directory.CreateDirectory(PATH);

            foreach (var fileWithContent in FilesWithContent)
            {
                File.WriteAllText(Path.Combine(PATH, fileWithContent.Key), fileWithContent.Value);
            }
        }

        public void Dispose()
        {
            if (Directory.Exists(PATH))
                Directory.Delete(PATH, true);
        }
    }
}
