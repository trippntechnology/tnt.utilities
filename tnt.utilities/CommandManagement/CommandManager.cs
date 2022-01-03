using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	/// <summary>
	/// Method signature for OnStatusBarHintChanged
	/// </summary>
	/// <param name="hint"></param>
	public delegate void StatusBarHintChangedHandler(string hint);

	/// <summary>
	/// Manages commands
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public class CommandManager : Dictionary<string, Command>
	{
		#region Event handlers

		/// <summary>
		/// Event that is fired when a hint is available or removed
		/// </summary>
		public StatusBarHintChangedHandler OnStatusBarHintChanged;

		#endregion 

		/// <summary>
		/// Initializes a CommandManager.
		/// </summary>
		/// <param name="statusBarHintChanged">Event handler to call when the hit changes</param>
		public CommandManager(StatusBarHintChangedHandler statusBarHintChanged)
		{
			Application.Idle += Application_Idle;
			OnStatusBarHintChanged += statusBarHintChanged;
		}

		/// <summary>
		/// Creates a command
		/// </summary>
		/// <param name="key">Key used to reference the command</param>
		/// <param name="execHandler">Handler executed OnExeucte. Can be null.</param>
		/// <param name="updateHandler">Handler executed OnUpdate. Can be null.</param>
		/// <returns>TNT.Components.Command</returns>
		public Command Create(string key, ExecuteHandler execHandler, UpdateHandler updateHandler )
		{
			Command cmd = new Command(execHandler, updateHandler);
			cmd.OnShowToolTip += ShowToolTipHandler;

			this.Add(key, cmd);

			return cmd;
		}

		/// <summary>
		/// Creates a command
		/// </summary>
		/// <param name="key">Key used to reference the command</param>
		/// <param name="execHandler">Handler executed OnExeucte. Can be null.</param>
		/// <returns>TNT.Components.Command</returns>
		public Command Create(string key, ExecuteHandler execHandler)
		{
			Command cmd = new Command(execHandler);
			cmd.OnShowToolTip += ShowToolTipHandler;

			this.Add(key, cmd);

			return cmd;
		}

		/// <summary>
		/// Creates a command
		/// </summary>
		/// <param name="key">Key used to reference the command</param>
		/// <returns>TNT.Components.Command</returns>
		public Command Create(string key)
		{
			Command cmd = new Command();
			cmd.OnShowToolTip += ShowToolTipHandler;

			this.Add(key, cmd);

			return cmd;
		}

		/// <summary>
		/// Calls the Command's Update method.
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		private void Application_Idle(Object sender, EventArgs e)
		{
			foreach (string key in this.Keys)
			{
				this[key].Update();
			}
		}

		private void ShowToolTipHandler(Command cmd, string toolTip)
		{
			if (OnStatusBarHintChanged != null)
			{
				OnStatusBarHintChanged(toolTip);
			}
		}

	}
}
