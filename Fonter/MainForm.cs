using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fonter.BL;

namespace Fonter
{
    public interface IMainForm
    {
        // Events
        event EventHandler NewFileClick;
        event EventHandler NewProjectClick;
        event EventHandler OpenProjectClick;
        event EventHandler SaveProjectClick;
        event EventHandler GenerateFCodeClick;
        event EventHandler SaveFileClick;
        //
        event EventHandler DrawFromFile;
        event EventHandler DrawFromOutBuffer;
        event EventHandler DrawFromImage;
        //
        event EventHandler TreeNodeDoubleClick;
        event EventHandler GeneratePCodeClick;
        event EventHandler CloseProjectClick;
        event EventHandler ExitClick;
        event EventHandler ConvertLetterSizeTo;
        event EventHandler ConvertFontSizeTo;
        //
        event EventHandler DeleteNode;

        // Properties
        List<IPixel> ListPixel{get; set; }
        //Dictionary<string, List<IPixel>> DictOfChars { get; set; }
        string ResultText { get; set; }
        int Progress { get; set; }
        int ProgressMaxValue { get; set; }
        Image Image { get; set; }
        string StatusLabel { get; set; }
        Color SetPixelColor { get; set; }
        Color ResetPixelColor { get; set; }

        // Methods
        void FontFieldClear();
        void DrawField(int Rows, int Columns);
        void ClearTree();
        void UpdateTree(Dictionary<string, ImageData> data);
    }

    public partial class MainForm : Form, IMainForm
    {
        #region Declarations

        // Events
        public event EventHandler NewFileClick;
        public event EventHandler NewProjectClick;
        public event EventHandler OpenProjectClick;
        public event EventHandler SaveProjectClick;
        public event EventHandler SaveFileClick;
        public event EventHandler GenerateFCodeClick;
        public event EventHandler GeneratePCodeClick;
        //
        public event EventHandler DrawFromFile;
        public event EventHandler DrawFromOutBuffer;
        public event EventHandler DrawFromImage;
        //
        public event EventHandler TreeNodeDoubleClick;
        public event EventHandler CloseProjectClick;
        public event EventHandler ExitClick;
        public event EventHandler ConvertLetterSizeTo;
        public event EventHandler ConvertFontSizeTo;
        //
        public event EventHandler DeleteNode;

        // Variables 
        private string PreviewPath = Environment.CurrentDirectory;
        private int FieldHeight = 0;
        private int FieldWidth = 0;
        private List<IPixel> listPixel = new List<IPixel>();
        // Color Set
        private Color _SetPixelColor = Color.Green;
        private Color _ResetPixelColor = Color.White;

        // Flag for defining of pressed mouse button
        bool MouseDownLeftFlag = false;
        bool MouseDownRightFlag = false;
        bool SixPixelsFlag = false;

        // Properties
        public List<IPixel> ListPixel { get { return listPixel; } set { listPixel = value; } }
        #endregion
        /* =================================  */

        public MainForm()
        {
            InitializeComponent();
            mnuProjectNew.Click += Menu_Click;
            //
            bmnuNewFile.Click += Menu_Click;
            bmnuClearField.Click += ClearField;
            bmnuFillField.Click += FillField;
            bmnuBuildFile.Click += Menu_Click;
            bmnuBuildProject.Click += Menu_Click;
            bmnuSaveFile.Click += Menu_Click;
            mnuDrawFromOutBuffer.Click += Menu_Click;
            mnuDrawFromFile.Click += Menu_Click;
            // 
            pasteToolStripMenuItem.Click += Menu_Click;
            pasteToolStripButton.Click += Menu_Click;
            copyToolStripMenuItem.Click += Menu_Click;
            copyToolStripButton.Click += Menu_Click;
            cutToolStripMenuItem.Click += Menu_Click;
            cutToolStripButton.Click += Menu_Click;
            //
            bmnuMoveDown.Click += MoveDown;
            bmnuMoveUp.Click += MoveUp;
            bmnuMoveLeft.Click += MoveLeft;
            bmnuMoveRight.Click += MoveRight;
            bmnuSetSixPixels.Click += Menu_Click;

            mnuDrawFromImage.Click += Menu_Click;
            mnuSaveProject.Click += Menu_Click;
            mnuOpenProject.Click += Menu_Click;

            mnuCloseProject.Click += Menu_Click;
            mnuExit.Click += Menu_Click;

            mnuConvertLetterSizeTo.Click += Menu_Click;
            mnuConvertFontSizeTo.Click += Menu_Click;

            cmnuLoadLetter.Click += Menu_Click;
            cmnuDeleteLetter.Click += Menu_Click;

            TreeImageList.NodeMouseDoubleClick += TreeImageList_NodeMouseDoubleClick;
        }

        /// <summary>
        /// MENU CLICK
        /// </summary>
        private void Menu_Click(object sender, EventArgs e)
        {
            string TitleOfMenuItem = "";
            MyEventArgs myEventArgs = new MyEventArgs();
            try
            {
                TitleOfMenuItem = ((ToolStripMenuItem)sender).Text;
            }
            catch (Exception)
            {

                TitleOfMenuItem = ((ToolStripButton)sender).Text;
            }
            string FileFilter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;|Text Files|*.txt;|Bin Files|*.bin;";
            OpenFileDialog openFileDialog = new OpenFileDialog() { InitialDirectory = Environment.CurrentDirectory, Filter = FileFilter, FilterIndex = 1 };
            SaveFileDialog saveFileDialog = new SaveFileDialog() { InitialDirectory = Environment.CurrentDirectory, Filter = FileFilter, FilterIndex = 1 };

            switch (TitleOfMenuItem)
            {
                case "&New File":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    NewFileClick?.Invoke(sender, myEventArgs);
                    break;
                case "&New Project":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    NewProjectClick?.Invoke(sender, myEventArgs);
                    break;
                case "&Save File":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    SaveFileClick?.Invoke(sender, myEventArgs);
                    break;
                case "&Save Project":
                    myEventArgs.Data.Clear();
                    saveFileDialog.FilterIndex = 3;
                    saveFileDialog.InitialDirectory = PreviewPath;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PreviewPath = saveFileDialog.FileName;
                        myEventArgs.Add("FileName", PreviewPath);
                        SaveProjectClick?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "&Open Project":
                    myEventArgs.Data.Clear();
                    openFileDialog.FilterIndex = 3;
                    openFileDialog.InitialDirectory = PreviewPath;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PreviewPath = openFileDialog.FileName;
                        myEventArgs.Add("FileName", PreviewPath);
                        OpenProjectClick?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "Build File":
                    if (FontField.Controls.Count != 0)
                    {
                        myEventArgs.Data.Clear();
                        myEventArgs.Add("MenuItem", TitleOfMenuItem);
                        GenerateFCodeClick?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "Build Project":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    GeneratePCodeClick?.Invoke(sender, myEventArgs);
                    break;
                case "Draw From OutBuffer":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    DrawFromOutBuffer?.Invoke(sender, myEventArgs);
                    break;
                case "Draw From File":
                    myEventArgs.Data.Clear();
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.InitialDirectory = PreviewPath;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.rtbResult.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                        PreviewPath = openFileDialog.FileName;
                        myEventArgs.Add("FileName", PreviewPath);
                        DrawFromFile?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "Draw From Image":
                    myEventArgs.Data.Clear();
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.InitialDirectory = PreviewPath;
                    openFileDialog.Multiselect = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PreviewPath = openFileDialog.FileName;
                        myEventArgs.Add("FileAmount", openFileDialog.FileNames.Count().ToString());
                        int FileCounter = 0;
                        foreach (var item in openFileDialog.FileNames)
                        {
                            myEventArgs.Add($"Image {++FileCounter}", item);
                        }
                        DrawFromImage?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "Close Project":
                    CloseProjectClick?.Invoke(sender, myEventArgs);
                    break;
                case "E&xit":
                    ExitClick?.Invoke(sender, myEventArgs);
                    Application.Exit();
                    break;
                case "&Paste":
                    this.rtbResult.Paste();
                    this.rtbResult.Font = new Font(rtbResult.Font.FontFamily, (float)9);
                    break;
                case "&Copy":
                    Clipboard.SetText(this.rtbResult.Text, TextDataFormat.Text);
                    break;
                case "Cu&t":
                case "C&ut":
                    Clipboard.SetText(this.rtbResult.Text, TextDataFormat.Text);
                    this.rtbResult.Clear();
                    break;
                case "Set Six Pixels Pointer":
                    SixPixelsFlag = !SixPixelsFlag;
                    break;
                case "Convert Letter Size To...":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    ConvertLetterSizeTo?.Invoke(sender, myEventArgs);
                    break;
                case "Convert Font Size To...":
                    myEventArgs.Data.Clear();
                    myEventArgs.Add("MenuItem", TitleOfMenuItem);
                    ConvertFontSizeTo?.Invoke(sender, myEventArgs);
                    break;
                case "Load Letter To Field":
                    if (TreeImageList.SelectedNode != null)
                    {
                        myEventArgs.Data.Clear();
                        myEventArgs.Add("Key", (TreeImageList.SelectedNode.ImageKey));
                        TreeNodeDoubleClick?.Invoke(sender, myEventArgs);
                    }
                    break;
                case "Delete Letter":
                    if (TreeImageList.SelectedNode != null)
                    {
                        myEventArgs.Data.Clear();
                        myEventArgs.Add("Key", (TreeImageList.SelectedNode.ImageKey));
                        DeleteNode?.Invoke(sender, myEventArgs);
                    }
                    break;
                default:
                    Exception exception = new Exception("Such menu item not exist!");
                    MessageBox.Show(exception.Message);
                    break;
            }
        }

        #region DRAW SECTION    
        private void FillField(object sender, EventArgs e)
        {
            this.StatusLabel = "Working...";
            IEnumerable<IPixel> temp = from c in ListPixel
                                       where c.BackColor == _ResetPixelColor
                                       select c;
            foreach (IPixel item in temp)
            {
                item.BackColor = _SetPixelColor;
            }
            this.StatusLabel = "Done!";
        }

        private void ClearField(object sender, EventArgs e)
        {
            this.StatusLabel = "Working...";
            IEnumerable<IPixel> temp = from c in ListPixel
                                       where c.BackColor == _SetPixelColor
                                       select c;
            foreach (IPixel item in temp)
            {
                item.BackColor = _ResetPixelColor;
            }
            this.StatusLabel = "Done!";
        }

        /// <summary>
        /// Clear the whole FontField
        /// </summary>
        public void FontFieldClear()
        {
            this.StatusLabel = "Clearing...";
            ListPixel.Clear();
            FontField.Controls.Clear();
            this.StatusLabel = "Done!";
        }

        /// <summary>
        /// Drawing of the Font Field
        /// </summary>
        /// <param name="Rows">Number of Rows</param>
        /// <param name="Columns">Number of Columns</param>
        public void DrawField(int Rows, int Columns)
        {
            // Переменные уровня формы (view-модели), для хранения размера поля Pixels
            this.FieldHeight = Rows;
            this.FieldWidth = Columns;

            int PixelSize = 15;
            while (Rows * PixelSize >= this.FontField.Height)
            {
                PixelSize = (PixelSize > 9) ? PixelSize - 1 : 9;
            }

            // Top-Left of the Field -> Ordered by Center
            int top = (FontField.Height / 2) - (PixelSize * Rows / 2); ;
            int left = (FontField.Width / 2) - (PixelSize * Columns / 2);

            this.ProgressMaxValue = Rows * Columns;
            int ProcessCounter = 0;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    IPixel pixel = new Pixel(_ResetPixelColor, width: PixelSize, height: PixelSize);
                    pixel.Left = (pixel.Width + 0) * col + left;
                    pixel.Top = (pixel.Height + 0) * row + top;
                    pixel.Column = col;
                    pixel.Row = row;
                    pixel.BackColor = _ResetPixelColor;
                    pixel.Parent = this.FontField;
                    pixel.Show();
                    pixel.PixelMouseDown += Pixel_MouseDown; ;
                    pixel.PixelMouseEnter += Pixel_MouseEnter; ;
                    ListPixel.Add(pixel);
                    this.Progress = ProcessCounter++;
                }
            }
            ShowSize(Rows, Columns);
        }
        
        private void Pixel_MouseEnter(object sender, EventArgs e)
        {
            IPixel pixel = (IPixel)sender;
            sbCoordinates.Text = $"Col = {pixel.Column} | Row = {pixel.Row}";
            if (MouseDownLeftFlag == true)
            {
                if (SixPixelsFlag == true || Control.ModifierKeys == Keys.Alt)
                {
                    MouseDownRightFlag = false;
                    int index = pixel.Row * FieldWidth + pixel.Column;
                    try
                    {
                        ListPixel[index].BackColor = _SetPixelColor;
                        ListPixel[index - 1].BackColor = _SetPixelColor;
                        ListPixel[index + 1].BackColor = _SetPixelColor;
                        ListPixel[index - FieldWidth].BackColor = _SetPixelColor;
                        ListPixel[index - FieldWidth - 1].BackColor = _SetPixelColor;
                        ListPixel[index - FieldWidth + 1].BackColor = _SetPixelColor;
                        ListPixel[index + FieldWidth].BackColor = _SetPixelColor;
                        ListPixel[index + FieldWidth - 1].BackColor = _SetPixelColor;
                        ListPixel[index + FieldWidth + 1].BackColor = _SetPixelColor;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    pixel.BackColor = _SetPixelColor;
                }
                
            }
            else if (MouseDownRightFlag == true)
            {
                if (SixPixelsFlag == true)
                {
                    //MouseDownRightFlag = !MouseDownRightFlag;
                    MouseDownLeftFlag = false;

                    pixel = (Pixel)sender;
                    int index = pixel.Row * FieldWidth + pixel.Column;

                    try
                    {
                        ListPixel[index - 1].BackColor = _ResetPixelColor;
                        ListPixel[index + 1].BackColor = _ResetPixelColor;
                        ListPixel[index - FieldWidth].BackColor = _ResetPixelColor;
                        ListPixel[index - FieldWidth - 1].BackColor = _ResetPixelColor;
                        ListPixel[index - FieldWidth + 1].BackColor = _ResetPixelColor;
                        ListPixel[index + FieldWidth].BackColor = _ResetPixelColor;
                        ListPixel[index + FieldWidth - 1].BackColor = _ResetPixelColor;
                        ListPixel[index + FieldWidth + 1].BackColor = _ResetPixelColor;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    pixel.BackColor = _ResetPixelColor;
                }
            }
        }
        
        private void Pixel_MouseDown(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                MouseButtons mb = ((MouseEventArgs)e).Button;
                if (mb == MouseButtons.Left)
                {
                    MouseDownRightFlag = false;
                    // Fill selected column
                    IEnumerable<IPixel> pixels = from p in ListPixel
                                                 where p.Column == ((IPixel)sender).Column
                                                 select p;
                    foreach (IPixel item in pixels)
                    {
                        item.BackColor = _SetPixelColor;
                    }
                }
                else if (mb == MouseButtons.Right)
                {
                    MouseDownLeftFlag = false;
                    // Fill selected column
                    IEnumerable<IPixel> pixels = from p in ListPixel
                                                 where p.Column == ((IPixel)sender).Column
                                                 select p;
                    foreach (IPixel item in pixels)
                    {
                        item.BackColor = _ResetPixelColor;
                    }
                }
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                MouseButtons mb = ((MouseEventArgs)e).Button;
                if (mb == MouseButtons.Left)
                {
                    MouseDownRightFlag = false;
                    // Fill selected column
                    IEnumerable<IPixel> pixels = from p in ListPixel
                                                 where p.Row == ((IPixel)sender).Row
                                                 select p;
                    foreach (IPixel item in pixels)
                    {
                        item.BackColor = _SetPixelColor;
                    }
                }
                else if (mb == MouseButtons.Right)
                {
                    MouseDownLeftFlag = false;
                    // Fill selected column
                    IEnumerable<IPixel> pixels = from p in ListPixel
                                                 where p.Row == ((IPixel)sender).Row
                                                 select p;
                    foreach (IPixel item in pixels)
                    {
                        item.BackColor = _ResetPixelColor;
                    }
                }
            }
            else
            {
                MouseButtons mb = ((MouseEventArgs)e).Button;
                if (mb == MouseButtons.Left)
                {
                    if (SixPixelsFlag == true || Control.ModifierKeys == Keys.Alt)
                    {
                        MouseDownLeftFlag = !MouseDownLeftFlag;
                        MouseDownRightFlag = false;
                        IPixel pixel = (Pixel)sender;
                        int index = pixel.Row * FieldWidth + pixel.Column;

                        try
                        {
                            ListPixel[index].BackColor = _SetPixelColor;
                            ListPixel[index - 1].BackColor = _SetPixelColor;
                            ListPixel[index + 1].BackColor = _SetPixelColor;
                            ListPixel[index - FieldWidth].BackColor = _SetPixelColor;
                            ListPixel[index - FieldWidth - 1].BackColor = _SetPixelColor;
                            ListPixel[index - FieldWidth + 1].BackColor = _SetPixelColor;
                            ListPixel[index + FieldWidth].BackColor = _SetPixelColor;
                            ListPixel[index + FieldWidth - 1].BackColor = _SetPixelColor;
                            ListPixel[index + FieldWidth + 1].BackColor = _SetPixelColor;
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MouseDownLeftFlag = !MouseDownLeftFlag;
                        MouseDownRightFlag = false;
                        ((Pixel)sender).BackColor = _SetPixelColor;
                    }
                }
                else if (mb == MouseButtons.Right)
                {
                    if (SixPixelsFlag == true || Control.ModifierKeys == Keys.Alt)
                    {
                        MouseDownRightFlag = !MouseDownRightFlag;
                        MouseDownLeftFlag = false; 
                        IPixel pixel = (Pixel)sender;
                        int index = pixel.Row * FieldWidth + pixel.Column;

                        try
                        {
                            ListPixel[index].BackColor = _ResetPixelColor;
                            ListPixel[index - 1].BackColor = _ResetPixelColor;
                            ListPixel[index + 1].BackColor = _ResetPixelColor;
                            ListPixel[index - FieldWidth].BackColor = _ResetPixelColor;
                            ListPixel[index - FieldWidth - 1].BackColor = _ResetPixelColor;
                            ListPixel[index - FieldWidth + 1].BackColor = _ResetPixelColor;
                            ListPixel[index + FieldWidth].BackColor = _ResetPixelColor;
                            ListPixel[index + FieldWidth - 1].BackColor = _ResetPixelColor;
                            ListPixel[index + FieldWidth + 1].BackColor = _ResetPixelColor;
                        }
                        catch (Exception ex)
                        {
                           // MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MouseDownRightFlag = !MouseDownRightFlag;
                        MouseDownLeftFlag = false;
                        ((Pixel)sender).BackColor = _ResetPixelColor;
                    }
                }
            }
        }
        #endregion

        #region Move Section
        /* Move Right */
        private void MoveRight(object sender, EventArgs e)
        {
            Color temp_color = new Color();
            temp_color = _ResetPixelColor;
            List<Color> ListColor = new List<Color>
            {
                temp_color
            };

            for (int i = 0; i < ListPixel.Count - 1; i++)
            {
                ListColor.Add(ListPixel[i].BackColor);
            }

            for (int i = 0; i < ListPixel.Count; i++)
            {
                ListPixel[i].BackColor = ListColor[i];
            }
        }

        /* Move Left */
        private void MoveLeft(object sender, EventArgs e)
        {
            List<Color> ListColor = new List<Color>();

            for (int i = 1; i < ListPixel.Count; i++)
            {
                ListColor.Add(ListPixel[i].BackColor);
            }
            Color temp_color = new Color();
            temp_color = _ResetPixelColor;
            ListColor.Add(temp_color);

            for (int i = 0; i < ListPixel.Count; i++)
            {
                ListPixel[i].BackColor = ListColor[i];
            }
        }

        /* Move Up */
        private void MoveUp(object sender, EventArgs e)
        {
            Color[] ArrayColor = new Color[FieldHeight * FieldWidth];
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //watch.Start();
            for (int i = 0; i < ListPixel.Count - FieldWidth; i++)
            {
                ArrayColor[i] = ListPixel[i + FieldWidth].BackColor;
            }

            for (int i = 0; i < ListPixel.Count - FieldWidth; i++)
            {
                ListPixel[i].BackColor = ArrayColor[i];
            }
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //IMessager msg = new Messager();
            //msg.ShowMessage((elapsedMs).ToString());
        }

        /* Move Down */
        private void MoveDown(object sender, EventArgs e)
        {
            Color[] ArrayColor = new Color[FieldHeight * FieldWidth];

            for (int i = 0; i < ListPixel.Count - FieldWidth; i++)
            {
                ArrayColor[i + FieldWidth] = ListPixel[i].BackColor;
            }

            for (int i = 0; i < ListPixel.Count; i++)
            {
                ListPixel[i].BackColor = ArrayColor[i];
            }
        }
        #endregion


        #region Tree Section
        private void TreeImageList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MyEventArgs myEventArgs = new MyEventArgs();
            myEventArgs.Add("Key", ((TreeView)sender).SelectedNode.ImageKey);
            TreeNodeDoubleClick?.Invoke(sender, myEventArgs);
        }

        public void UpdateTree(Dictionary<string, ImageData> data)
        {
            imageList.Images.Clear();
            foreach (var item in data)
            {
                imageList.Images.Add(item.Key, item.Value.Image);
            }
            foreach (var item in data)
            {
                TreeImageList.Nodes.Add(item.Key, item.Key, item.Key, item.Key).ContextMenuStrip = cmnuTreeView;
            }
        }

        public void ClearTree()
        {
            TreeImageList.Nodes.Clear();
        }
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Property: Get or Set text of the ResultText (RTF)
        /// </summary>
        public string ResultText
        {
            get
            {
                return rtbResult.Text;
            }
            set
            {
                rtbResult.Text = value;
            }
        }

        public int Progress
        {
            get
            {
                return sbProgressBar.Value;
            }
            set
            {
                sbProgressBar.Value = value;
            }
        }

        public int ProgressMaxValue {
            get {
                return sbProgressBar.Maximum;
            }
            set {
                sbProgressBar.Maximum = value;
            }
        }

        public string StatusLabel
        {
            get
            {
                return sbStatusLabel.Text;
            }
            set
            {
                sbStatusLabel.Text = value;
            }
        }

        public Image Image {
            get
            {
                return pbPreviewImage.Image;
            }
            set
            {
                pbPreviewImage.Image = value;
            }
        }

        public Color SetPixelColor { get { return _SetPixelColor; } set { _SetPixelColor = value; } }
        public Color ResetPixelColor { get { return _ResetPixelColor; } set { _ResetPixelColor = value; } }
        #endregion

        private void ShowSize(int rows, int columns)
        {
            this.sbSize.Text = $"Columns = {columns} | Rows = {rows}";
        }
    }
}
