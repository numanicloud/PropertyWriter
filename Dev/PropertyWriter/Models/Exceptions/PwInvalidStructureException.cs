using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Exceptions
{
	class PwInvalidStructureException : Exception
	{
		public PwInvalidStructureException(string message)
			: base(message)
		{
		}
	}
}
