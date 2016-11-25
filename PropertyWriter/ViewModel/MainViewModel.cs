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
		public ReactiveProperty<IPropertyViewModel[]> Masters { get; }

		public ReactiveCommand NewProjectCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand OpenProjectCommand { get; set; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; set; }

		public MainViewModel()
		{
			SaveCommand = Project.Select(x => x?.IsValid?.Value == true)
				.ToReactiveCommand();
			Masters = Project.Where(x => x != null)
                .SelectMany(x => x.Root)
                .Where(x => x != null)
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
            SubscribeSaveCommand();

			IsError.Value = false;
		}

        private void SubscribeSaveCommand()
        {
            SaveCommand.SelectMany(x => SaveFile().ToObservable())
                .Subscribe(
                    unit => { },
                    exception =>
                    {
                        StatusMessage.Value = $"保存を中止し、以前のファイルに復元しました。{exception.Message}";
                        IsError.Value = true;
                        SubscribeSaveCommand();
                    }); ;
        }

        private void SubscribeOpenCommand()
		{
			OpenProjectCommand.SelectMany(x => OpenProject().ToObservable())
				.Subscribe(
					unit => { },
					exception =>
					{
						StatusMessage.Value = $"データを読み込めませんでした。{exception.Message}";
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
                Project.Value.Initialize();

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

                Project.Value = await Model.Project.Load(dialog.FileName);

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
            
            await Project.Value.Save(ProjectPath.Value);

			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
		}
	}
}
