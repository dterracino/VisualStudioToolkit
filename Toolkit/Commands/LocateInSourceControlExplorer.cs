using System;
using System.ComponentModel.Design;
using System.Threading;

using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.Extensions;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal sealed class LocateInSourceControlExplorer
	{
		public static LocateInSourceControlExplorer Instance { get; private set; }

		public const int CommandId = 0x0003;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

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
			var dte = ServiceProvider.GetDte();
			var vc = dte.GetTfsVersionControl();

			var enabled = vc.SolutionWorkspace.IsLocalPathMapped(dte.ActiveDocument.FullName);

			command.Visible = enabled;
			command.Enabled = enabled;
			command.Supported = enabled;
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var vc = dte.GetTfsVersionControl();

			var workspace = vc.SolutionWorkspace.VersionControlServer.GetWorkspace(dte.ActiveDocument.FullName);
			var serverItem = workspace.GetServerItemForLocalItem(dte.ActiveDocument.FullName);

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