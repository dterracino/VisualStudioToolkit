using System;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.VisualStudio;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal sealed class EditSolutionFile
	{
		public static EditSolutionFile Instance { get; private set; }

		public const int CommandId = 0x0004;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private EditSolutionFile(VisualStudioPackage package)
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
			Instance = new EditSolutionFile(package);
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var solutionPath = dte.Solution.FullName;

			dte.ExecuteCommand("File.CloseSolution");
			dte.ItemOperations.OpenFile(solutionPath, ViewKind.Text);
		}
	}
}