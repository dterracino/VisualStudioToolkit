using System;
using System.ComponentModel.Design;
using System.IO;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.VisualStudio;

namespace TheSolutionEngineers.Toolkit.Commands
{
	public sealed class CompareFiles
	{
		public static CompareFiles Instance { get; private set; }

		public const int CommandId = 0x0006;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private CompareFiles(VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetMenuCommandService();
			var command = new OleMenuCommand(CommandCallback, new CommandID(CommandSet.Guid, CommandId));
			command.BeforeQueryStatus += CommandOnBeforeQueryStatus;
			commandService.AddCommand(command);
		}

		private void CommandOnBeforeQueryStatus(object sender, EventArgs eventArgs)
		{
			var command = (OleMenuCommand)sender;
			var dte = ServiceProvider.GetDte();

			var hasSelectedTwoFiles = (dte.SelectedItems.Count == 2);

			command.Visible = hasSelectedTwoFiles;
			command.Supported = hasSelectedTwoFiles;
			command.Enabled = hasSelectedTwoFiles;
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new CompareFiles(package);
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();

			var firstFilePath = dte.SelectedItems.Item(1).ProjectItem.FileNames[1];
			var secondFilePath = dte.SelectedItems.Item(2).ProjectItem.FileNames[1];

			Difference.VisualDiffFiles(firstFilePath, secondFilePath, Path.GetDirectoryName(firstFilePath), Path.GetDirectoryName(secondFilePath), Path.GetFileName(firstFilePath), Path.GetFileName(secondFilePath), false, false);
		}
	}
}
