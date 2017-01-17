using System;

using ThijsTijsma.VisualStudioToolkit.VisualStudio;

namespace ThijsTijsma.VisualStudioToolkit.Commands
{
	internal sealed class EditProjectFile : SingleCommand<EditProjectFile>
	{
		private EditProjectFile(int commandId, VisualStudioPackage package) : base(commandId, package) { }

		public static void Initialize(int commandId, VisualStudioPackage package)
		{
			Instance = new EditProjectFile(commandId, package);
		}

		protected override bool ShouldEnableCommand()
		{
			return Package.Configuration.IsEditProjectSolutionFileEnabled;
		}

		protected override void InvokeHandler(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();
			var projectPath = dte.SelectedItems.Item(1).Project.FullName;

			dte.ExecuteCommand("Project.UnloadProject");
			dte.ItemOperations.OpenFile(projectPath, ViewKind.Text);
		}
	}
}