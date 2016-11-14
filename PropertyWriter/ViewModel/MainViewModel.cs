using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Livet.Messaging;
using Newtonsoft.Json;
using PropertyWriter.Model.Instance;
using Reactive.Bindings;
using JsonSerializer = PropertyWriter.Model.JsonSerializer;

namespace PropertyWriter.ViewModel
{
	class MainViewModel : Livet.ViewModel
	{
		public ReactiveProperty<string> StatusMessage { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsError { get; set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<string> ProjectPath { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsReady { get; set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<string> Title { get; set; }

		public ReactiveProperty<Project> Project { get; } = new ReactiveProperty<Project>();
		public ReactiveProperty<RootViewModel> Root { get; } = new ReactiveProperty<RootViewModel>();
		public ReactiveProperty<IPropertyModel[]> Masters { get; }

		public ReactiveCommand NewProjectCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand OpenProjectCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; set; }

		public MainViewModel()
		{
			SaveCommand = Project.Select(x => x?.IsValid?.Value == true)
				.ToReactiveCommand();
			Masters = Root.Where(x => x != null)
				.Select(x => x.Structure.Properties.Select(y => y.Model).ToArray())
				.ToReactiveProperty();

			var notReady = IsReady.Where(x => !x)
				.Select(x => "PropertyWriter");
			Title = ProjectPath.Where(x => IsReady.Value)
				.Select(x => x ?? "新規プロジェクト")
				.Select(x => $"PropertyWriter - {x}")
				.Merge(notReady)
				.ToReactiveProperty();

			NewProjectCommand.Subscribe(x => CreateNewProject());
			SubscribeOpenCommand();
			SaveCommand.SelectMany(x => SaveFile().ToObservable()).Subscribe();

			IsError.Value = false;
		}

		private void SubscribeOpenCommand()
		{
			OpenProjectCommand.SelectMany(x => OpenProject().ToObservable())
				.Subscribe(
					unit => { },
					exception =>
					{
						StatusMessage.Value = $"データを完全には読み込めませんでした。{exception.Message}";
						IsError.Value = true;
						SubscribeOpenCommand();
					});
		}

		private void CreateNewProject()
		{
			var project = new Project();
			var vm = new ProjectViewModel(project);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "NewProject"));

			if (vm.Confirmed.Value)
			{
				Project.Value = project;

				var modelFactory = new ModelFactory();
				var assembly = Project.Value.GetAssembly();
				Root.Value = modelFactory.LoadStructure(assembly, Project.Value.GetProjectType(assembly));

				StatusMessage.Value = "プロジェクトを作成しました。";
				IsError.Value = false;
				IsReady.Value = true;
				ProjectPath.Value = null;
			}
		}

		private async Task OpenProject()
		{
			var dialog = new OpenFileDialog()
			{
				FileName = "",
				Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
				Title = "マスターデータ プロジェクトを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				StatusMessage.Value = "プロジェクトを読み込み中…";

				using (var file = new StreamReader(dialog.FileName))
				{
					Project.Value = JsonConvert.DeserializeObject<Project>(await file.ReadToEndAsync());
				}

				var modelFactory = new ModelFactory();
				var assembly = Project.Value.GetAssembly();
				Root.Value = modelFactory.LoadStructure(assembly, Project.Value.GetProjectType(assembly));

				await JsonSerializer.LoadData(Root.Value, Project.Value.SavePath.Value);

				StatusMessage.Value = "プロジェクトを読み込みました。";
				IsError.Value = false;
				IsReady.Value = true;
				ProjectPath.Value = dialog.FileName;
			}
		}

		private async Task SaveFile()
		{
			if (ProjectPath.Value == null)
			{
				var dialog = new SaveFileDialog()
				{
					FileName = "NewProject.pwproj",
					Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
					Title = "マスター プロジェクトを保存"
				};
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					ProjectPath.Value = dialog.FileName;
				}
			}

			if (ProjectPath.Value == null)
			{
				return;
			}

			StatusMessage.Value = "データを保存中…";

			using (var file = new StreamWriter(ProjectPath.Value))
			{
				var json = JsonConvert.SerializeObject(Project.Value);
				await file.WriteLineAsync(json);
			}
			await JsonSerializer.SaveData(Root.Value, Project.Value.SavePath.Value);

			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
		}
	}
}
