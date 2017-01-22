using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.TypeSample
{
	public enum Piyo
	{
		Foo, Bar, Buzz
	}

	[PwProject]
	public class TypesSample
	{
		[PwMaster]
		public int Integer { get; set; }
		[PwMaster]
		public float Float { get; set; }
		[PwMaster]
		public string String { get; set; }
		[PwMaster]
		public bool Boolean { get; set; }
		[PwMaster]
		public Piyo EnumValue { get; set; }
		[PwMaster]
		public Hoge ClassValue { get; set; }
		[PwMaster]
		public int[] BasicList { get; set; }
		[PwMaster]
		public Hoge[] ComplicateList { get; set; }
	}

	public class Hoge
	{
		[PwMember]
		public int Integer { get; set; }
		[PwMember]
		public float Float { get; set; }
		[PwMember]
		public string String { get; set; }
		[PwMember]
		public bool Boolean { get; set; }
		[PwMember]
		public Piyo EnumValue { get; set; }
		[PwMember]
		public Fuga ClassValue { get; set; }
		[PwMember]
		public int[] BasicList { get; set; }
		[PwMember]
		public Fuga[] ComplicateList { get; set; }
	}

	public class Fuga
	{
		[PwMember]
		public int X { get; set; }
		[PwMember]
		public int Y { get; set; }
	}
}
