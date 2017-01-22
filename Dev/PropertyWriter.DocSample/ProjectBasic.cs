using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.ProjectBasic
{
	[PwProject]
    public class MyProject
    {
		[PwMaster]
		public int[] Data { get; set; }
		[PwMaster]
		public Hoge[] Referencable { get; set; }
	}

	public class Hoge
	{
		[PwMember]
		public int X { get; set; }
		[PwMember]
		public string Message { get; set; }
		[PwMember]
		public bool Check { get; set; }
	}
}
