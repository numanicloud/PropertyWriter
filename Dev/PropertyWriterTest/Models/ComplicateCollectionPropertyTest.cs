using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;

namespace PropertyWriterTest.Models
{
    [TestClass]
    public class ComplicateCollectionPropertyTest
    {
        [TestMethod]
        public void AddNewElementTest()
        {
            var property = new ComplicateCollectionProperty(typeof(TestClass[]), new PropertyFactory());
            property.AddNewElement();

            property.Collection.ToArray().Length.Is(1);
            property.Collection[0].Title.Value.Is(CollectionHolder.ElementTitle);
            property.Collection[0].IsInstanceOf<ClassProperty>();

            var e1 = property.Collection[0] as ClassProperty;
            e1.Members.Length.Is(2);
            e1.Members[0].IsInstanceOf<IntProperty>();
            e1.Members[1].IsInstanceOf<StringProperty>();
        }

        [TestMethod]
        public void RemoveElementAtTest()
        {
            var property = new ComplicateCollectionProperty(typeof(TestClass[]), new PropertyFactory());
            var ee1 = property.AddNewElement() as ClassProperty;
            var ee2 = property.AddNewElement() as ClassProperty;
            (ee1.Members[0] as IntProperty).IntValue.Value = 1;
            (ee2.Members[0] as IntProperty).IntValue.Value = 2;

            property.RemoveElementAt(0);
            property.Collection.Count.Is(1);

            var e1 = property.Collection[0] as ClassProperty;
            var e1m1 = e1.Members[0] as IntProperty;
            e1m1.IntValue.Value.Is(2);
        }
    }
}
