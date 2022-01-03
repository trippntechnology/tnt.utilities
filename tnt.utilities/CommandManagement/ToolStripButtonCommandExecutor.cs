using System;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	/// <summary>
	/// Class that represents a ToolStripButton
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public class ToolStripButtonCommandExecutor : CommandExecutor
	{
		#region Properties

		/// <summary>
		/// Property to cast Base.m_ToolStripItem to ToolStripButton
		/// </summary>
		protected ToolStripButton Button { get { return ToolStripItem as ToolStripButton; } }

		/// <summary>
		/// Represents a ToolStripButton.Checked property
		/// </summary>
		public override bool Checked
		{
			get { return Button.Checked; }
			set { Button.Checked = value; }
		}

		/// <summary>
		/// Represents a ToolStripButton.CheckOnClick property
		/// </summary>
		public override bool CheckOnClick
		{
			get { return Button.CheckOnClick; }
			set { Button.CheckOnClick = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a ToolStripButtonCommandExecutor with a Command and ToolStripButton
		/// </summary>
		/// <param name="cmd">Command that owns this CommandExecutor</param>
		/// <param name="button">ToolStripButton to associate to the ToolStripButtonCommandExecutor</param>
		public ToolStripButtonCommandExecutor(Command cmd, ToolStripButton button)
			: base(cmd, button)
		{
			Button.CheckStateChanged += CheckStateChanged;
		}

		#endregion

		/// <summary>
		/// ToolStripButton CheckStateChanged event handler. This sets the Command's
		/// Checked property to syncronize all ToolStripItems assocated with the Command
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected override void CheckStateChanged(object sender, EventArgs e)
		{
			m_Command.Checked = (sender as ToolStripButton).Checked;
		}
	}
}
