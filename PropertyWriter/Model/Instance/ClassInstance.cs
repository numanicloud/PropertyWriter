﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class ClassInstance : Instance
	{
		public ClassInstance( Type type )
		{
			if( !type.IsClass )
			{
				throw new ArgumentException( "type がクラスを表す Type クラスではありません。" );
			}

			ClassValue = new StructureValue( type );
		}

		public IEnumerable<InstanceAndPropertyInfo> Properties => ClassValue.Properties;
		public override ReactiveProperty<object> Value => ClassValue.Value;

		public override ReactiveProperty<string> FormatedString
		{
			get
			{
				var events = ClassValue.Properties
					.Select(x => x.Instance.Value)
					.Cast<IObservable<object>>()
					.ToArray();
				return Observable.Merge(events)
					.Select(x => Value.Value.ToString())
					.ToReactiveProperty();
			}
		}

		private StructureValue ClassValue { get; set; }
	}
}
