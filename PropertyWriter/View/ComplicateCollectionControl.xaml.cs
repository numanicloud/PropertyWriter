using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyWriter.Model;

namespace PropertyWriter.View
{
	/// <summary>
	/// ComplicateCollectionControl.xaml の相互作用ロジック
	/// </summary>
	public partial class ComplicateCollectionControl : UserControl
	{
		public ComplicateCollectionControl()
		{
			InitializeComponent();
		}

		private ComplicateCollectionInstance property
		{
			get { return DataContext as ComplicateCollectionInstance; }
		}

		private void addButton_Click( object sender, RoutedEventArgs e )
		{
			property.AddNewProperty();
		}

		private void removeButton_Click( object sender, RoutedEventArgs e )
		{
			var index = dataList.SelectedIndex;
			if( index != -1 )
			{
				property.RemoveAt( index );
				propertyContent.Content = null;
				if( index < dataList.Items.Count )
				{
					dataList.SelectedIndex = index;
				}
			}
		}

		private void dataList_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			var index = dataList.SelectedIndex;
			if( index != -1 )
			{
				propertyContent.Content = property.Collection[dataList.SelectedIndex];
			}
		}
	}
}
