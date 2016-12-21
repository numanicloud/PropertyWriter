using Livet.Messaging.Windows;
using PropertyWriter.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.ProjectSetting
{
    class ProjectRepairViewModel : Livet.ViewModel
    {
        private Project clone_;

        public ProjectSettingViewModel ProjectSetting { get; }
        public ReactiveCommand CommitCommand { get; }
        public ReactiveProperty<string> Message { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<bool> IsCommitted { get; } = new ReactiveProperty<bool>(false);
        public Project Result { get; private set; }

        public ProjectRepairViewModel(Project project, string message)
        {
            clone_ = project.Clone();
            ProjectSetting = new ProjectSettingViewModel(clone_);
            Message.Value = message + "プロジェクト型を元の型に設定しなおしてください。必要であればアセンブリを設定しなおしてください。";

            CommitCommand = clone_.IsValid.ToReactiveCommand();
            CommitCommand.SelectMany(x => CommitAsync().ToObservable())
                .SafelySubscribe(ex => { });
        }

        private async Task CommitAsync()
        {
            try
            {
                await clone_.LoadDataAsync();
            }
            catch (Models.Exceptions.PwObjectMissmatchException)
            {
                Messenger.Raise(new Livet.Messaging.ConfirmationMessage(
                    "プロジェクト型情報が一致しません。元の型と同じ型を指定してください。",
                    "エラー"));
                return;
            }

            Result = new Project(clone_);
            IsCommitted.Value = true;
            Messenger.Raise(new WindowActionMessage(WindowAction.Close));
        }
    }
}
