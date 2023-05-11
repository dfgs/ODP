using System.Globalization;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class DateTimeParserUnitTest
	{
		[DataTestMethod]
		[DataRow("18:57:52.320  UTC Mon Dec 05 2022")]
		public void ShouldParseLongDateTime(string Line)
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseLongDate(Line);
			Assert.IsNotNull(result);
			Assert.AreEqual(2022, result.Value.Year);
		}
		[DataTestMethod]
		[DataRow("18:57:52.320")]
		public void ShouldParseShortDateTime(string Line)
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseShortDate(Line);
			Assert.IsNotNull(result);
			Assert.AreEqual(18, result.Value.Hour);
		}
		[TestMethod]
		public void ShouldParseNullValue()
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseLongDate(null);
			Assert.IsNull(result);
			result = parser.ParseLongDate("");
			Assert.IsNull(result);
		}


	}
}