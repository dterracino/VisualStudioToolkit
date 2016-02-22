using System;
using System.ComponentModel.Design;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit.Extensions
{
	internal static class ServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
		{
			var service = serviceProvider.GetService(typeof(T)) as T;

			if (service == null)
			{
				throw new ServiceUnavailableException(typeof(T));
			}

			return service;
		}

		public static DTE2 GetDte(this IServiceProvider serviceProvider)
		{
			return (DTE2) serviceProvider.GetService<DTE>();
		}

		public static IMenuCommandService GetMenuCommandService(this IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<IMenuCommandService>();
		}
	}
}
