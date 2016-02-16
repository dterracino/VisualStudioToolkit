using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit
{
	internal sealed class BreakInCurrentDocument
	{
		public static readonly Guid CommandSet = new Guid("c85a85a9-78cb-4a7c-97bf-d246bc680366");
		public static BreakInCurrentDocument Instance { get; private set; }

		public const int ToolbarCommandId = 0x0001;
		public const int MenuCommandId = 0x0002;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private BreakInCurrentDocument(VisualStudioPackage package)
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

			commandService.AddCommand(new MenuCommand(CommandCallback, new CommandID(CommandSet, ToolbarCommandId)));
			commandService.AddCommand(new MenuCommand(CommandCallback, new CommandID(CommandSet, MenuCommandId)));
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new BreakInCurrentDocument(package);
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetService(typeof(DTE)) as DTE;

			if (dte == null)
			{
				throw new ServiceUnavailableException(typeof(DTE));
			}

			var activeWindow = dte.ActiveWindow;
			var windowCount = dte.Windows.Count;

			dte.Debugger.Break();

			if (activeWindow == dte.ActiveWindow)
			{
				return;
			}

			if (dte.Windows.Count > windowCount)
			{
				dte.ActiveWindow.Close();
			}
			else
			{
				activeWindow.Activate();
			}
		}
	}
}