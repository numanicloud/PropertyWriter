﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Common
{
    internal class StructureHolder
    {
        public IEnumerable<IPropertyModel> Properties { get; }
        public ReactiveProperty<object> Value { get; private set; }
        public Subject<Unit> ValueChanged { get; } = new Subject<Unit>();
        
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
                    property.PropertyInfo.SetValue(Value.Value, x);
                    if (property is ReferenceByIntProperty refModel)
                    {
                        refModel.PropertyToBindBack?.SetValue(Value.Value, refModel.SelectedObject.Value);
                    }
                    ValueChanged.OnNext(Unit.Default);
                });
            }
        }
    }
}