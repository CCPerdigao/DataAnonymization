 using OutSystems.ExternalLibraries.SDK;

 namespace DataAnonymization
 {
     [OSInterface(Description ="Provides actions to perform data anonymization capabilities into JSON structures.", IconResourceName ="DataAnonymization.Resources.DataAnonymizationLib.png")]

     public interface IDataAnonymizationLibrary
     {

         /// <summary>
		/// Copies the a new value to a specific path. To be used with JSONPath notation.
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be updated</param>
		/// <param name="Value">Value to copy to the input JSON at the designated path</param>
		string Copy(string JSONInput, string Path, string Value);

		/// <summary>
		/// Masks the values at the designated path using a regular expreion
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be shuffled</param>
		/// <param name="Value">Value to copy to the match on the expression</param>
		/// <param name="Expression">Regular expression to use while replacing the values.</param>
		string Mask(string JSONInput, string Path, string Value, string Expression);

		/// <summary>
		/// Generate a random value at the designated JSON path
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be updated. JSONPath notation</param>
		/// <param name="DataType">DataType of the random value to be generated. Currently supports: TEXT, INTEGER, DATE, EMAIL</param>
		string Randomize(string JSONInput, string Path, string DataType);

		/// <summary>
		/// This method shuffles data on a specific JSONPath.
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be shuffled</param>
		/// <param name="JSONOutput">Output parameter with processed JSON</param>
		string Shuffle(string JSONInput, string Path);

		/// <summary>
		/// Method that applies a data variance on numeric and date attributes.
		/// </summary>
		/// <param name="JSONInput">Input JSON to be processed</param>
		/// <param name="Path">Location of the item to be updated</param>
		/// <param name="MaxVariation">Maximum variation to randomly apply
		/// </param>
		string Variation(string JSONInput, string Path, int MaxVariation);

     }
 }