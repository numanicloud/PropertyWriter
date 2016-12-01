﻿using Livet;
using Livet.Messaging;
using Livet.Messaging.Windows;
using PropertyWriter.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyWriter.ViewModels.Editor
{
	class EditorLifecycleManager
	{
		public ReactiveProperty<Project> Project { get; set; } = new ReactiveProperty<Models.Project>();
		public ReactiveProperty<bool> IsError { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<string> StatusMessage { get; private set; } = new ReactiveProperty<string>();
		public MainViewModel Owner { get; private set; }

		public EditorLifecycleManager(MainViewModel owner)
		{
			Owner = owner;
		}


		public bool CreateNewProject()
		{
			var project = new Project();
			var vm = new NewProjectViewModel(project);
			Owner.Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "NewProject"));

			if (vm.Confirmed.Value)
			{
				Project.Value = project;
				Project.Value.InitializeRoot();

				StatusMessage.Value = "プロジェクトを作成しました。";
				IsError.Value = false;
				return true;
			}

			return false;
		}

		public async Task<string> OpenProjectAsync()
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

				var project = await Models.Project.LoadSettingAsync(dialog.FileName);
				try
				{
					await project.LoadDataAsync();
				}
				catch (Models.Exceptions.PwProjectException ex)
				{
					var vm = new ProjectTypeViewModel(project, ex.Message);
					Owner.Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "MissingProjectType"));
					if (!vm.Confirmed.Value)
					{
						return null;
					}
				}

				Project.Value = project;
				StatusMessage.Value = "プロジェクトを読み込みました。";
				IsError.Value = false;
				return dialog.FileName;
			}
			return null;
		}

		public async Task SaveFileAsync(string path)
		{
			StatusMessage.Value = "データを保存中…";

			await Project.Value.SaveSettingAsync(path);
			await Project.Value.SaveDataAsync();

			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
		}

		public async Task<string> SaveFileAsAsync()
		{
			var dialog = new SaveFileDialog()
			{
				FileName = "NewProject.pwproj",
				Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
				Title = "マスター プロジェクトを保存"
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				StatusMessage.Value = "データを保存中…";

				await Project.Value.SaveSettingAsync(dialog.FileName);
				await Project.Value.SaveDataAsync();

				StatusMessage.Value = "データを保存しました。";
				IsError.Value = false;
				return dialog.FileName;
			}

			return null;
		}

		public ClosingViewModel.Result ConfirmClose()
		{
			var vm = new ClosingViewModel();
			var message = new TransitionMessage(vm, "ConfirmClose");
			Owner.Messenger.Raise(message);
			return vm.Response;
		}

		public async Task CloseAsync()
		{
			await DispatcherHelper.UIDispatcher.InvokeAsync(() =>
			{
				Owner.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
			});
		}
	}
}
