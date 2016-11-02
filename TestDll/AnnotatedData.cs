using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace TestDll
{
	[PwMaster]
	class AppleAnnotatedData
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public int X;
		[PwMember]
		public AtherAnnotatedData Ather { get; set; }

		public override string ToString() => $"Apple: {Id}, {X}";
	}

    [PwMaster]
    class OrangeAnnotatedData
    {
        [PwReferenceMember(typeof(AppleAnnotatedData), nameof(AppleAnnotatedData.Id))]
        public int AppleId { get; set; }

        public override string ToString() => $"Orange: {AppleId}";
    }

	[PwSubtyping]
	[PwMinor]
	class AtherAnnotatedData
	{
		[PwMember]
		public string Title { get; set; }

		public override string ToString() => $"Ather: {Title}";
	}

	[PwSubtype]
	[PwMinor]
	class IgnisAnnotatedData : AtherAnnotatedData
	{
		[PwMember]
		public int FirePower { get; set; }

		public override string ToString() => $"Ignis: {Title}, {FirePower}";
	}

	[PwSubtype]
	[PwMinor]
	class AquaAnnotatedData : AtherAnnotatedData
	{
		[PwMember]
		public int Volume { get; set; }

		public override string ToString() => $"Aqua: {Title}, {Volume}";
	}

	[PwGlobal]
	struct UniverseAnnotatedData
	{
		[PwMember]
		public string ProjectName { get; set; }
		[PwMember]
		public int FileNum;
		[PwReferenceMember(typeof(AppleAnnotatedData), nameof(AppleAnnotatedData.Id))]
		public int AppleId { get; set; }
		[PwMember]
		public AtherAnnotatedData Ather { get; set; }
	}
}
