using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.ViewModels;

namespace PropertyWriter.Models.Editor
{
	public interface IEditorViewModel
	{
		(bool isCommited, Project result) CreateNewProject();
		(bool isCommited, Project result) RepairProject(Project project, string message);
		ClosingResult ConfirmCloseProject();
		Task TerminateAsync();
	}
}
