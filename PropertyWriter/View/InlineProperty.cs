using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PropertyWriter.View
{
	partial class InlineProperty : ResourceDictionary
	{
		public void OpenBasicCollection( object sender, RoutedEventArgs e )
		{
			var window = new BasicCollectionWindow()
			{
				DataContext = ( sender as FrameworkElement ).DataContext
			};
			window.ShowDialog();
		}
	}
}
