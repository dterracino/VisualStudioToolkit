using System.Linq;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace TheSolutionEngineers.Toolkit.VisualStudio
{
	public static class DteExtensions
	{
		public static VersionControlExt GetTfsVersionControl(this DTE2 dte)
		{
			return (VersionControlExt) dte.GetObject(typeof(VersionControlExt).FullName);
		}

		public static T GetSelectedSolutionExplorerItem<T>(this DTE2 dte)
		{
			var selectedItems = (UIHierarchyItem[]) dte.ToolWindows.SolutionExplorer.SelectedItems;

			return (T) selectedItems.Single().Object;
		}
	}
}
