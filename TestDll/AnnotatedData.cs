using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace TestDll
{
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
	
    class OrangeAnnotatedData
    {
        [PwReferenceMember("Apple", nameof(AppleAnnotatedData.Id))]
        public int AppleId { get; set; }

        public override string ToString() => $"Orange: {AppleId}";
    }

	[PwSubtyping]
	class AtherAnnotatedData
	{
		[PwMember]
		public string Title { get; set; }

		public override string ToString() => $"Ather: {Title}";
	}

	[PwSubtype]
	class IgnisAnnotatedData : AtherAnnotatedData
	{
		[PwMember]
		public int FirePower { get; set; }

		public override string ToString() => $"Ignis: {Title}, {FirePower}";
	}

	[PwSubtype]
	class AquaAnnotatedData : AtherAnnotatedData
	{
		[PwMember]
		public int Volume { get; set; }

		public override string ToString() => $"Aqua: {Title}, {Volume}";
	}
	
	struct UniverseAnnotatedData
	{
		[PwMember]
		public string ProjectName { get; set; }
		[PwMember]
		public int FileNum;
		[PwReferenceMember("Apple", nameof(AppleAnnotatedData.Id))]
		public int AppleId { get; set; }
		[PwMember]
		public AtherAnnotatedData Ather { get; set; }
	}
}
