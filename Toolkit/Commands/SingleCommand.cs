using System;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell;

namespace TheSolutionEngineers.Toolkit.Commands
{
	internal class SingleCommand<T> : Command<T>
	{
		private readonly OleMenuCommand _command;

		protected SingleCommand(int commandId, VisualStudioPackage package) : base(commandId, package)
		{
			_command = new OleMenuCommand(InvokeHandler, new CommandID(CommandSet.Guid, commandId));
			_command.BeforeQueryStatus += CommandOnBeforeQueryStatus;
			MenuCommandService.AddCommand(_command);
		}

		private void CommandOnBeforeQueryStatus(object sender, EventArgs eventArgs)
		{
			var shouldEnable = ShouldEnableCommand();

			_command.Visible = shouldEnable;
			_command.Enabled = shouldEnable;
			_command.Supported = shouldEnable;
		}

		protected virtual bool ShouldEnableCommand()
		{
			throw new NotImplementedException();
		}

		protected virtual void InvokeHandler(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}
	}
}
