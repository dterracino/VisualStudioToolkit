using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using TheSolutionEngineers.Toolkit.Extensions;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal sealed class BreakAllInCurrentDocument
	{
		public static BreakAllInCurrentDocument Instance { get; private set; }

		public const int CommandId = 0x0001;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private BreakAllInCurrentDocument(VisualStudioPackage package)
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
			Instance = new BreakAllInCurrentDocument(package);
		}

		private void Command_BeforeQueryStatus(object sender, EventArgs e)
		{
			var command = (OleMenuCommand)sender;
			var dte = ServiceProvider.GetDte();

			var currentlyRunning = (dte.Debugger.CurrentMode == dbgDebugMode.dbgRunMode);

			command.Enabled = currentlyRunning;
			command.Supported = currentlyRunning;
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();

			var activeWindowBefore = dte.ActiveWindow;

			dte.Debugger.Break();

			var activeWindowAfter = dte.ActiveWindow;

			if (activeWindowBefore == activeWindowAfter)
			{
				return;
			}

			if (activeWindowAfter.Document == null)
			{
				activeWindowAfter.Close();
			}

			activeWindowBefore.Activate();
		}
	}
}