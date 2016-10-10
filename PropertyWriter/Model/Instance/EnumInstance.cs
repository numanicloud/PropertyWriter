using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class EnumInstance : Instance
	{
		public EnumInstance(Type type)
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
