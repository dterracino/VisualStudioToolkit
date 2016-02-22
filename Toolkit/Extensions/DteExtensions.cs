using EnvDTE80;

using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace TheSolutionEngineers.Toolkit.Extensions
{
	public static class DteExtensions
	{
		public static TeamFoundationServerExt GetTfs(this DTE2 dte)
		{
			return (TeamFoundationServerExt) dte.GetObject(typeof(TeamFoundationServerExt).FullName);
		}

		public static VersionControlExt GetTfsVersionControl(this DTE2 dte)
		{
			return (VersionControlExt) dte.GetObject(typeof(VersionControlExt).FullName);
		}
	}
}
