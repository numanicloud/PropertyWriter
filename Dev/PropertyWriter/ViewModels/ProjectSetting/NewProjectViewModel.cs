using Livet.Messaging.Windows;
using PropertyWriter.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.ProjectSetting
{
    class NewProjectViewModel : Livet.ViewModel
    {
		public Project Project { get; }
        public ProjectSettingViewModel ProjectSetting { get; }
        public ReactiveProperty<bool> IsCommitted { get; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand CommitCommand { get; }
        
        public NewProjectViewModel()
        {
			Project = new Project();
            ProjectSetting = new ProjectSettingViewModel(Project);
            CommitCommand = Project.IsValid.ToReactiveCommand();
            CommitCommand.Subscribe(x => Commit());
        }

        private void Commit()
        {
            IsCommitted.Value = true;
            Messenger.Raise(new WindowActionMessage(WindowAction.Close));
        }
    }
}
