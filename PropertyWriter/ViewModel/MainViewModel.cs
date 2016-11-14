using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Livet.Messaging;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
{
	class MainViewModel : Livet.ViewModel
	{
		public ReactiveProperty<string> AssemblyPath { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> StatusMessage { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsError { get; set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<MasterInfo[]> Roots { get; } = new ReactiveProperty<MasterInfo[]>();
		public ReactiveCommand NewProjectCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand LoadDataCommand { get; set; } = new ReactiveCommand();

		public ReactiveProperty<Project> Project { get; } = new ReactiveProperty<Project>();

		public MainViewModel()
		{
			NewProjectCommand.Subscribe(x => CreateNewProject());
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
			var assembly = Project.Value.GetAssembly();
			Roots.Value = modelFactory.LoadStructure(assembly, Project.Value.GetProjectType(assembly));

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

		private void CreateNewProject()
		{
			var project = new Project();
			var vm = new ProjectViewModel(project);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "NewProject"));
			Project.Value = project;

			if (vm.Confirmed.Value)
			{
				var modelFactory = new ModelFactory();
				var assembly = Project.Value.GetAssembly();
				Roots.Value = modelFactory.LoadStructure(assembly, Project.Value.GetProjectType(assembly));

				StatusMessage.Value = "プロジェクトを作成しました。";
				IsError.Value = false;
			}
		}
	}
}
