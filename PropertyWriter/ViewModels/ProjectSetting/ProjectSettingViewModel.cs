using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Livet.Messaging.Windows;
using PropertyWriter.Annotation;
using PropertyWriter.Models;
using Reactive.Bindings;
using System.Windows;
using System.Reactive.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace PropertyWriter.ViewModels.ProjectSetting
{
	class ProjectSettingViewModel : Livet.ViewModel
	{
		private readonly Project project_;
		public ReactiveProperty<string> AssemblyPath => project_.AssemblyPath;
		public ReactiveProperty<string> ProjectTypeName => project_.ProjectTypeName;
		public ReactiveProperty<string> ExportPath => project_.SavePath;
		public ObservableCollection<ReactiveProperty<string>> DependenciesPathes => project_.DependenciesPathes;
		public ReactiveProperty<bool> IsValid => project_.IsValid;
        
        public ReactiveCommand OpenAssemblyCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SelectExportPathCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> ReferenceDependencyCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand AddDependencyCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveDependencyCommand { get; } = new ReactiveCommand<int>();
		public ReactiveProperty<IEnumerable<Type>> AvailableProjectTypes { get; } = new ReactiveProperty<IEnumerable<Type>>();
        public ReactiveProperty<Type> ProjectType { get; } = new ReactiveProperty<Type>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public ProjectSettingViewModel(Project project)
		{
			project_ = project;

			ProjectType.Subscribe(x => project_.ProjectTypeName.Value = x?.Name);
			OpenAssemblyCommand.Subscribe(x => OpenAssembly());
			SelectExportPathCommand.Subscribe(x => SelectExportPath());

			ReferenceDependencyCommand.Where(x => x >= 0).Subscribe(i => InputReference(i));
			AddDependencyCommand.Subscribe(i => AddDependency());
			RemoveDependencyCommand.Where(x => x >= 0).Subscribe(i => RemoveDependency(i));

            SetAvailableProjectTypes();
		}

		private void RemoveDependency(int i)
		{
			DependenciesPathes.RemoveAt(i);
		}

		private void AddDependency()
		{
			DependenciesPathes.Add(new ReactiveProperty<string>("C:/Hoge.pwproj"));
		}

		private void InputReference(int i)
		{
			var dialog = new OpenFileDialog()
			{
				Filter = "PropertyWriter プロジェクト (*.pwproj)|*.pwproj",
				Title = "参照するプロジェクトを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				DependenciesPathes[i].Value = dialog.FileName;
			}
		}

		private void OpenAssembly()
		{
			var dialog = new OpenFileDialog
			{
				FileName = "",
				Filter = "アセンブリ ファイル (*.dll, *.exe)|*.dll;*.exe",
				Title = "アセンブリを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
            {
                AssemblyPath.Value = dialog.FileName;
                SetAvailableProjectTypes();
            }
        }

        private void SetAvailableProjectTypes()
        {
            if (project_.AssemblyPath.Value != null && File.Exists(project_.AssemblyPath.Value))
            {
                AvailableProjectTypes.Value = project_.GetAssembly().GetTypes()
                    .Where(x => x.GetCustomAttribute<PwProjectAttribute>() != null)
                    .ToArray();
                if (ProjectTypeName.Value != null)
                {
                    ProjectType.Value = AvailableProjectTypes.Value.FirstOrDefault(x => x.Name == ProjectTypeName.Value);
                }
            }
        }

        private void SelectExportPath()
		{
			var dialog = new SaveFileDialog()
			{
				FileName = "DataBase.json",
				Filter = "任意の種類 (*.*)|*.*",
				Title = "アセンブリを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				ExportPath.Value = dialog.FileName;
			}
		}
	}
}
