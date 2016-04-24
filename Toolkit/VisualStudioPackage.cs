using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using PaddleSDK;

using TheSolutionEngineers.Toolkit.Commands;

namespace TheSolutionEngineers.Toolkit
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("The Solution Engineer Toolkit", "", "1.0", IconResourceID = 400)]
	[Guid(PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideAutoLoad(UIContextGuids.SolutionExists)]
	[ProvideOptionPage(typeof(ConfigurationPage), "The Solution Engineer Toolkit", "General", 0, 0, true)]
	public sealed class VisualStudioPackage : Package
	{
		public const string PackageGuidString = "5258bcf7-7850-4987-9a45-2c8a747e2b48";
		public ConfigurationPage Configuration { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			Paddle.CreateSharedInstance(PaddleConfiguration.ApiKey, PaddleConfiguration.VendorId, PaddleConfiguration.ProductId);

			if (!Paddle.SharedInstance.StartLicensing())
			{
				return;
			}

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
