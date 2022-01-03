using System;
using System.Drawing;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	/// <summary>
	/// Wraps a ToolStripItem to handle manipulation of the common properties and events
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public abstract class CommandExecutor
	{
		#region Members

		/// <summary>
		/// Command that owns the CommandExecutor
		/// </summary>
		protected Command m_Command;

		#endregion

		#region Properties

		/// <summary>
		/// ToolStripItem
		/// </summary>
		public ToolStripItem ToolStripItem { get; set; }

		/// <summary>
		/// References the ToolStripItem.Enabled property
		/// </summary>
		virtual public bool Enabled
		{
			get { return ToolStripItem.Enabled; }
			set { ToolStripItem.Enabled = value; }
		}

		/// <summary>
		/// References the ToolStripItem.Visible property
		/// </summary>
		virtual public bool Visible
		{
			get { return ToolStripItem.Visible; }
			set { ToolStripItem.Visible = value; }
		}

		/// <summary>
		/// References the ToolStripItem.Image property
		/// </summary>
		virtual public Image Image
		{
			get { return ToolStripItem.Image; }
			set { ToolStripItem.Image = value; }
		}

		/// <summary>
		/// References the ToolStripItem.Text property
		/// </summary>
		virtual public string Text
		{
			get { return ToolStripItem.Text; }
			set { ToolStripItem.Text = value; }
		}

		/// <summary>
		/// References the ToolStripItem.ToolTipText property
		/// </summary>
		virtual public string ToolTipText
		{
			get { return ToolStripItem.ToolTipText; }
			set { ToolStripItem.ToolTipText = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a CommandExecutor with a Command and associated ToolStripItem. Wires the Command
		/// event to the ToolStripItem.
		/// </summary>
		/// <param name="cmd">Command that owns this CommandExecutor</param>
		/// <param name="toolStripItem">ToolStripItem to associate with CommandExecutor</param>
		public CommandExecutor(Command cmd, ToolStripItem toolStripItem)
		{
			m_Command = cmd;
			ToolStripItem = toolStripItem;

			SetExecuteCommandEventHandler(ExecuteCommand);
			ToolStripItem.MouseEnter += MouseEnter;
			ToolStripItem.MouseLeave += MouseLeave;
		}

		#endregion

		/// <summary>
		/// Executes Command.Execute()
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected void ExecuteCommand(object sender, EventArgs e)
		{
			m_Command.Execute(this);
		}

		/// <summary>
		/// Send the ToolTip associated with this ToolStripItem
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected void MouseEnter(object sender, EventArgs e)
		{
			m_Command.ShowToolTip(ToolTipText);
		}

		/// <summary>
		/// Send and empty string to clear the previously sent ToolTip
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected void MouseLeave(object sender, EventArgs e)
		{
			m_Command.ShowToolTip(string.Empty);
		}

		/// <summary>
		/// Represents the checked property of the ToolStripItem's subclass
		/// </summary>
		public abstract bool Checked { get; set; }

		/// <summary>
		/// Represents the CheckOnClick property of the ToolStripItem's subclass
		/// </summary>
		public abstract bool CheckOnClick { get; set; }

		/// <summary>
		/// Associates the ExecuteCommand method with the ToolStripItem's Click
		/// event.
		/// </summary>
		/// <param name="executeCommand">Hander method that calls Command.ExecuteCommand()</param>
		virtual protected void SetExecuteCommandEventHandler(EventHandler executeCommand)
		{
			ToolStripItem.Click += executeCommand;
		}

		/// <summary>
		/// ToolStripItem's subclass's CheckStateChanged event handler. 
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		protected abstract void CheckStateChanged(object sender, EventArgs e);
	}
}
