﻿using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("The Solution Engineer Toolkit", "", "1.0", IconResourceID = 400)]
	[Guid(PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	public sealed class VisualStudioPackage : Package
	{
		public const string PackageGuidString = "5258bcf7-7850-4987-9a45-2c8a747e2b48";

		protected override void Initialize()
		{
            base.Initialize();
		    BreakInCurrentDocument.Initialize(this);
		}
	}
}
