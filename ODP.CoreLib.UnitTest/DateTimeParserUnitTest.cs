using System.Globalization;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class DateTimeParserUnitTest
	{
		[DataTestMethod]
		[DataRow("18:57:52.320  UTC Mon Dec 05 2022")]
		public void ShouldParseDateTime(string Line)
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.Parse(Line);
			Assert.IsNotNull(result);
			Assert.AreEqual(2022, result.Value.Year);
		}

		[TestMethod]
		public void ShouldParseNullValue()
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.Parse(null);
			Assert.IsNull(result);
			result = parser.Parse("");
			Assert.IsNull(result);
		}


	}
}