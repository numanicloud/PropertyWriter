﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels;
using Reactive.Bindings;
using System.Diagnostics;

namespace PropertyWriter.Models.Properties.Common
{
    internal class StructureHolder
    {
		private Subject<Unit> ValueChangedSubject { get; } = new Subject<Unit>();

        public IEnumerable<IPropertyModel> Properties { get; }
        public ReactiveProperty<object> Value { get; private set; }
		public IObservable<Unit> ValueChanged => ValueChangedSubject;
        
        public StructureHolder(Type type, PropertyFactory modelFactory)
        {
            Properties = modelFactory.GetMembers(type);
            Initialize(type);
        }

        public StructureHolder(Type type, MasterInfo[] masters)
        {
            Properties = masters.Do(x => x.Master.PropertyInfo = x.Property)
                .Select(x => x.Master)
                .ToArray();
            Initialize(type);
        }

        private void Initialize(Type type)
        {
            Value = new ReactiveProperty<object> { Value = Activator.CreateInstance(type) };

            foreach (var property in Properties)
            {
                property.Value.Subscribe(x =>
                {
					try
					{
						property.PropertyInfo.SetValue(Value.Value, x);
					}
					catch (ArgumentException ex)
					{
						throw new Exceptions.PwInvalidStructureException($"{property.Title.Value} プロパティに Set アクセサがありません。");
					}
                    if (property is ReferenceByIntProperty refModel)
                    {
                        refModel.PropertyToBindBack?.SetValue(Value.Value, refModel.SelectedObject.Value);
                    }
                    ValueChangedSubject.OnNext(Unit.Default);
                }, ex => Debugger.Log(0, "Exception", ex.ToString()));
            }
        }
    }
}