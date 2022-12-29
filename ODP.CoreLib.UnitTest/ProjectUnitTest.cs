using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class ProjectUnitTest
	{


		[TestMethod]
		public async Task AddFileAsyncShouldCheckParameters()
		{
			Project project;

			project = new Project();

			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync(null, Mock.Of<SyslogParser>(), Mock.Of<ReportParser>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", null, Mock.Of<ReportParser>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<SyslogParser>(), null));

		}

		[TestMethod]
		public void AddReportShouldCheckParameter()
		{
			Project project;

			project = new Project();

			Assert.ThrowsException<ArgumentNullException>(() => project.AddReport(null));

		}


	}


}