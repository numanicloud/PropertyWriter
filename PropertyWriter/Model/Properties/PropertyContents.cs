using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class PropertyContents
    {
        private PropertyInfo Info { get; set; }
        public IPropertyModel Model { get; private set; }

        public PropertyContents(PropertyInfo info, IPropertyModel model)
        {
            Info = info;
            Model = model;
        }

        public string MemberName => Info.Name;
        public object GetValue(object obj) => Info.GetValue(obj);
        public void SetValue(object obj, object value) => Info.SetValue(obj, value);
    }
}
