using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit
{
	internal sealed class BreakAllInCurrentDocument
	{
		public static readonly Guid CommandSet = new Guid("c85a85a9-78cb-4a7c-97bf-d246bc680366");
		public static BreakAllInCurrentDocument Instance { get; private set; }

		public const int ToolbarCommandId = 0x0001;
		public const int MenuCommandId = 0x0002;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private BreakAllInCurrentDocument(VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

			if (commandService == null)
			{
				throw new ServiceUnavailableException(typeof(IMenuCommandService));
			}

			var commands = new[]
			{
				new OleMenuCommand(CommandCallback, new CommandID(CommandSet, ToolbarCommandId)), 
				new OleMenuCommand(CommandCallback, new CommandID(CommandSet, MenuCommandId)), 
			};

			foreach (var command in commands)
			{
				command.BeforeQueryStatus += Command_BeforeQueryStatus;
				commandService.AddCommand(command);
			}
		}

		private void Command_BeforeQueryStatus(object sender, EventArgs e)
		{
			var command = (OleMenuCommand) sender;

			var dte = ServiceProvider.GetService(typeof (DTE)) as DTE;

			if (dte == null)
			{
				throw new ServiceUnavailableException(typeof(DTE));
			}

			var currentlyRunning = (dte.Debugger.CurrentMode == dbgDebugMode.dbgRunMode);

			command.Enabled = currentlyRunning;
			command.Supported = currentlyRunning;
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new BreakAllInCurrentDocument(package);
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetService(typeof(DTE)) as DTE;

			if (dte == null)
			{
				throw new ServiceUnavailableException(typeof(DTE));
			}

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