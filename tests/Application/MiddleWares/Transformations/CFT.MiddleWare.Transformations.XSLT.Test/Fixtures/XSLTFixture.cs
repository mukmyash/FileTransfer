using CFT.MiddleWare.Base;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFT.MiddleWare.Transformations.XSLT.Test.Fixtures
{
    public class XSLTFixture : IDisposable
    {
        private readonly string PATH = Path.Combine(".", "xslt");

        public const string FILENAME_DATA_XML = "data.xml";
        public const string FILENAME_VALID_XSL = "valid.xsl";
        public const string FILENAME_VALID_XSLT = "valid.xslt";
        public const string FILENAME_NOT_VALID_XSLT = "not-valid.xslt";
        public const string FILENAME_BAD_EXTENSION_TXT = "bad-extension.txt";

        public const string CONTENT_XSLT_NOT_VALID = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">  <xsl:template match=\"bookstore\">    <HTML>      <BODY>        <TABLE BORDER=\"2\">          <TR>            <TD>ISBN</TD>            <TD>Title</TD>            <TD>Price</TD>          </TR>          <xsl:apply-templates select=\"book\"/>        </TABLE>      </BODY>    </HTML>  </xsl:template>  <xsl:template match=\"book\">    <TR>      <TD>        <xsl:value-of select=\"@ISBN\"/>      </TD>      <TD>        <xsl:value-of select=\"title\"/>      </TD>      <TD>        <xsl:value-of select=\"price\"/>      </TD>    </TR>  </xsl:template><xsl:stylesheet>";
        public const string CONTENT_XSLT_VALID = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">  <xsl:template match=\"bookstore\">    <HTML>      <BODY>        <TABLE BORDER=\"2\">          <TR>            <TD>ISBN</TD>            <TD>Title</TD>            <TD>Price</TD>          </TR>          <xsl:apply-templates select=\"book\"/>        </TABLE>      </BODY>    </HTML>  </xsl:template>  <xsl:template match=\"book\">    <TR>      <TD>        <xsl:value-of select=\"@ISBN\"/>      </TD>      <TD>        <xsl:value-of select=\"title\"/>      </TD>      <TD>        <xsl:value-of select=\"price\"/>      </TD>    </TR>  </xsl:template></xsl:stylesheet>";
        public const string CONTENT_DATA_XML_AFTER_XSLT = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HTML><BODY><TABLE BORDER=\"2\"><TR><TD>ISBN</TD><TD>Title</TD><TD>Price</TD></TR><TR><TD>1-861003-11-0</TD><TD>The Autobiography of Benjamin Franklin</TD><TD>8.99</TD></TR><TR><TD>0-201-63361-2</TD><TD>The Confidence Man</TD><TD>11.99</TD></TR><TR><TD>1-861001-57-6</TD><TD>The Gorgias</TD><TD>9.99</TD></TR></TABLE></BODY></HTML>";
        public const string CONTENT_DATA_XML = "<?xml version='1.0'?><bookstore>  <book genre=\"autobiography\" publicationdate=\"1981\" ISBN=\"1-861003-11-0\">    <title>The Autobiography of Benjamin Franklin</title>    <author>      <first-name>Benjamin</first-name>      <last-name>Franklin</last-name>    </author>    <price>8.99</price>  </book>  <book genre=\"novel\" publicationdate=\"1967\" ISBN=\"0-201-63361-2\">    <title>The Confidence Man</title>    <author>      <first-name>Herman</first-name>      <last-name>Melville</last-name>    </author>    <price>11.99</price>  </book>  <book genre=\"philosophy\" publicationdate=\"1991\" ISBN=\"1-861001-57-6\">    <title>The Gorgias</title>    <author>      <name>Plato</name>    </author>    <price>9.99</price>  </book></bookstore>";

        public string GetFullPath(string fileName)
        {
            switch (fileName)
            {
                case FILENAME_DATA_XML:
                case FILENAME_BAD_EXTENSION_TXT:
                case FILENAME_VALID_XSLT:
                case FILENAME_NOT_VALID_XSLT:
                case FILENAME_VALID_XSL:
                    return Path.Combine(PATH, fileName);
                default:
                    throw new Exception($"Файл '{fileName}' не создавался.");
            }
        }

        public ICFTInputFileInfo GetFakeFileInfo()
        {
            var fakeFileInfo = A.Fake<ICFTInputFileInfo>();
            A.CallTo(() => fakeFileInfo.FileContent)
                .Returns(Encoding.Default.GetBytes(CONTENT_DATA_XML));
            return fakeFileInfo;
        }

        Dictionary<string, string> FilesWithContent = new Dictionary<string, string>
        {
            { FILENAME_NOT_VALID_XSLT,CONTENT_XSLT_NOT_VALID },
            { FILENAME_DATA_XML,  CONTENT_DATA_XML },
            { FILENAME_VALID_XSL, CONTENT_XSLT_VALID},
            { FILENAME_VALID_XSLT, CONTENT_XSLT_VALID},
            { FILENAME_BAD_EXTENSION_TXT, CONTENT_XSLT_VALID},
        };

        public XSLTFixture()
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
