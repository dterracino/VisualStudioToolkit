using System;
using System.ComponentModel.Design;

using TheSolutionEngineers.Toolkit.VisualStudio;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal class Command<T>
	{
		public static Command<T> Instance { get; protected set; }

		protected int CommandId { get; set; }
		protected VisualStudioPackage Package { get; private set; }
		protected IServiceProvider ServiceProvider { get; }

		protected IMenuCommandService MenuCommandService { get; private set; }

		protected Command(int commandId, VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			CommandId = commandId;
			Package = package;
			ServiceProvider = package;

			MenuCommandService = ServiceProvider.GetMenuCommandService();
		}
	}
}
