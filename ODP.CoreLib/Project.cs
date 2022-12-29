using System.Text.RegularExpressions;

namespace ODP.CoreLib
{
	public class Project
	{
		private static Regex CDRRegex = new Regex(@"[^|]+\|(?<CDR>.*)");
		public string? Name { get; set; }


		public List<string> CDRs
		{
			get;
			set;
		}
		public Project()
		{
			CDRs= new List<string>(); 
		}

		public async Task AddFileAsync(string FileName)
		{
			string? line;
			Match match;

			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				StreamReader reader= new StreamReader(stream);
				while (!reader.EndOfStream)
				{
					line = await reader.ReadLineAsync();
					if (line == null) continue;
					match= CDRRegex.Match(line);
					if (!match.Success) continue;

					CDRs.Add(match.Groups["CDR"].Value);
				}

			}
		}


	}
}