using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cft.Application.Tests.Fixtures
{
    public class DataXMLFixture : IDisposable
    {
        private readonly string PATH = Path.Combine(".", "xml");

        public const string FILENAME_BOOKSTORE_DATA_XML = "data.xml";
        public const string CONTENT_BOOKSTORE_DATA_CML = "<?xml version='1.0'?><bookstore>  <book genre=\"autobiography\" publicationdate=\"1981\" ISBN=\"1-861003-11-0\">    <title>The Autobiography of Benjamin Franklin</title>    <author>      <first-name>Benjamin</first-name>      <last-name>Franklin</last-name>    </author>    <price>8.99</price>  </book>  <book genre=\"novel\" publicationdate=\"1967\" ISBN=\"0-201-63361-2\">    <title>The Confidence Man</title>    <author>      <first-name>Herman</first-name>      <last-name>Melville</last-name>    </author>    <price>11.99</price>  </book>  <book genre=\"philosophy\" publicationdate=\"1991\" ISBN=\"1-861001-57-6\">    <title>The Gorgias</title>    <author>      <name>Plato</name>    </author>    <price>9.99</price>  </book></bookstore>";
        public string GetFullPath(string fileName)
        {
            switch (fileName)
            {
                case FILENAME_BOOKSTORE_DATA_XML:
                    break;
                default:
                    throw new Exception($"Файл '{fileName}' не создавался.");
            }

            return Path.Combine(PATH, fileName);
        }

        Dictionary<string, string> FilesWithContent = new Dictionary<string, string>
        {
            { FILENAME_BOOKSTORE_DATA_XML, CONTENT_BOOKSTORE_DATA_CML },
        };

        public DataXMLFixture()
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
