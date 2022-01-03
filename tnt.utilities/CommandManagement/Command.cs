using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TNT.Utilities.CommandManagement
{
	#region Delegates
	
	/// <summary>
	/// Method signature of the OnExecute method
	/// </summary>
	/// <param name="cmd">Caller</param>
	public delegate void ExecuteHandler(Command cmd);

	/// <summary>
	/// Method signature of the OnUpdate method
	/// </summary>
	/// <param name="cmd"></param>
	public delegate void UpdateHandler(Command cmd);

	/// <summary>
	/// Method signature of the OnShowToolTip method
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="toolTip">ToolTip to show</param>
	public delegate void ShowToolTipHandler(Command cmd, string toolTip);

	#endregion

	/// <summary>
	/// A command represents multiple ToolStripItems 
	/// </summary>
	[Obsolete("Use TNT.ToolStripItemManager")]
	public class Command
	{
		#region Members

		/// <summary>
		/// List of CommandExecutors
		/// </summary>
		protected List<CommandExecutor> m_Executors = new List<CommandExecutor>();

		#endregion

		#region Properties

		/// <summary>
		/// References the ToolStripItem.Text property
		/// </summary>
		public string Text
		{
			get { return m_Executors.First().Text; }
			set { m_Executors.ForEach(e => e.Text = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.Enabled property
		/// </summary>
		public bool Enabled
		{
			get { return m_Executors.First().Enabled; }
			set { m_Executors.ForEach(e => e.Enabled = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.Image property
		/// </summary>
		public Image Image
		{
			get { return m_Executors.First().Image; }
			set { m_Executors.ForEach(e => e.Image = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.Checked property
		/// </summary>
		public bool Checked
		{
			get { return m_Executors.First().Checked; }
			set { m_Executors.ForEach(e => e.Checked = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.CheckOnClick property
		/// </summary>
		public bool CheckOnClick
		{
			get { return m_Executors.First().CheckOnClick; }
			set { m_Executors.ForEach(e => e.CheckOnClick = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.Visible property
		/// </summary>
		public bool Visible 
		{
			get { return m_Executors.First().Visible; }
			set { m_Executors.ForEach(e => e.Visible = value); }
		}

		/// <summary>
		/// Represents the ToolStripItem.TooTipText property
		/// </summary>
		public string ToolTipText
		{
			get { return m_Executors.First().ToolTipText; }
			set { m_Executors.ForEach(e => e.ToolTipText = value); }
		}

		/// <summary>
		/// User-defined data associated with the item
		/// </summary>
		public object Tag { get; set; }

		/// <summary>
		/// The executor that initiated the event
		/// </summary>
		public CommandExecutor Executor { get; protected set; }

		#endregion

		#region Event Handlers

		/// <summary>
		/// Executed by Execute
		/// </summary>
		public event ExecuteHandler OnExecute;

		/// <summary>
		/// OnUpdate event handler
		/// </summary>
		public event UpdateHandler OnUpdate;

		/// <summary>
		/// OnShowToolTip event handler
		/// </summary>
		public event ShowToolTipHandler OnShowToolTip;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Command()
		{
		}

		/// <summary>
		/// Initializes new Command with an ExecuteHandler
		/// </summary>
		/// <param name="executeHandler">ExecuteHander to execute</param>
		public Command(ExecuteHandler executeHandler)
		{
			OnExecute += executeHandler;
		}

		/// <summary>
		/// Initializes new Command with an ExecuteHandler and UpdateHandler
		/// </summary>
		/// <param name="executeHandler">ExecuteHander to execute</param>
		/// <param name="updateHandler">UpdateHandler to execute</param>
		public Command(ExecuteHandler executeHandler, UpdateHandler updateHandler)
		{
			OnExecute += executeHandler;
			OnUpdate += updateHandler;
		}

		#endregion

		/// <summary>
		/// Executes the OnExecute event if it exists
		/// </summary>
		/// <param name="executor">Executor that initiated the event</param>
		internal void Execute(CommandExecutor executor)
		{
			if (OnExecute != null)
			{
				Executor = executor;
				OnExecute(this);
			}
		}

		/// <summary>
		/// Executes the OnUpdate event if it exists
		/// </summary>
		internal void Update()
		{
			if (OnUpdate != null)
			{
				OnUpdate(this);
			}
		}

		/// <summary>
		/// Executes the OnShowToolTip event if it exists
		/// </summary>
		internal void ShowToolTip(string toolTip)
		{
			if (OnShowToolTip != null)
			{
				OnShowToolTip(this, toolTip);
			}
		}

		/// <summary>
		/// Associates a new ToolStripItem with this command
		/// </summary>
		/// <param name="item">ToolStripItem to associate</param>
		/// <returns>CommandExecutor that represents this ToolStripItem</returns>
		public CommandExecutor Add(ToolStripItem item)
		{
			CommandExecutor cmdEx = null;

			if (item.GetType() == typeof(ToolStripButton))
			{
				cmdEx = new ToolStripButtonCommandExecutor(this, item as ToolStripButton);
			}
			else if (item.GetType() == typeof(ToolStripSplitButton))
			{
				cmdEx = new ToolStripSplitButtonCommandExecutor(this, item as ToolStripSplitButton);
			}
			else if (item.GetType() == typeof(ToolStripMenuItem))
			{
				cmdEx = new ToolStripMenuItemCommandExecutor(this, item as ToolStripMenuItem);
			}
			else
			{
				return null;
			}

			m_Executors.Add(cmdEx);

			// Set all properties of previous items to match the last added
			SetProperties(cmdEx);

			return cmdEx;
		}

		/// <summary>
		/// Called to initialize all CommandExecutors
		/// </summary>
		virtual protected void SetProperties(CommandExecutor cmdEx)
		{
			Checked = cmdEx.Checked;
			Enabled = cmdEx.Enabled;
			Image = cmdEx.Image;
			Text = cmdEx.Text;
			ToolTipText = cmdEx.ToolTipText;
		}
	}
}
