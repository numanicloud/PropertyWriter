using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;

namespace PropertyWriterTest
{
    [TestClass]
    public class BasicCollectionPropertyTest
    {
        [TestMethod]
        public void AddNewElementTest()
        {
            var property = new BasicCollectionProperty(typeof(int[]), new PropertyFactory());
            property.AddNewElement();

            var expected = new IPropertyModel[]
            {
                new IntProperty()
                {
                    Title = { Value = CollectionHolder.ElementTitle }
                }
            };
            property.Collection.ToArray()
                .IsStructuralEqual(expected);
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            var property = new BasicCollectionProperty(typeof(int[]), new PropertyFactory());

            var e1 = property.AddNewElement() as IntProperty;
            var e2 = property.AddNewElement() as IntProperty;
            e1.IntValue.Value = 1;
            e2.IntValue.Value = 2;
            property.RemoveAt(0);

            var expected = new IPropertyModel[]
            {
                new IntProperty()
                {
                    IntValue = { Value = 2 },
                    Title = { Value = CollectionHolder.ElementTitle },
                }
            };
            property.Collection.ToArray()
                .IsStructuralEqual(expected);
        }

		[TestMethod]
		public void RemoveFromEmptyTest()
		{
			var prop = new BasicCollectionProperty(typeof(int[]), new PropertyFactory());
			prop.RemoveAt(-1);
		}
    }
}
