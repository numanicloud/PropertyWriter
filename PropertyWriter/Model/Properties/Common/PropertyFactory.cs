using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PropertyWriter.Annotation;
using PropertyWriter.Model.Info;
using PropertyWriter.Model.Instance;
using PropertyWriter.ViewModel;
using Reactive.Bindings;
using System.Diagnostics;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model
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
            var types = assembly.GetTypes();
            var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
            var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
            subtypings_ = subtypings.ToDictionary(x => x, x => subtypes.Where(y => y.BaseType == x).ToArray());

            var masterMembers = projectType.GetMembers();
            var masters = GetMastersInfo(masterMembers, true).ToArray();
            masters_ = masters.Where(x => x.Master is ComplicateCollectionViewModel)
                .ToDictionary(x => x.Key, x =>
                {
                    if (x.Master is ComplicateCollectionViewModel masterModel)
                    {
                        return new ReferencableMasterInfo()
                        {
                            Collection = masterModel.Collection.ToReadOnlyReactiveCollection(y => y.Value.Value),
                            Type = x.Property.PropertyType.GetElementType(),
                        };
                    }
                    throw new Exception();
                });

            var globals = GetMastersInfo(masterMembers, false);
            var models = globals.Concat(masters).ToArray();

            return new PropertyRoot(projectType, models.ToArray());
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

        public IEnumerable<IPropertyModel> GetMembersInfo(Type type)
        {
            var properties = type.GetProperties()
                .Select(MakeModelAndInfo)
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
                    references[prop.Title.Value] = refByInt;
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

        private IPropertyModel MakeModelAndInfo(PropertyInfo info)
        {
            IPropertyModel result = null;

            var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
            if (memberAttr != null)
            {
                result = Create(info.PropertyType, memberAttr.Name);
            }

            var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
            if (attr != null)
            {
                result = CreateReference(info.PropertyType, attr.MasterKey, attr.IdFieldName, attr.Name);
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
