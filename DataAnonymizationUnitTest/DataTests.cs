using DataAnonymization;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DataAnonymizationUnitTest;

[TestClass]
public class UnitTest
{
    string inputJSON = File.ReadAllText("\\\\Mac\\Home\\Documents\\DataAnonymization\\Project\\DataAnonymizationUnitTest\\Sample.json");
    
    [TestMethod]
    public void TestShuffle()
    {

            string testJSON = "";
            string path ="$[*].Customer.LastName";

            testJSON = new DataAnonymizationLibrary().Shuffle(inputJSON,path);
            Console.Write("InputJSON:"+inputJSON);
            Console.Write("ProcessedJSON:"+testJSON);
            Assert.AreNotEqual(testJSON,inputJSON);
            
            File.WriteAllText("inputread.json",inputJSON);
            File.WriteAllText("inputparsed.json",testJSON);

    }
}