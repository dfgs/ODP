using System.Globalization;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class DateTimeParserUnitTest
	{
		[DataTestMethod]
		[DataRow("18:57:52.320  UTC Mon Dec 05 2022")]
		public void ShouldParseCDRDateTime(string Line)
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseCDRDate(Line);
			Assert.IsNotNull(result);
			Assert.AreEqual(2022, result.Value.Year);
		}
		[DataTestMethod]
		[DataRow("18:57:52.320")]
		[DataRow("2023-03-06 18:40:00")]
		public void ShouldParseSyslogDateTime(string Line)
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseSyslogDate(Line);
			Assert.IsNotNull(result);
			Assert.AreEqual(18, result.Value.Hour);
		}
		[TestMethod]
		public void ShouldParseNullValue()
		{
			DateTimeParser parser;
			DateTime? result;

			parser = new DateTimeParser();
			result = parser.ParseCDRDate(null);
			Assert.IsNull(result);
			result = parser.ParseCDRDate("");
			Assert.IsNull(result);
		}


	}
}