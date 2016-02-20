using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit
{
	internal sealed class LocateInSolutionExplorer
	{
		public static LocateInSolutionExplorer Instance { get; private set; }

		public const int MenuCommandId = 0x0003;

		private readonly Package _package;
		private IServiceProvider ServiceProvider => _package;

		private LocateInSolutionExplorer(VisualStudioPackage package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetMenuCommandService();

			var command = new OleMenuCommand(CommandCallback, new CommandID(CommandSet.Guid, MenuCommandId));
			command.BeforeQueryStatus += Command_BeforeQueryStatus;
			commandService.AddCommand(command);
		}

		public static void Initialize(VisualStudioPackage package)
		{
			Instance = new LocateInSolutionExplorer(package);
		}

		private void Command_BeforeQueryStatus(object sender, EventArgs e)
		{
			var command = (OleMenuCommand) sender;
			var dte = ServiceProvider.GetDte();

			var hasProjectItem = (dte.ActiveDocument?.ProjectItem != null);

			command.Visible = hasProjectItem;
			command.Supported = hasProjectItem;
			command.Enabled = hasProjectItem;
		}

		private void CommandCallback(object sender, EventArgs e)
		{
			var dte = ServiceProvider.GetDte();

			var item = LocateHierarchyItem(dte.ActiveDocument.ProjectItem);

			if (item == null)
			{
				return;
			}
			
			item.Select(vsUISelectionType.vsUISelectionTypeSelect);
			item.Select(vsUISelectionType.vsUISelectionTypeSetCaret);

			dte.Windows.Item(VSConstants.StandardToolWindows.SolutionExplorer.ToString("B")).Activate();
		}

		private UIHierarchyItem LocateHierarchyItem(ProjectItem item)
		{
			var dte = ServiceProvider.GetDte();
			var solutionExplorer = dte.ToolWindows.SolutionExplorer;
			var solutionItem = solutionExplorer.UIHierarchyItems.Item(1);

			return LocateHierarchyItem(solutionItem.UIHierarchyItems, item);
		}

		private UIHierarchyItem LocateHierarchyItem(UIHierarchyItems items, object item)
		{
			var dte = ServiceProvider.GetDte();
			var solutionExplorer = dte.ToolWindows.SolutionExplorer;
			var stack = CreateItemHierarchyStack(item);
			UIHierarchyItem projectHierarchyItem = null;

			while (stack.Count > 0)
			{
				if (!items.Expanded)
				{
					items.Expanded = true;
				}

				if (!items.Expanded)
				{
					var parent = (UIHierarchyItem) items.Parent;
					parent.Select(vsUISelectionType.vsUISelectionTypeSelect);
					solutionExplorer.DoDefaultAction();
				}

				var o = stack.Pop();

				var project1 = o as Project;
				var projectItem1 = o as ProjectItem;

				for (var i = 0; i < items.Count; i++)
				{
					var hierarchyItem = items.Item(i + 1);
					var project2 = hierarchyItem.Object as Project;
					var projectItem2 = hierarchyItem.Object as ProjectItem;

					if ((project1 != null && project2 != null && project1.Object == project2.Object) ||
					    (projectItem1 != null && projectItem2 != null && projectItem1.Object == projectItem2.Object)
						)
					{
						projectHierarchyItem = hierarchyItem;
						items = hierarchyItem.UIHierarchyItems;
					}
				}
			}

			return projectHierarchyItem;
		}

		private Stack<object> CreateItemHierarchyStack(object item)
		{
			return CreateItemHierarchyStack(new Stack<object>(), item);
		}

		private Stack<object> CreateItemHierarchyStack(Stack<object> stack, object item)
		{
			if (item is ProjectItem)
			{
				var projectItem = (ProjectItem) item;
				stack.Push(projectItem);
				CreateItemHierarchyStack(stack, projectItem.Collection.Parent);

			}
			else if (item is Project)
			{
				var project = (Project) item;
				stack.Push(project);

				if (project.ParentProjectItem != null)
				{
					CreateItemHierarchyStack(stack, project.ParentProjectItem);
				}
			}

			return stack;
		}
	}
}