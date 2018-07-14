using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;
using System.Drawing.Design;

namespace FrontWork{
	public partial class FormConfigurationEditor : Form
    {
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        public FormConfigurationEditor() {
			InitializeComponent();
            scintillaTextArea = new ScintillaNET.Scintilla();
            scintillaTextArea.Margin = new Padding(0);
            scintillaTextArea.BorderStyle = BorderStyle.None;
            scintillaTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            TextPanel.Controls.Add(scintillaTextArea);
        }

		private Scintilla scintillaTextArea;

        ////启用双缓冲技术
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        public void SetText(string text)
        {
            this.scintillaTextArea.Text = text;
        }

        public string GetText()
        {
            return this.scintillaTextArea.Text;
        }

		private void MainForm_Load(object sender, EventArgs e) {
			// BASIC CONFIG
			scintillaTextArea.TextChanged += (this.OnTextChanged);

			// INITIAL VIEW CONFIG
			//scintillaTextArea.WrapMode = WrapMode.None;
			scintillaTextArea.IndentationGuides = IndentView.LookBoth;
            scintillaTextArea.ScrollWidth = 100;

			// STYLING
			InitColors();
			InitSyntaxColoring();

			// NUMBER MARGIN
			InitNumberMargin();

			// BOOKMARK MARGIN
			InitBookmarkMargin();

			// CODE FOLDING MARGIN
			InitCodeFolding();

			// DRAG DROP
			InitDragDropFile();

			// INIT HOTKEYS
			InitHotkeys();
		}

		private void InitColors()
        {
            scintillaTextArea.CaretForeColor = Color.LightGray;
            scintillaTextArea.CaretWidth = 2;
            scintillaTextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
		}

		private void InitHotkeys() {

			// register the hotkeys with the form
			HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
			HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
			HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
			HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
			HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
			HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
			HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

			// remove conflicting hotkeys from scintilla
			scintillaTextArea.ClearCmdKey(Keys.Control | Keys.F);
			scintillaTextArea.ClearCmdKey(Keys.Control | Keys.R);
			scintillaTextArea.ClearCmdKey(Keys.Control | Keys.H);
			scintillaTextArea.ClearCmdKey(Keys.Control | Keys.L);
			scintillaTextArea.ClearCmdKey(Keys.Control | Keys.U);

		}

		private void InitSyntaxColoring() {

			// Configure the default style
			scintillaTextArea.StyleResetDefault();
            scintillaTextArea.Styles[Style.Default].Font = "Consolas";
			scintillaTextArea.Styles[Style.Default].Size = 12;
            scintillaTextArea.Styles[Style.Default].BackColor = IntToColor(0x1E1E1E);
            scintillaTextArea.Styles[Style.Default].ForeColor = IntToColor(0X9CDCFE);
			scintillaTextArea.StyleClearAll();
            
            scintillaTextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0X569CD6);
            scintillaTextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0XB5CEA8);
            scintillaTextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0XC39178);
            scintillaTextArea.Styles[Style.Cpp.StringEol].ForeColor = IntToColor(0XC39178);
            scintillaTextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0X608B4E);
            scintillaTextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0X608B4E);
            scintillaTextArea.Styles[Style.Cpp.Operator].ForeColor = Color.LightGray;
      
            scintillaTextArea.Lexer = Lexer.Cpp;

            scintillaTextArea.SetKeywords(0, @"
                abstract	arguments	boolean	break	byte
                case	catch	char	class	const
                continue	debugger	default	delete	do
                double	else	enum	eval	export
                extends	false	final	finally	float
                for	function	goto	if	implements
                import	in	instanceof	int	interface
                let	long	native	new	null
                package	private	protected	public	return
                short	static	super	switch	synchronized
                this	throw	throws	transient	true
                try	typeof	var	void	volatile
                while	with	yield
            ");
		}

		private void OnTextChanged(object sender, EventArgs e) {

		}


        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

		/// <summary>
		/// change this to whatever margin you want the line numbers to show in
		/// </summary>
		private const int NUMBER_MARGIN = 1;

		/// <summary>
		/// change this to whatever margin you want the bookmarks/breakpoints to show in
		/// </summary>
		private const int BOOKMARK_MARGIN = 2;
		private const int BOOKMARK_MARKER = 2;

		/// <summary>
		/// change this to whatever margin you want the code folding tree (+/-) to show in
		/// </summary>
		private const int FOLDING_MARGIN = 3;

		/// <summary>
		/// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
		/// </summary>
		private const bool CODEFOLDING_CIRCULAR = true;

		private void InitNumberMargin() {

			scintillaTextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
			scintillaTextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
			scintillaTextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
			scintillaTextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

			var nums = scintillaTextArea.Margins[NUMBER_MARGIN];
			nums.Width = 30;
			nums.Type = MarginType.Number;
			nums.Sensitive = true;
			nums.Mask = 0;

			scintillaTextArea.MarginClick += TextArea_MarginClick;
		}

		private void InitBookmarkMargin() {

			//TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

			var margin = scintillaTextArea.Margins[BOOKMARK_MARGIN];
			margin.Width = 20;
			margin.Sensitive = true;
			margin.Type = MarginType.Symbol;
			margin.Mask = (1 << BOOKMARK_MARKER);
			//margin.Cursor = MarginCursor.Arrow;

			var marker = scintillaTextArea.Markers[BOOKMARK_MARKER];
			marker.Symbol = MarkerSymbol.Circle;
			marker.SetBackColor(IntToColor(0xFF003B));
			marker.SetForeColor(IntToColor(0x000000));
			marker.SetAlpha(100);

		}

		private void InitCodeFolding() {

			scintillaTextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
			scintillaTextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

			// Enable code folding
			scintillaTextArea.SetProperty("fold", "1");
			scintillaTextArea.SetProperty("fold.compact", "1");

			// Configure a margin to display folding symbols
			scintillaTextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
			scintillaTextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
			scintillaTextArea.Margins[FOLDING_MARGIN].Sensitive = true;
			scintillaTextArea.Margins[FOLDING_MARGIN].Width = 20;

			// Set colors for all folding markers
			for (int i = 25; i <= 31; i++) {
				scintillaTextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
				scintillaTextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
			}

			// Configure folding markers with respective symbols
			scintillaTextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
			scintillaTextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
			scintillaTextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
			scintillaTextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			scintillaTextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
			scintillaTextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			scintillaTextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			scintillaTextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

		}

		private void TextArea_MarginClick(object sender, MarginClickEventArgs e) {
			if (e.Margin == BOOKMARK_MARGIN) {
				// Do we have a marker for this line?
				const uint mask = (1 << BOOKMARK_MARKER);
				var line = scintillaTextArea.Lines[scintillaTextArea.LineFromPosition(e.Position)];
				if ((line.MarkerGet() & mask) > 0) {
					// Remove existing bookmark
					line.MarkerDelete(BOOKMARK_MARKER);
				} else {
					// Add bookmark
					line.MarkerAdd(BOOKMARK_MARKER);
				}
			}
		}

		#endregion

		#region Drag & Drop File

		public void InitDragDropFile() {

			scintillaTextArea.AllowDrop = true;
			scintillaTextArea.DragEnter += delegate(object sender, DragEventArgs e) {
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.None;
			};
			scintillaTextArea.DragDrop += delegate(object sender, DragEventArgs e) {

				// get file drop
				if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

					Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
					if (a != null) {

						string path = a.GetValue(0).ToString();

						LoadDataFromFile(path);

					}
				}
			};

		}

		private void LoadDataFromFile(string path) {
			if (File.Exists(path)) {
				scintillaTextArea.Text = File.ReadAllText(path);
			}
		}

		#endregion

		#region Main Menu Commands

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				LoadDataFromFile(openFileDialog.FileName);
			}
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenSearch();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.SelectAll();
		}

		private void selectLineToolStripMenuItem_Click(object sender, EventArgs e) {
			Line line = scintillaTextArea.Lines[scintillaTextArea.CurrentLine];
			scintillaTextArea.SetSelection(line.Position + line.Length, line.Position);
		}

		private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.SetEmptySelection(0);
		}

		private void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Indent();
		}

		private void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Outdent();
		}

		private void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Uppercase();
		}

		private void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Lowercase();
		}

		//private void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e) {
		//	// toggle word wrap
		//	wordWrapItem.Checked = !wordWrapItem.Checked;
		//	scintillaTextArea.WrapMode = wordWrapItem.Checked ? WrapMode.Word : WrapMode.None;
		//}
		
		//private void indentGuidesToolStripMenuItem_Click(object sender, EventArgs e) {

		//	// toggle indent guides
		//	indentGuidesItem.Checked = !indentGuidesItem.Checked;
		//	scintillaTextArea.IndentationGuides = indentGuidesItem.Checked ? IndentView.LookBoth : IndentView.None;
		//}

		private void hiddenCharactersToolStripMenuItem_Click(object sender, EventArgs e) {

			// toggle view whitespace
			hiddenCharactersItem.Checked = !hiddenCharactersItem.Checked;
			scintillaTextArea.ViewWhitespace = hiddenCharactersItem.Checked ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomIn();
		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomOut();
		}

		private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomDefault();
		}

		private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.FoldAll(FoldAction.Contract);
		}

		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
			scintillaTextArea.FoldAll(FoldAction.Expand);
		}
		

		#endregion

		#region Uppercase / Lowercase

		private void Lowercase() {

			// save the selection
			int start = scintillaTextArea.SelectionStart;
			int end = scintillaTextArea.SelectionEnd;

			// modify the selected text
			scintillaTextArea.ReplaceSelection(scintillaTextArea.GetTextRange(start, end - start).ToLower());

			// preserve the original selection
			scintillaTextArea.SetSelection(start, end);
		}

		private void Uppercase() {

			// save the selection
			int start = scintillaTextArea.SelectionStart;
			int end = scintillaTextArea.SelectionEnd;

			// modify the selected text
			scintillaTextArea.ReplaceSelection(scintillaTextArea.GetTextRange(start, end - start).ToUpper());

			// preserve the original selection
			scintillaTextArea.SetSelection(start, end);
		}

		#endregion

		#region Indent / Outdent

		private void Indent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
			// although the indentation function exists. Pressing TAB with the editor focused confirms this.
			GenerateKeystrokes("{TAB}");
		}

		private void Outdent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
			// although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
			GenerateKeystrokes("+{TAB}");
		}

		private void GenerateKeystrokes(string keys) {
			HotKeyManager.Enable = false;
			scintillaTextArea.Focus();
			SendKeys.Send(keys);
			HotKeyManager.Enable = true;
		}

		#endregion

		#region Zoom

		private void ZoomIn() {
			scintillaTextArea.ZoomIn();
		}

		private void ZoomOut() {
			scintillaTextArea.ZoomOut();
		}

		private void ZoomDefault() {
			scintillaTextArea.Zoom = 0;
		}


		#endregion

		#region Quick Search Bar

		bool SearchIsOpen = false;

		private void OpenSearch() {

			SearchManager.SearchBox = textBoxSearch;
			SearchManager.TextArea = scintillaTextArea;

			if (!SearchIsOpen) {
				SearchIsOpen = true;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = true;
					textBoxSearch.Text = SearchManager.LastSearch;
					textBoxSearch.Focus();
					textBoxSearch.SelectAll();
				});
			} else {
				InvokeIfNeeded(delegate() {
					textBoxSearch.Focus();
					textBoxSearch.SelectAll();
				});
			}
		}
		private void CloseSearch() {
			if (SearchIsOpen) {
				SearchIsOpen = false;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = false;
					//CurBrowser.GetBrowser().StopFinding(true);
				});
			}
		}

		private void BtnClearSearch_Click(object sender, EventArgs e) {
			CloseSearch();
		}

		private void BtnPrevSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(false, false);
		}
		private void BtnNextSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(true, false);
		}
		private void TxtSearch_TextChanged(object sender, EventArgs e) {
			SearchManager.Find(true, true);
		}

		private void TxtSearch_KeyDown(object sender, KeyEventArgs e) {
			if (HotKeyManager.IsHotkey(e, Keys.Enter)) {
				SearchManager.Find(true, false);
			}
			if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true)) {
				SearchManager.Find(false, false);
			}
		}

		#endregion

		#region Utils

		public static Color IntToColor(int rgb) {
			return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			} else {
				action.Invoke();
			}
		}




        #endregion

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenSearch();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            menuItem.ForeColor = Color.Black;
        }

        private void ToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            menuItem.ForeColor = Color.White;
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point workAreaPosition = this.PointToClient(Control.MousePosition);
                mouseOff = new Point(-workAreaPosition.X, -workAreaPosition.Y); //得到鼠标偏移量
                leftFlag = true;   //点击左键按下时标注为true;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void menuItemMaximum_Click(object sender, EventArgs e)
        {
            if(this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
                menuItemMaximum.Text = "普通大小";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                menuItemMaximum.Text = "最大化";
            }
        }
    }
}
