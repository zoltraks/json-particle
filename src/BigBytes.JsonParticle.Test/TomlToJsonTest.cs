using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigBytes.JsonParticle.Test
{
    [TestClass]
    public class TomlToJsonTest
    {
        [TestMethod]
        public void Convert()
        {
            string needle, expect, result;

            needle = null;
            expect = null;
            result = new Converter.TomlToJson().Convert(needle);
            Assert.AreEqual(expect, result);

            needle = "";
            expect = "{}";
            result = new Converter.TomlToJson().Convert(needle);
            Assert.AreEqual(expect, result);

            needle = "key = value";
            expect = @"{""_"":{""key"":""value""}}";
            result = new Converter.TomlToJson().Convert(needle, false);
            Assert.AreEqual(expect, result);

            needle = @"
[Global]
Print = console

[Security]
Enable = true

[ABC]
XYZ = 123
";
            expect = @"{""global"":{""print"":""console""},""security"":{""enable"":""true""},""ABC"":{""XYZ"":""123""}}";
            result = new Converter.TomlToJson().Convert(needle, false);
            Assert.AreEqual(expect, result);

            needle = Converter.TomlToJson.TOML_EXAMPLE;
            expect = Converter.TomlToJson.JSON_EXAMPLE.Trim();
            result = new Converter.TomlToJson().Convert(needle, true);
            Assert.AreEqual(expect, result);
        }
    }
}
