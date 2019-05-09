using CFT.MiddleWare.Base;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD.Test.Fixtures
{
    public class XMLFixture : IDisposable
    {
        private readonly string PATH = Path.Combine(".", "xml");
        public enum XMLType
        {
            SIMPLE,
            NAMESPACE,
            ATTRIBUTES,
            DTD_NAMESPACE
        }

        public const string FILENAME_XML = "simple.xml";
        public const string CONTENT_XML =
    @"<Root>
        <Child1>content1</Child1>
        <Child3>content1</Child3>
    </Root>";

        public const string FILENAME_XML_ATTRIBUTES = "attributes.xml";
        public const string CONTENT_XML_ATTRIBUTES =
    @"<Root Attr1='testVal'>
        <Child1 InnerAttr1='TestVal2'>content1</Child1>
        <Child3>content1</Child3>
    </Root>";

        public const string FILENAME_XML_NAMESPACE = "namespace.xml";
        public const string CONTENT_XML_NAMESPACE =
    @"<ns1:Root xmlns:ns1='http://NamespaceTest.com/CustomerTypes'>
        <Child1>content1</Child1>
        <Child2>content1</Child2>
    </ns1:Root>";

        public const string FILENAME_XML_DTD_NAMESPACE = "dtd_namespace.xml";
        public const string CONTENT_XML_DTD_NAMESPACE =
    @"<?xml version='1.0' encoding='utf-8' standalone='no'?>
    <!DOCTYPE BPS SYSTEM 'bpml.dtd'>
    <ns1:Root xmlns:ns1='http://NamespaceTest.com/CustomerTypes'>
        <Child1>content1</Child1>
        <Child2>content1</Child2>
    </ns1:Root>";


        public ICFTInputFileInfo GetFakeFileInfo(XMLType type)
        {
            var content = string.Empty;
            switch (type)
            {
                case XMLType.ATTRIBUTES:
                    content = CONTENT_XML_ATTRIBUTES;
                    break;
                case XMLType.NAMESPACE:
                    content = CONTENT_XML_NAMESPACE;
                    break;
                case XMLType.SIMPLE:
                    content = CONTENT_XML;
                    break;
                case XMLType.DTD_NAMESPACE:
                    content = CONTENT_XML_DTD_NAMESPACE;
                    break;
            }
            var fakeFileInfo = A.Fake<ICFTInputFileInfo>();
            A.CallTo(() => fakeFileInfo.FileContent)
                .Returns(Encoding.Default.GetBytes(content));
            return fakeFileInfo;
        }

        public string GetFullPath(string fileName)
        {
            switch (fileName)
            {
                case FILENAME_XML:
                case FILENAME_XML_ATTRIBUTES:
                case FILENAME_XML_NAMESPACE:
                case FILENAME_XML_DTD_NAMESPACE:
                    break;
                default:
                    throw new Exception($"Файл '{fileName}' не создавался.");
            }

            return Path.Combine(PATH, fileName);
        }

        Dictionary<string, string> FilesWithContent = new Dictionary<string, string>
        {
            { FILENAME_XML, CONTENT_XML },
            { FILENAME_XML_ATTRIBUTES,  CONTENT_XML_ATTRIBUTES},
            { FILENAME_XML_NAMESPACE,  CONTENT_XML_NAMESPACE },
            { FILENAME_XML_DTD_NAMESPACE,  CONTENT_XML_DTD_NAMESPACE },
        };

        public XMLFixture()
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
