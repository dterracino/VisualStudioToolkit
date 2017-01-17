using System;
using System.IO;
using Microsoft.TeamFoundation.VersionControl.Client;

using ThijsTijsma.VisualStudioToolkit.VisualStudio;

namespace ThijsTijsma.VisualStudioToolkit.Commands
{
	internal sealed class CompareFiles : SingleCommand<CompareFiles>
	{
		private CompareFiles(int commandId, VisualStudioPackage package) : base(commandId, package) { }

		public static void Initialize(int commandId, VisualStudioPackage package)
		{
			Instance = new CompareFiles(commandId, package);
		}

		protected override bool ShouldEnableCommand()
		{
			if (!Package.Configuration.IsCompareFilesEnabled)
			{
				return false;
			}

			var dte = ServiceProvider.GetDte();

			return (dte.SelectedItems.Count == 2);
		}

		protected override void InvokeHandler(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();

			var firstFilePath = dte.SelectedItems.Item(1).ProjectItem.FileNames[1];
			var secondFilePath = dte.SelectedItems.Item(2).ProjectItem.FileNames[1];

			Difference.VisualDiffFiles(firstFilePath, secondFilePath, Path.GetDirectoryName(firstFilePath), Path.GetDirectoryName(secondFilePath), Path.GetFileName(firstFilePath), Path.GetFileName(secondFilePath), false, false);
		}
	}
}
