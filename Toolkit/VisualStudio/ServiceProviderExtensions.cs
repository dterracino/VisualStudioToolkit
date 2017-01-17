using System;
using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace ThijsTijsma.VisualStudioToolkit.VisualStudio
{
	internal static class ServiceProviderExtensions
	{
		public static TTarget GetService<TSource, TTarget>(this IServiceProvider serviceProvider)
			where TSource : class
			where TTarget : class
		{
			var service = serviceProvider.GetService(typeof(TSource)) as TTarget;

			if (service == null)
			{
				throw new ServiceUnavailableException(typeof(TSource));
			}

			return service;
		}

		public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
		{
			return serviceProvider.GetService<T, T>();
		}

		public static DTE2 GetDte(this IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<DTE, DTE2>();
		}

		public static IMenuCommandService GetMenuCommandService(this IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<IMenuCommandService>();
		}
	}
}
