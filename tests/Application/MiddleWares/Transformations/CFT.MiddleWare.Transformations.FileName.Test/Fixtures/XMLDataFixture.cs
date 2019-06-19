using CFT.MiddleWare.Base;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CFT.MiddleWare.Transformations.FileName.Test.Fixtures
{
    public class XMLDataFixture : IDisposable
    {
        public const string XPATH_ELEMENT = "/bookstore/book[2]/author/first-name";
        public const string XPATH_ATTRIBUTE = "/bookstore/book[2]/@genre";
        public const string XPATH_ELEMENT_VALUE = "Herman";
        public const string XPATH_ATTRIBUTE_VALUE = "novel";

        public const string CONTENT_DATA_XML = "<?xml version='1.0'?><bookstore>  <book genre=\"autobiography\" publicationdate=\"1981\" ISBN=\"1-861003-11-0\">    <title>The Autobiography of Benjamin Franklin</title>    <author>      <first-name>Benjamin</first-name>      <last-name>Franklin</last-name>    </author>    <price>8.99</price>  </book>  <book genre=\"novel\" publicationdate=\"1967\" ISBN=\"0-201-63361-2\">    <title>The Confidence Man</title>    <author>      <first-name>Herman</first-name>      <last-name>Melville</last-name>    </author>    <price>11.99</price>  </book>  <book genre=\"philosophy\" publicationdate=\"1991\" ISBN=\"1-861001-57-6\">    <title>The Gorgias</title>    <author>      <name>Plato</name>    </author>    <price>9.99</price>  </book></bookstore>";

        /// <summary>
        /// Получаем контекст формирования параметров.
        /// </summary>
        /// <returns></returns>
        internal ParameterContext GetParameterContext()
        {
            return new ParameterContext(
                context: new Base.CFTFileContext(
                    applicationServices: new ServiceCollection().BuildServiceProvider(),
                    inputFile: this.GetFakeFileInfo()));
        }

        public ICFTInputFileInfo GetFakeFileInfo()
        {
            var fakeFileInfo = A.Fake<ICFTInputFileInfo>();
            A.CallTo(() => fakeFileInfo.FileContent)
                .Returns(Encoding.Default.GetBytes(CONTENT_DATA_XML));
            return fakeFileInfo;
        }

        public XmlElement GetXmlElement()
        {
            var xmlDocument = new XmlDocument();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(CONTENT_DATA_XML)))
            {
                xmlDocument.Load(stream);
            }

            return xmlDocument.DocumentElement;
        }

        /// <summary>
        /// Проверяем XML элемент на совпадение с XML документом из Fixture.
        /// </summary>
        /// <param name="XmlRoot"></param>
        public void CheckXmlElement(XmlElement XmlRoot)
        {
            XmlRoot.Should().NotBeNull();
            XmlRoot.Name.Should().Be("bookstore");
            XmlRoot.ChildNodes.Count.Should().Be(3);
            XmlRoot.FirstChild.Name.Should().Be("book");
            XmlRoot.FirstChild.Attributes.Count.Should().Be(3);
            XmlRoot.FirstChild.Attributes["genre"].Value.Should().Be("autobiography");
            XmlRoot.FirstChild.Attributes["ISBN"].Value.Should().Be("1-861003-11-0");
            XmlRoot.FirstChild.Attributes["publicationdate"].Value.Should().Be("1981");
            XmlRoot.FirstChild.FirstChild.Name.Should().Be("title");
            XmlRoot.FirstChild.FirstChild.InnerText.Should().Be("The Autobiography of Benjamin Franklin");
            XmlRoot.FirstChild.LastChild.Name.Should().Be("price");
            XmlRoot.FirstChild.LastChild.InnerText.Should().Be("8.99");
        }

        public void Dispose()
        {
        }
    }
}
