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

namespace PropertyWriter
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class TypeSelectWindow : Window
	{
		public TypeSelectWindow()
		{
			InitializeComponent();
			viewmodel = new TypeSelectViewModel();
			DataContext = viewmodel;
			TargetType = null;
		}

		TypeSelectViewModel viewmodel { get; set; }

		public Type TargetType { get; private set; }

		private void Button_Click( object sender, RoutedEventArgs e )
		{
			TargetType = viewmodel.Types[viewmodel.SelectedTypeIndex];
			Close();
		}
	}
}
