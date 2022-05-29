using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

#pragma warning disable 0649

namespace BigBytes.JsonParticle.Test
{
    [TestClass]
    public class ConverterTest
    {
        internal class Mock
        {
            internal class TomlExample : Json<TomlExample>
            {
                internal class __
                {
                    public string Title;
                }

                internal class _Owner
                {
                    public string Name;
                    public string Dob;
                }

                internal class _Database
                {
                    public string Enabled;
                    public string Ports;
                    public string Data;
                    public string TempTargets;
                }

                internal class _Servers
                {
                    public string Ip;
                    public string Role;
                }

                public __ _;
                public _Owner Owner;
                public _Database Database;
                public _Servers ServersAlpha;
                public _Servers ServersBeta;
            }

            public class Record : Json<Record>
            {
                public class _Book
                {
                    public string Title;
                    public int Year;
                }

                public class _Author
                {
                    public string Name;
                }

                public _Book Book;
                public _Author Author;
            }
        }

        [TestMethod]
        public void JSON()
        {
            var json = "";
            var record = default(Mock.Record);
            record = new Mock.Record()
            {
                Book = new Mock.Record._Book()
                {
                    Title = "UML Distilled",
                    Year = 2003,
                },
                Author = new Mock.Record._Author()
                {
                    Name = "Martin Fowler",
                },
            };
            json = Mock.Record.Serialize(record);
            Debug.WriteLine(json);
            Assert.IsFalse(string.IsNullOrEmpty(json));
            json = @"
{
  ""book"": {
    ""title"": ""UML Distilled"",
    ""year"": 2003
  },
  ""author"": {
    ""name"": ""Martin Fowler""
  }
}
";
            record = Mock.Record.Deserialize(json);
            Assert.AreEqual("UML Distilled", record.Book?.Title);
            Assert.AreEqual(2003, record.Book?.Year);
            Assert.AreEqual("Martin Fowler", record.Author?.Name);
            Debug.WriteLine(record.Book.Title); // expect "UML Distilled"
            Debug.WriteLine(record.Book.Year);  // expect "2003"
            Debug.WriteLine(record.Author.Name); // expect Martin Fowler"
        }

        [TestMethod]
        public void TOML()
        {
            var json = "";
            var toml = "";

            var o1 = new Mock.TomlExample()
            {
                _ = new Mock.TomlExample.__
                {
                    Title = "X",
                },
                Database = new Mock.TomlExample._Database()
                {
                    Enabled = "true",
                },
            };

            json = Mock.TomlExample.Serialize(o1);
            Assert.IsFalse(string.IsNullOrEmpty(json));

            toml = Converter.TomlToJson.TOML_EXAMPLE;
            json = new Converter.TomlToJson().Convert(toml);

            var o = Mock.TomlExample.Deserialize(json);
            Assert.IsNotNull(o);
            Assert.AreEqual("TOML Example", o._?.Title);
            Assert.AreEqual("10.0.0.2", o.ServersBeta?.Ip);
            Assert.IsTrue(true);

            toml = @"
[book]
title = ""UML Distilled""
year = 2003

[author]
name = ""Martin Fowler""
";
            json = new Converter.TomlToJson().Convert(toml);
            Assert.IsNotNull(json);
            var record = Mock.Record.Deserialize(json);
            Assert.IsNotNull(record);
            Assert.AreEqual("UML Distilled", record.Book?.Title);
            Assert.AreEqual(2003, record.Book?.Year);
            Assert.AreEqual("Martin Fowler", record.Author?.Name);
        }
    }
}
