using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows.Forms;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
{
	class MainViewModel
	{
		public ReactiveProperty<string> AssemblyPath { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> StatusMessage { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsError { get; set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<MasterInfo[]> Roots { get; } = new ReactiveProperty<MasterInfo[]>();
		public ReactiveCommand NewFileCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand LoadDataCommand { get; set; } = new ReactiveCommand();

		public MainViewModel()
		{
			NewFileCommand.Subscribe(x => OnNewFile());
			SaveCommand.SelectMany(x => SaveFile().ToObservable()).Subscribe();
			SubscribeLoadDataCommand();
			IsError.Value = false;
		}

		private void SubscribeLoadDataCommand()
		{
			LoadDataCommand.SelectMany(x => LoadData().ToObservable())
				.Subscribe(
					unit => { },
					exception =>
					{
						StatusMessage.Value = $"データを完全には読み込めませんでした。{exception.Message}";
						IsError.Value = true;
						SubscribeLoadDataCommand();
					});
		}

		private async Task LoadData()
		{
			var modelFactory = new ModelFactory();
			Roots.Value = modelFactory.LoadData(AssemblyPath.Value);
			StatusMessage.Value = "データを読み込み中…";
			var result = await JsonSerializer.LoadData(Roots.Value, "SaveData/");
			StatusMessage.Value = "データを読み込みました。";
			IsError.Value = false;
		}

		private async Task SaveFile()
		{
			StatusMessage.Value = "データを保存中…";
			await JsonSerializer.SaveData(Roots.Value, "SaveData/");
			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
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
				var modelFactory = new ModelFactory();
				Roots.Value = modelFactory.LoadData(dialog.FileName);
				AssemblyPath.Value = dialog.FileName;
				StatusMessage.Value = "DLLを読み込みました。";
				IsError.Value = false;
			}
		}
	}
}
