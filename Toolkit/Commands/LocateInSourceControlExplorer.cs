using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

using ThijsTijsma.VisualStudioToolkit.VisualStudio;
using Thread = System.Threading.Thread;

namespace ThijsTijsma.VisualStudioToolkit.Commands
{
	internal sealed class LocateInSourceControlExplorer : SingleCommand<LocateInSourceControlExplorer>
	{
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

		private LocateInSourceControlExplorer(int commandId, VisualStudioPackage package) : base(commandId, package) { }

		public static void Initialize(int commandId, VisualStudioPackage package)
		{
			Instance = new LocateInSourceControlExplorer(commandId, package);
		}

		protected override bool ShouldEnableCommand()
		{
			if (!Package.Configuration.IsLocateInSourceControlExplorerEnabled)
			{
				return false;
			}

			var dte = ServiceProvider.GetDte();
			var vc = dte.GetTfsVersionControl();

			var selectedItemPath = SelectedItemPath;

			if (string.IsNullOrEmpty(selectedItemPath))
			{
				return false;
			}

			return vc.SolutionWorkspace?.IsLocalPathMapped(selectedItemPath) == true;
		}

		protected override void InvokeHandler(object sender, EventArgs eventArgs)
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