using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.VisualStudio;

using Thread = System.Threading.Thread;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal sealed class LocateInSourceControlExplorer
	{
		public static LocateInSourceControlExplorer Instance { get; private set; }

		public const int CommandId = 0x0003;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private string SelectedItemPath
		{
			get
			{
				var dte = ServiceProvider.GetDte();

				if (dte.ActiveWindow.ObjectKind != Constants.vsWindowKindSolutionExplorer)
				{
					return dte.ActiveDocument?.FullName;
				}

				if (dte.SelectedItems.Count != 1)
				{
					return null;
				}

				var selectedItem = dte.SelectedItems.Item(1);

				if (selectedItem.Project == null)
				{
					return dte.Solution.FullName;
				}

				if (selectedItem.ProjectItem == null)
				{
					return selectedItem.Project.FullName;
				}

				return selectedItem.ProjectItem.FileNames[1];
			}
		}

		private LocateInSourceControlExplorer(VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetMenuCommandService();

			var command = new OleMenuCommand(CommandCallback, new CommandID(CommandSet.Guid, CommandId));
			command.BeforeQueryStatus += Command_BeforeQueryStatus;
			commandService.AddCommand(command);
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new LocateInSourceControlExplorer(package);
		}

		private void Command_BeforeQueryStatus(object sender, EventArgs e)
		{
			var command = (OleMenuCommand) sender;
			var enabled = ShouldEnableCommand();

			command.Visible = enabled;
			command.Enabled = enabled;
			command.Supported = enabled;
		}

		private bool ShouldEnableCommand()
		{
			var dte = ServiceProvider.GetDte();
			var vc = dte.GetTfsVersionControl();

			var selectedItemPath = SelectedItemPath;

			if (string.IsNullOrEmpty(selectedItemPath))
			{
				return false;
			}

			return vc.SolutionWorkspace?.IsLocalPathMapped(selectedItemPath) == true;
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var vc = dte.GetTfsVersionControl();

			var path = SelectedItemPath;

			var workspace = vc.SolutionWorkspace.VersionControlServer.GetWorkspace(path);
			var serverItem = workspace.GetServerItemForLocalItem(path);

			dte.ExecuteCommand("View.TfsSourceControlExplorer");

			if (vc.Explorer.Connected)
			{
				vc.Explorer.Navigate(serverItem);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() =>
				{
					for (var i = 1; i <= 50; i++)
					{
						Thread.Sleep(100);

						if (!vc.Explorer.Connected)
						{
							continue;
						}

						ThreadHelper.Generic.Invoke(() => vc.Explorer.Navigate(serverItem));

						break;
					}
				});
			}
		 }
	}
}