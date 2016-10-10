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
	/// BasicCollectionControl.xaml の相互作用ロジック
	/// </summary>
	public partial class BasicCollectionControl : UserControl
	{
		public BasicCollectionControl()
		{
			InitializeComponent();
		}

		private BasicCollectionInstance property
		{
			get { return DataContext as BasicCollectionInstance; }
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
				if( index < dataList.Items.Count )
				{
					dataList.SelectedIndex = index;
				}
			}
		}
	}
}
