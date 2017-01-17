using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using ThijsTijsma.VisualStudioToolkit.Commands;

namespace ThijsTijsma.VisualStudioToolkit
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration(Name, "", Version, IconResourceID = 400)]
	[Guid(PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideAutoLoad(UIContextGuids.SolutionExists)]
	[ProvideOptionPage(typeof(ConfigurationPage), Name, "General", 0, 0, true)]
	public sealed class VisualStudioPackage : Package
	{
		public const string PackageGuidString = "5258bcf7-7850-4987-9a45-2c8a747e2b48";

		public const string Name = "Visual Studio Toolkit";
		public const string Version = "1.4.1";

		public ConfigurationPage Configuration { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			Configuration = (ConfigurationPage) GetDialogPage(typeof(ConfigurationPage));

			BreakAllInCurrentDocument.Initialize(0x0001, this);
			LocateInSolutionExplorer.Initialize(0x0002, this);
			LocateInSourceControlExplorer.Initialize(0x0003, this);
			EditSolutionFile.Initialize(0x0004, this);
			EditProjectFile.Initialize(0x0005, this);
			CompareFiles.Initialize(0x0006, this);
		}
	}
}
