using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace ThijsTijsma.VisualStudioToolkit
{
	public class ConfigurationPage : DialogPage
	{
		private const string FeaturesCategoryName = "Features";

		[Category(FeaturesCategoryName)]
		[DisplayName("Enable 'Locate In Solution Explorer'")]
		public bool IsLocateInSolutionExplorerEnabled { get; set; } = true;

		[Category(FeaturesCategoryName)]
		[DisplayName("Enable 'Locate In Source Control Explorer'")]
		public bool IsLocateInSourceControlExplorerEnabled { get; set; } = true;

		[Category(FeaturesCategoryName)]
		[DisplayName("Enable 'Break All In Current Document'")]
		public bool IsBreakAllInCurrentDocumentEnabled { get; set; } = true;

		[Category(FeaturesCategoryName)]
		[DisplayName("Enable 'Compare files'")]
		public bool IsCompareFilesEnabled { get; set; } = true;

		[Category(FeaturesCategoryName)]
		[DisplayName("Enable 'Edit project/solution file''")]
		public bool IsEditProjectSolutionFileEnabled { get; set; } = true;
	}
}
