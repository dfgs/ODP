﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public interface IReportParser
	{
		Report? Parse(string Line);
	}
}
