using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PropertyWriter.Views.Properties
{
	partial class InlineProperty : ResourceDictionary
	{
		public void SelectAll(object sender, EventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
	}
}
