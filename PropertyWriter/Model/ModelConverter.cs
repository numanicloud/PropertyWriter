using PropertyWriter.Model.Interfaces;
using PropertyWriter.Model.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
    class ModelConverter
    {
        public static async Task LoadValueToRootAsync(PropertyRoot root, object obj)
        {
            var references = new List<(ReferenceByIntProperty model, int id)>();
            foreach (var p in root.Structure.Properties)
            {
                var value = p.PropertyInfo.GetValue(obj);
                LoadValueToModel(p, value, references);
            }
            await Task.Delay(100);
            foreach (var reference in references)
            {
                reference.model.SetItemById(reference.id);
            }
        }

        private static void LoadValueToModel(
            IPropertyModel model,
            object value,
            List<(ReferenceByIntProperty reference, int id)> references)
        {
            switch (model)
            {
            case IntProperty m:
                m.IntValue.Value = (int)value;
                break;
            case StringProperty m:
                m.StringValue.Value = (string)value;
                break;
            case BoolProperty m:
                m.BoolValue.Value = (bool)value;
                break;
            case FloatProperty m:
                m.FloatValue.Value = (float)value;
                break;
            case EnumProperty m:
                ConvertEnum(m, value);
                break;
            case ClassProperty m:
                LoadObjectToModel(m, value, references);
                break;
            case StructProperty m:
                LoadObjectToModel(m, value, references);
                break;
            case ReferenceByIntProperty m:
                references.Add((m, (int)value));
                break;
            case SubtypingProperty m:
                foreach (var type in m.AvailableTypes)
                {
                    if (type.Type == value.GetType())
                    {
                        m.SelectedType.Value = type;
                        LoadObjectToModel((IStructureProperty)m.Model, value, references);
                    }
                }
                break;
            case ComplicateCollectionProperty m:
                LoadCollectionToModel(m, value, references);
                break;
            case BasicCollectionProperty m:
                LoadCollectionToModel(m, value, references);
                break;
            default:
                throw new Exception("開発者向け：不正なModelです。");
            }
        }

        private static void LoadCollectionToModel(
            ICollectionProperty model,
            object value,
            List<(ReferenceByIntProperty reference, int id)> references)
        {
            if (value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    var element = model.AddNewElement();
                    LoadValueToModel(element, item, references);
                }
            }
            else
            {
                throw new ArgumentException("開発者向け：コレクションかどうかの判定が間違っています。", nameof(value));
            }
        }

        private static void LoadObjectToModel(
            IStructureProperty model,
            object structureValue,
            List<(ReferenceByIntProperty reference, int id)> references)
        {
            var members = model.Members;
            foreach (var property in members)
            {
                object value = null;

                try
                {
                    value = property.PropertyInfo.GetValue(structureValue);
                }
                catch (ArgumentException)
                {
                    throw new PwObjectMissmatchException(model.Type.Name, property.Title.Value);
                }

                LoadValueToModel(property, value, references);
            }
        }

        private static void ConvertEnum(EnumProperty model, object obj)
        {
            var val = model.EnumValues.FirstOrDefault(x => x.ToString() == model.Type.GetEnumName(obj));
            model.EnumValue.Value = val;
        }
    }
}
