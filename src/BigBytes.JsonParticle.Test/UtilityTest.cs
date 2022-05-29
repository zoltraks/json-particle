using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigBytes.JsonParticle.Test
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void TomlSectionNameToCSharpName()
        {
            var toml = "list.one.12";
            var csharp = BigBytes.JsonParticle.Utility.TomlSectionNameToCSharpName(toml);
            System.Diagnostics.Debug.WriteLine(csharp); // expect "myObject"

            string search, expect, result;

            search = null;
            expect = null;
            result = Utility.TomlSectionNameToCSharpName(search);
            Assert.AreEqual(expect, result);
            
            search = "abc-def.= .. xyz1 23 4ah óŚ";
            expect = "AbcDefXyz1_23_4ahÓś";
            result = Utility.TomlSectionNameToCSharpName(search);
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void CSharpNameToJsonName()
        {
            string search, expect, result;

            search = null;
            expect = null;
            result = Utility.CSharpNameToJsonName(search);
            Assert.AreEqual(expect, result);

            search = "AbcDefXyz1_23_4ahÓś";
            expect = "abcDefXyz1_23_4ahÓś";
            result = Utility.CSharpNameToJsonName(search);
            Assert.AreEqual(expect, result);

            search = "XMLtoJSON";
            expect = "XMLtoJSON";
            result = Utility.CSharpNameToJsonName(search);
            Assert.AreEqual(expect, result);

            search = "XmlToJson";
            expect = "xmlToJson";
            result = Utility.CSharpNameToJsonName(search);
            Assert.AreEqual(expect, result);
        }
    }
}
