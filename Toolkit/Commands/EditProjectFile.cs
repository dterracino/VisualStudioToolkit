using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.VisualStudio;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal sealed class EditProjectFile
	{
		public static EditProjectFile Instance { get; private set; }

		public const int CommandId = 0x0005;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private EditProjectFile(VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetMenuCommandService();
			var command = new OleMenuCommand(CommandCallback, new CommandID(CommandSet.Guid, CommandId));
			commandService.AddCommand(command);
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new EditProjectFile(package);
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var projectPath = dte.SelectedItems.Item(1).Project.FullName;

			dte.ExecuteCommand("Project.UnloadProject");
			dte.ItemOperations.OpenFile(projectPath, ViewKind.Text);
		}
	}
}