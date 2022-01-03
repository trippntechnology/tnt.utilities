using System;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	/// <summary>
	/// Represents a ToolStripsplitButton
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public class ToolStripSplitButtonCommandExecutor : CommandExecutor
	{
		#region Properties

		/// <summary>
		/// Casts Base.m_ToolStripItem to ToolStripSplitButton
		/// </summary>
		protected ToolStripSplitButton Button { get { return ToolStripItem as ToolStripSplitButton; } }

		#endregion

		/// <summary>
		/// Initializes new ToolStripSplitButtonCommandExecutor
		/// </summary>
		/// <param name="command">Command owner</param>
		/// <param name="toolStripSplitButton">ToolStripSplitButton</param>
		public ToolStripSplitButtonCommandExecutor(Command command, ToolStripSplitButton toolStripSplitButton)
			:base(command, toolStripSplitButton)
		{
		}

		/// <summary>
		/// Associates the ExecuteCommand method with the ToolStripSplitButton.ButtonClick
		/// event.
		/// </summary>
		/// <param name="executeCommand">Hander method that calls Command.ExecuteCommand()</param>
		protected override void SetExecuteCommandEventHandler(System.EventHandler executeCommand)
		{
			Button.ButtonClick += executeCommand;
		}

		/// <summary>
		/// Not valid for this control
		/// </summary>
		public override bool Checked { get; set; }

		/// <summary>
		/// Not valid for this control
		/// </summary>
		public override bool CheckOnClick { get; set; }

		/// <summary>
		/// Not valid for this control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void CheckStateChanged(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}
