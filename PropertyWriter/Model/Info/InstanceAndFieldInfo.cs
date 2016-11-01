using System;
using System.Reflection;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class InstanceAndFieldInfo : InstanceAndMemberInfo
	{
		private readonly FieldInfo field_;

		public InstanceAndFieldInfo(FieldInfo field, IPropertyModel model)
			: base(field.Name, model)
		{
			field_ = field;
		}

		public override Type Type => field_.FieldType;
		public override object GetValue(object obj) => field_.GetValue(obj);
		public override void SetValue(object obj, object value) => field_.SetValue(obj, value);
		
		public static InstanceAndFieldInfo ForMember(FieldInfo field)
		{
			return new InstanceAndFieldInfo(field, InstanceFactory.Create(field.FieldType));
		}

		public static InstanceAndFieldInfo ForReferenceMember(FieldInfo field, Type type, string idMemberName)
		{
			return new InstanceAndFieldInfo(field, InstanceFactory.CreateReference(field.FieldType, type, idMemberName));
		}
	}
}