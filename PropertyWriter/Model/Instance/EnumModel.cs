using System;
using System.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class EnumModel : PropertyModel
	{
		public EnumModel(Type type)
		{
			if(!type.IsEnum)
			{
				throw new ArgumentException("type が列挙型を表す Type クラスではありません。");
			}

			EnumValues = type.GetEnumValues()
				.Cast<object>()
				.ToArray();

			if(EnumValues.Length != 0)
			{
				EnumValue.Value = EnumValues[0];
			}
		}

		public object[] EnumValues { get; private set; }

		public override ReactiveProperty<object> Value => EnumValue;

		public ReactiveProperty<object> EnumValue { get; set; } = new ReactiveProperty<object>();
	}
}
