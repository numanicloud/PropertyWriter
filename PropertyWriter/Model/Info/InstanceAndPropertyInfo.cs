﻿using System;
using System.Reflection;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class InstanceAndPropertyInfo : InstanceAndMemberInfo
	{
		private readonly PropertyInfo property_;

		public InstanceAndPropertyInfo(PropertyInfo property, IPropertyModel model)
			: base(property.Name, model)
		{
			property_ = property;
		}

		public override Type Type => property_.PropertyType;

		public override object GetValue(object obj) => property_.GetValue(obj);
		public override void SetValue(object obj, object value) => property_.SetValue(obj, value);
	}
}