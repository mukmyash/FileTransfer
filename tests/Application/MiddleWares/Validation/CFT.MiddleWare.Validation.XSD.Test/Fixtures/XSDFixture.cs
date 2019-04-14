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

        public const string FILENAME_XSD_NAMESPACE = "schema_valid.xsd";
        public const string CONTENT_XSD_NAMESPACE =
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

        public const string FILENAME_XSD_NOT_VALID = "schema_not_valid.xsd";
        public const string CONTENT_XSD_NOT_VALID =
    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  
       <xsd:element name='Root'>";

        public const string FILENAME_XSD_ANY_TYPE = "any_type.xsd";
        public const string CONTENT_XSD_ANY_TYPE = @"<?xml version='1.0' encoding='utf-8'?>
<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
  <xsd:element name='Root'>
     <xsd:complexType>
        <xsd:sequence>
            <xsd:any minOccurs='0'  maxOccurs='unbounded' processContents='skip'/>
        </xsd:sequence>
     </xsd:complexType>
  </xsd:element>
</xsd:schema>";

        public string GetFullPath(string fileName)
        {
            switch (fileName)
            {
                case FILENAME_XSD_NAMESPACE:
                case FILENAME_XSD_NOT_VALID:
                case FILENAME_XSD_ANY_TYPE:
                    break;
                default:
                    throw new Exception($"Файл '{fileName}' не создавался.");
            }

            return Path.Combine(PATH, fileName);
        }

        Dictionary<string, string> FilesWithContent = new Dictionary<string, string>
        {
            { FILENAME_XSD_NAMESPACE, CONTENT_XSD_NAMESPACE },
            { FILENAME_XSD_NOT_VALID, CONTENT_XSD_NOT_VALID},
            { FILENAME_XSD_ANY_TYPE, CONTENT_XSD_ANY_TYPE},
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
