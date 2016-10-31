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
			var instance = ( sender as FrameworkElement ).DataContext as Model.BasicCollectionModel;
			var window = new BlockWindow()
			{
				Title = "Collection",
				DataContext = instance
			};
			window.ShowDialog();
		}

		public void OpenComplicateCollection(object sender, RoutedEventArgs e)
		{
			var instance = ( sender as FrameworkElement ).DataContext as Model.ComplicateCollectionModel;
			var window = new BlockWindow()
			{
				Title = "Collection",
				DataContext = instance
			};
			window.ShowDialog();
		}

		public void OpenBlockWindow( object sender, RoutedEventArgs e )
		{
			var instance = ( sender as FrameworkElement ).DataContext;
			var window = new BlockWindow()
			{
				Title = instance.GetType().Name,
				DataContext = instance
			};
			window.ShowDialog();
		}
	}
}
