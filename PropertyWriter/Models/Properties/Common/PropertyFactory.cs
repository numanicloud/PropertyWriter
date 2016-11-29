﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels;
using PropertyWriter.ViewModels.Properties;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Common
{
    class ReferencableMasterInfo
    {
        public Type Type { get; set; }
        public ReadOnlyReactiveCollection<object> Collection { get; set; }
    }

    class PropertyFactory
    {
        private Dictionary<string, ReferencableMasterInfo> masters_;
        private Dictionary<Type, Type[]> subtypings_;

        public PropertyFactory()
        {
            masters_ = new Dictionary<string, ReferencableMasterInfo>();
            subtypings_ = new Dictionary<Type, Type[]>();
        }

        public PropertyRoot GetStructure(Assembly assembly, Type projectType)
        {
            LoadSubtypes(assembly);

            var masterMembers = projectType.GetMembers();
            var masters = GetMastersInfo(masterMembers, true).ToArray();
            LoadMasters(masters);

            var globals = GetMastersInfo(masterMembers, false);
            var models = globals.Concat(masters).ToArray();

            return new PropertyRoot(projectType, models.ToArray());
        }

        private void LoadMasters(MasterInfo[] masters)
        {
			masters_ = new Dictionary<string, ReferencableMasterInfo>();
			foreach (var info in masters)
			{
				if (info.Master is ComplicateCollectionProperty prop)
				{
					masters_[info.Key] = new ReferencableMasterInfo()
					{
						Collection = prop.Collection.ToReadOnlyReactiveCollection(y => y.Value.Value),
						Type = info.Property.PropertyType.GetElementType(),
					};
				}
			}
        }

        private void LoadSubtypes(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
            var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
            subtypings_ = subtypings.ToDictionary(x => x, x => subtypes.Where(y => y.BaseType == x).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterMembers"></param>
        /// <param name="filterArrayType"></param>
        /// <returns></returns>
        /// <remarks>リストマスターが読み込まれていない場合<paramref name="filterArrayType"/>をfalseにすると例外を投げる</remarks>
        private IEnumerable<MasterInfo> GetMastersInfo(MemberInfo[] masterMembers, bool filterArrayType)
        {
            foreach (var member in masterMembers)
            {
                var attr = member.GetCustomAttribute<PwMasterAttribute>();
                if (attr == null)
                {
                    continue;
                }

                if (member.MemberType != MemberTypes.Property)
                {
                    continue;
                }

                var property = (PropertyInfo)member;
                if (property.PropertyType.IsArray == filterArrayType)
                {
                    yield return new MasterInfo(attr.Key, property, Create(property.PropertyType, attr.Name));
                }
            }
        }

        public IEnumerable<IPropertyModel> GetMembers(Type type)
        {
            var properties = type.GetProperties()
                .Select(MakeModel)
                .Where(x => x != null)
                .ToArray();

            LoadBackwardBind(type, properties);

            return properties.Cast<IPropertyModel>().ToArray();
        }

        private static void LoadBackwardBind(Type type, IPropertyModel[] properties)
        {
            var references = new Dictionary<string, ReferenceByIntProperty>();
            foreach (var prop in properties)
            {
                if (prop is ReferenceByIntProperty refByInt)
                {
                    references[prop.PropertyInfo.Name] = refByInt;
                }
            }

            foreach (var prop in type.GetProperties())
            {
                var bindAttr = prop.GetCustomAttribute<PwBindBackAttribute>();
                if (bindAttr != null)
                {
                    references[bindAttr.PropertyName].PropertyToBindBack = prop;
                }
            }
        }

        private IPropertyModel MakeModel(PropertyInfo info)
        {
            IPropertyModel result = null;

            var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
            if (memberAttr != null)
            {
                result = Create(info.PropertyType, memberAttr.Name ?? info.Name);
            }

            var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
            if (attr != null)
            {
                result = CreateReference(info.PropertyType, attr.MasterKey, attr.IdFieldName, attr.Name ?? info.Name);
            }

            if (result != null)
            {
                result.PropertyInfo = info;
            }

            return result;
        }


        public IPropertyModel CreateReference(Type type, string masterKey, string idMemberName, string title)
        {
            var propertyType = TypeRecognizer.ParseType(type);
            switch (propertyType)
            {
            case PropertyKind.Integer:
                return new ReferenceByIntProperty(masters_[masterKey], idMemberName)
                {
                    Title = { Value = title }
                };
            default:
                throw new InvalidOperationException("ID参照はInt32のみがサポートされます。");
            }
        }

        public IPropertyModel Create(Type type, string title)
        {
            var propertyType = TypeRecognizer.ParseType(type);
            var model = Create(propertyType, type);
            model.Title.Value = title;
            return model;
        }

        private IPropertyModel Create(PropertyKind propertyType, Type type)
        {
            switch (propertyType)
            {
            case PropertyKind.Integer: return new IntProperty();
            case PropertyKind.Boolean: return new BoolProperty();
            case PropertyKind.String: return new StringProperty();
            case PropertyKind.Float: return new FloatProperty();
            case PropertyKind.Enum: return new EnumProperty(type);
            case PropertyKind.Class: return new ClassProperty(type, this);
            case PropertyKind.Struct: return new StructProperty(type, this);
            case PropertyKind.BasicCollection: return new BasicCollectionProperty(type, this);
            case PropertyKind.ComplicateCollection: return new ComplicateCollectionProperty(type, this);
            case PropertyKind.SubtypingClass: return new SubtypingProperty(type, this, subtypings_[type]);

            case PropertyKind.Unknown: return null;
            default: return null;
            }
        }
    }
}
