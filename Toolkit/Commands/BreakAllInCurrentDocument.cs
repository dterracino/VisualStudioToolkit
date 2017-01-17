using System;
using EnvDTE;

using ThijsTijsma.VisualStudioToolkit.VisualStudio;

namespace ThijsTijsma.VisualStudioToolkit.Commands
{
	internal sealed class BreakAllInCurrentDocument : SingleCommand<BreakAllInCurrentDocument>
	{
		private BreakAllInCurrentDocument(int commandId, VisualStudioPackage package) : base(commandId, package) { }

		public static void Initialize(int commandId, VisualStudioPackage package)
		{
			Instance = new BreakAllInCurrentDocument(commandId, package);
		}

		protected override bool ShouldEnableCommand()
		{
			if (!Package.Configuration.IsBreakAllInCurrentDocumentEnabled)
			{
				return false;
			}

			var dte = ServiceProvider.GetDte();

			return (dte.Debugger.CurrentMode == dbgDebugMode.dbgRunMode);
		}

		protected override void InvokeHandler(object sender, EventArgs eventArgs)
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