using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAnonymization
 {
     public class DataAnonymizationLibrary : IDataAnonymizationLibrary
     {
        /// <summary>
        /// Helpers
        /// </summary>
        public static Random rand = new Random();

        private string GenerateRandomString()
        {

            StringBuilder result = new StringBuilder();
            char chr;
            for (int i = 0; i < 32; i++)
            {
                int seed = 0;
                Math.DivRem(rand.Next(), 25, out seed);
                chr = Convert.ToChar(seed + 65);
                result.Append(chr);
            }
            return result.ToString();
        }

        private DateTime GenerateRandomDate()
        {

            StringBuilder result = new StringBuilder();
            DateTime dtt = DateTime.Parse("2000-01-01").AddDays(rand.Next(9000));
            return dtt;
        }

        private int GenerateRandomNumber()
        {

            return rand.Next();
        }


        /// <summary>
        /// Copies the a new value to a specific path. To be used with JSONPath notation.
        /// </summary>
        /// <param name="JSONInput">Input JSON to be processed</param>
        /// <param name="Path">Location of the item to be updated</param>
        /// <param name="Value">Value to copy to the input JSON at the designated path</param>
        /// <param name="JSONOutput">Output parameter with processed JSON</param>
        public string Copy(string JSONInput, string Path, string Value) {
			string ssJSONOutput = "";
            // TODO: Write implementation for action
            JToken root = JToken.Parse(JSONInput);

            var filteredTokens = root.SelectTokens(Path).ToList();
            foreach (var tok in filteredTokens)
            {
                tok.Replace(JToken.FromObject(Value));
            }
            ssJSONOutput = JsonConvert.SerializeObject(root, Formatting.Indented);
            return ssJSONOutput;

        } // Copy

        /// <summary>
        /// Masks the values at the designated path using a regular expression
        /// </summary>
        /// <param name="JSONInput">Input JSON to be processed</param>
        /// <param name="Path">Location of the item to be shuffled</param>
        /// <param name="Value">Value to copy to the match on the expression</param>
        /// <param name="Expression">Regular expression to use while replacing the values.</param>
        public string Mask(string JSONInput, string Path, string Value, string Expression) {
			string ssJSONOutput = "";
            // TODO: Write implementation for action
            JToken root = JToken.Parse(JSONInput);

            var filteredTokens = root.SelectTokens(Path).ToList();
            foreach (var tok in filteredTokens)
            {
                tok.Replace(JToken.FromObject(Regex.Replace(tok.ToString(), Expression, Value)));
            }
            ssJSONOutput = JsonConvert.SerializeObject(root, Formatting.Indented);
            return ssJSONOutput;
        } // MssJSONPath_Mask

		/// <summary>
		/// Generate a random value at the designated JSON path
		/// </summary>
		/// <param name="ssJSONInput">Input JSON to be processed</param>
		/// <param name="ssPath">Location of the item to be updated. JSONPath notation</param>
		/// <param name="ssDataType">DataType of the random value to be generated. Currently supports: TEXT, INTEGER, DATE, EMAIL</param>
		public string Randomize(string JSONInput, string Path, string DataType) {
			string ssJSONOutput = "";
            // TODO: Write implementation for action
            JToken root = JToken.Parse(JSONInput);

            var filteredTokens = root.SelectTokens(Path).ToList();

            foreach (var tok in filteredTokens)
            {
                switch (DataType)
                {
                    case "DATE":
                        tok.Replace(JToken.FromObject(GenerateRandomDate().Date.ToString("yyyy-MM-dd"))); break;
                    case "TEXT":
                        tok.Replace(JToken.FromObject(GenerateRandomString())); break;
                    case "INTEGER":
                        tok.Replace(JToken.FromObject(GenerateRandomNumber())); break;
                    case "EMAIL":
                        tok.Replace(JToken.FromObject(GenerateRandomString() + "@test.com")); break;
                }
            }

            ssJSONOutput = JsonConvert.SerializeObject(root, Formatting.Indented);

            return ssJSONOutput;
        } // MssJSONPath_Randomize

		/// <summary>
		/// This method shuffles data on a specific JSONPath.
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be shuffled</param>
		public string  Shuffle(string JSONInput, string Path) {
			string ssJSONOutput = "";
            // TODO: Write implementation for action
            JToken root = JToken.Parse(JSONInput);

            var filteredTokens = root.SelectTokens(Path).ToList();

            JToken[] copiedTokens = new JToken[filteredTokens.Count];
            filteredTokens.CopyTo(copiedTokens);
            var BackupList = copiedTokens.ToList();

            foreach (var tok in filteredTokens)
            {
                int pos = 0;
                Math.DivRem(rand.Next(), BackupList.Count, out pos);
                tok.Replace(JToken.FromObject(BackupList[pos]));
                BackupList.RemoveAt(pos);
            }
            ssJSONOutput = JsonConvert.SerializeObject(root, Formatting.Indented);
            return ssJSONOutput;
        } // MssJSONPath_Shuffle

		/// <summary>
		/// Method that applies a data variance on numeric and date attributes.
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be updated</param>
		/// <param name="MaxVariation">Maximum variation to randomly apply
		/// </param>
		public string Variation(string JSONInput, string Path, int MaxVariation) {
			string ssJSONOutput = "";
            // TODO: Write implementation for action
            JToken root = JToken.Parse(JSONInput);

            var filteredTokens = root.SelectTokens(Path).ToList();

            foreach (var tok in filteredTokens)
            {

                int seed = 0;
                decimal random = 0;
                Math.DivRem(GenerateRandomNumber(), MaxVariation, out seed);
                random = ((decimal)100 + seed) / 100;

                //This part is not pretty but needed as the JTokenType is always string
                try
                {

                    DateTime date = Convert.ToDateTime(tok.ToString());
                    date = new DateTime((long)Math.Floor(date.Ticks * random));
                    tok.Replace(JToken.FromObject(date.ToString("yyyy-MM-dd")));

                }
                catch (Exception e)
                {
                    try
                    {
                        var tokStr = tok.ToString();
                        if (tokStr.Contains(".") || tokStr.Contains(","))
                        {
                            //random = (decimal)1 + (decimal)(seed / 100);
                            decimal dec = (decimal)tok;
                            dec = dec * random;
                            tok.Replace(JToken.FromObject(dec));

                        }
                        else
                        {
                            int i = (int)tok, j;
                            Math.DivRem(GenerateRandomNumber(), MaxVariation, out j);
                            i = i + j;
                            tok.Replace(JToken.FromObject(i));
                        }
                    }
                    catch (Exception ee)
                    {

                    }
                }

            }
            ssJSONOutput = JsonConvert.SerializeObject(root, Formatting.Indented);
            return ssJSONOutput;
        }



        
     }

 }