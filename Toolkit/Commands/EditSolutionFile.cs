using System;

using ThijsTijsma.VisualStudioToolkit.VisualStudio;

namespace ThijsTijsma.VisualStudioToolkit.Commands
{
	internal sealed class EditSolutionFile : SingleCommand<EditSolutionFile>
	{
		private EditSolutionFile(int commandId, VisualStudioPackage package) : base(commandId, package) { }

		public static void Initialize(int commandId, VisualStudioPackage package)
		{
			Instance = new EditSolutionFile(commandId, package);
		}

		protected override bool ShouldEnableCommand()
		{
			return Package.Configuration.IsEditProjectSolutionFileEnabled;
		}

		protected override void InvokeHandler(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var solutionPath = dte.Solution.FullName;

			dte.ExecuteCommand("File.CloseSolution");
			dte.ItemOperations.OpenFile(solutionPath, ViewKind.Text);
		}
	}
}