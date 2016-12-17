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

namespace PropertyWriter.ViewModels.ProjectSetting
{
	class ProjectSettingViewModel : Livet.ViewModel
	{
		private readonly Project project_;
		public ReactiveProperty<string> AssemblyPath => project_.AssemblyPath;
		public ReactiveProperty<string> ProjectTypeName => project_.ProjectTypeName;
		public ReactiveProperty<string> ExportPath => project_.SavePath;
		public ReactiveProperty<bool> IsValid => project_.IsValid;
        
        public ReactiveCommand OpenAssemblyCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SelectExportPathCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<IEnumerable<Type>> AvailableProjectTypes { get; } = new ReactiveProperty<IEnumerable<Type>>();
        public ReactiveProperty<Type> ProjectType { get; } = new ReactiveProperty<Type>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public ProjectSettingViewModel(Project project)
		{
			project_ = project;

			ProjectType.Subscribe(x => project_.ProjectTypeName.Value = x?.Name);
			OpenAssemblyCommand.Subscribe(x => OpenAssembly());
			SelectExportPathCommand.Subscribe(x => SelectExportPath());

            SetAvailableProjectTypes();
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
