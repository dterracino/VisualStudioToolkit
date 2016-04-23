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
	}
}
