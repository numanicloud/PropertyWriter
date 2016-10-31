using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using MvvmHelper;
using System.ComponentModel;
using System.Windows.Forms;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
{
	class MainViewModel
	{
		public ReactiveProperty<MasterInfo[]> Roots { get; } = new ReactiveProperty<MasterInfo[]>();
		public ReactiveCommand NewFileCommand { get; set; } = new ReactiveCommand();

		public MainViewModel()
		{
			NewFileCommand.Subscribe(x => OnNewFile());
		}

		private void OnNewFile()
		{
			var dialog = new OpenFileDialog
			{
				FileName = "",
				Filter = "アセンブリ ファイル (*.dll, *.exe)|*.dll;*.exe",
				Title = "アセンブリを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Roots.Value = DllLoader.LoadDataTypes(dialog.FileName)
					.Select(x =>
					{
						var master = typeof (IEnumerable<>).MakeGenericType(x);
						var masterModel = InstanceFactory.Create(master);
						return new MasterInfo(x.Name, masterModel);
					})
					.ToArray();
			}
		}
	}
}
