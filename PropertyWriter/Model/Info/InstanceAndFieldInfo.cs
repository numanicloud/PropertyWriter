using System;
using System.Reflection;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class InstanceAndFieldInfo : InstanceAndMemberInfo
	{
		private readonly FieldInfo field_;

		public InstanceAndFieldInfo(FieldInfo field, IPropertyModel model, string name)
			: base(name ?? field.Name, model)
		{
			field_ = field;
		}

		public override Type Type => field_.FieldType;
		public override string MemberName => field_.Name;
		public override object GetValue(object obj) => field_.GetValue(obj);
		public override void SetValue(object obj, object value) => field_.SetValue(obj, value);
	}
}