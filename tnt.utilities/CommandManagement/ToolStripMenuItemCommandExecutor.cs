using System;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	/// <summary>
	/// Class that represents a ToolStripMenuItem
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public class ToolStripMenuItemCommandExecutor : CommandExecutor
	{
		#region Properties

		/// <summary>
		/// Property to cast Base.m_ToolStripItem to ToolStripMenuItem
		/// </summary>
		protected ToolStripMenuItem MenuItem { get { return (ToolStripItem as ToolStripMenuItem); } }

		/// <summary>
		/// Represents a ToolStripMenuItem.Checked property
		/// </summary>
		public override bool Checked
		{
			get { return MenuItem.Checked; }
			set { MenuItem.Checked = value; }
		}

		/// <summary>
		/// Represents a ToolStripMenuItem.CheckOnClick property
		/// </summary>
		public override bool CheckOnClick
		{
			get { return MenuItem.CheckOnClick; }
			set { MenuItem.CheckOnClick = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a ToolStripMenuItemCommandExecutor with a Command and ToolStripMenuItem
		/// </summary>
		/// <param name="cmd">Command that owns this CommandExecutor</param>
		/// <param name="menuItem">ToolStripMenuItem to associate to the ToolStripMenuItemCommandExecutor</param>
		public ToolStripMenuItemCommandExecutor(Command cmd, ToolStripMenuItem menuItem)
			: base(cmd, menuItem)
		{
			MenuItem.CheckStateChanged += CheckStateChanged;
		}

		#endregion

		/// <summary>
		/// ToolStripMenuItem CheckStateChanged event handler. This sets the Command's
		/// Checked property to syncronize all ToolStripItems assocated with the Command
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected override void CheckStateChanged(object sender, EventArgs e)
		{
			m_Command.Checked = (sender as ToolStripMenuItem).Checked;
		}
	}
}
