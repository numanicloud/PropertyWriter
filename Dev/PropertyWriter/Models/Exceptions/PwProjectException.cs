using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Exceptions
{
	class PwProjectException : Exception
	{
		public PwProjectException(string message)
			: base(message)
		{
		}
	}
}
