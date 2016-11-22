using PropertyWriter.Model.Instance;
using PropertyWriter.ViewModel;
using PropertyWriter.ViewModel.Instance;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
    class ModelConverter
    {
        public static async Task LoadValueToRoot(RootViewModel root, object obj)
        {
            var references = new List<(ReferenceByIntModel model, int id)>();
            foreach (var p in root.Structure.Properties)
            {
                var value = p.GetValue(obj);
                LoadValueToModel(p.Model, value, references);
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
            List<(ReferenceByIntModel reference, int id)> references)
        {
            switch (model)
            {
            case IntModel m:
                m.IntValue.Value = (int)value;
                break;
            case StringModel m:
                m.StringValue.Value = (string)value;
                break;
            case BoolModel m:
                m.BoolValue.Value = (bool)value;
                break;
            case FloatModel m:
                m.FloatValue.Value = (float)value;
                break;
            case EnumModel m:
                ConvertEnum(m, value);
                break;
            case ClassModel m:
                LoadObjectToModel(m, value, references);
                break;
            case StructModel m:
                LoadObjectToModel(m, value, references);
                break;
            case ReferenceByIntModel m:
                references.Add((m, (int)value));
                break;
            case SubtypingModel m:
                foreach (var type in m.AvailableTypes)
                {
                    if (type.Type == value.GetType())
                    {
                        m.SelectedType.Value = type;
                        LoadObjectToModel((IStructureModel)m.Model, value, references);
                    }
                }
                break;
            case ComplicateCollectionModel m:
                LoadCollectionToModel(m, value, references);
                break;
            case BasicCollectionModel m:
                LoadCollectionToModel(m, value, references);
                break;
            default:
                throw new Exception("開発者向け：不正なModelです。");
            }
        }

        private static void LoadCollectionToModel(
            ICollectionModel model,
            object value,
            List<(ReferenceByIntModel reference, int id)> references)
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
            IStructureModel model,
            object structureValue,
            List<(ReferenceByIntModel reference, int id)> references)
        {
            var members = model.Members;
            foreach (var property in members)
            {
                object value = null;

                try
                {
                    value = property.GetValue(structureValue);
                }
                catch (ArgumentException)
                {
                    throw new PwObjectMissmatchException(model.Type.Name, property.MemberName);
                }

                LoadValueToModel(property.Model, value, references);
            }
        }

        private static void ConvertEnum(EnumModel model, object obj)
        {
            var val = model.EnumValues.FirstOrDefault(x => x.ToString() == model.Type.GetEnumName(obj));
            model.EnumValue.Value = val;
        }
    }
}
