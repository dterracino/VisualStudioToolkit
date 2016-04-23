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
	public sealed class VisualStudioPackage : Package
	{
		public const string PackageGuidString = "5258bcf7-7850-4987-9a45-2c8a747e2b48";

		protected override void Initialize()
		{
			base.Initialize();

			Paddle.CreateSharedInstance(PaddleConfiguration.ApiKey, PaddleConfiguration.VendorId, PaddleConfiguration.ProductId);

			if (!Paddle.SharedInstance.StartLicensing())
			{
				return;
			}

		    BreakAllInCurrentDocument.Initialize(this);
		    LocateInSolutionExplorer.Initialize(this);
			LocateInSourceControlExplorer.Initialize(this);
			EditSolutionFile.Initialize(this);
			EditProjectFile.Initialize(this);
			CompareFiles.Initialize(this);
		}
	}
}
