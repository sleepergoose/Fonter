using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fonter.BL;

namespace Fonter
{
    class Presenter
    {


        Dictionary<string, ImageData> LetterDict;
        bool IsProjectMode = false;

        private readonly IMainForm _view;
        private readonly IMessager _messager;
        private readonly ILogic _logic;

        private int Rows = 0;
        private int Columns = 0;
        // Global Index for indexation of the ImageList through _view_SaveFileClick()
        private int GlobalImageListIndex = 0;

        /* Constructor */
        public Presenter(IMainForm view, IMessager messager, ILogic logic)
        {
            _view = view;
            _messager = messager;
            _logic = logic;

            _view.GenerateFCodeClick += _view_GenerateFCodeClick;
            _view.DrawFromOutBuffer += _view_DrawFromOutBuffer;
            _view.DrawFromFile += _view_DrawFromOutBuffer;
            _view.NewFileClick += _view_NewFileClick;
            _view.DrawFromImage += _view_DrawFromImage;
            _view.NewProjectClick += _view_NewProjectClick;
            _view.SaveFileClick += _view_SaveFileClick;
            _view.SaveProjectClick += _view_SaveProjectClick;
            _view.OpenProjectClick += _view_OpenProjectClick;
            _view.TreeNodeDoubleClick += _view_TreeNodeDoubleClick;
            _view.GeneratePCodeClick += _view_GeneratePCodeClick;
            _view.ExitClick += _view_ExitClick;
            _view.CloseProjectClick += _view_CloseProjectClick;
            _view.ConvertFontSizeTo += _view_ConvertFontSizeTo;
            _view.ConvertLetterSizeTo += _view_ConvertLetterSizeTo;
            _view.DeleteNode += _view_DeleteNode;
        }

        private void _view_DeleteNode(object sender, EventArgs e)
        {
            Wait();
            LetterDict.Remove(((MyEventArgs)e).Data["Key"]);
            _view.ClearTree();
            _view.UpdateTree(LetterDict);
            StopWait();
        }

        private void _view_ConvertLetterSizeTo(object sender, EventArgs e)
        {
            Wait();
            DialogForm dialogForm = new DialogForm
            {
                Text = "Dialog: New Resolution",
                Content = "Enter desired resolution below:",
                NewFontHeight = Rows,
                NewFontWidth = Columns
                
            };
            if (dialogForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // TODO:
                string StrBinCode = _logic.GenerateCode(_view.ListPixel, _view.SetPixelColor);
                Image temp_image = _logic.ConvertStrBinToImage(StrBinCode, Columns, Rows);
                Columns = dialogForm.NewFontWidth;
                Rows = dialogForm.NewFontHeight;
                temp_image = new Bitmap(temp_image, new Size(Columns, Rows));
                List<char> temp_pixels = _logic.ConvertImageToListPixels(temp_image, Columns, Rows);

                _view.FontFieldClear();
                _view.DrawField(dialogForm.NewFontHeight, dialogForm.NewFontWidth);
                RefreshPixelField(temp_pixels);
            }
            StopWait();
        }

        private void _view_ConvertFontSizeTo(object sender, EventArgs e)
        {
            Wait();
            DialogForm dialogForm = new DialogForm
            {
                Text = "Dialog: New Resolution",
                Content = "Enter desired resolution below:",
                NewFontHeight = Rows,
                NewFontWidth = Columns
            };

            if (LetterDict != null && dialogForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Dictionary<string, ImageData> ResultDict = new Dictionary<string, ImageData>();
                    foreach (var item in LetterDict)
                    {
                        Columns = dialogForm.NewFontWidth;
                        Rows = dialogForm.NewFontHeight;
                        Image temp_image = new Bitmap(item.Value.Image, new Size(Columns, Rows));
                        List<char> temp_pixels = _logic.ConvertImageToListPixels(temp_image, Columns, Rows);
                        string tempBinCode = "";
                        foreach (var c in temp_pixels)
                        {
                            tempBinCode += c;
                        }

                        string code = _logic.ConvertToHex(tempBinCode);

                        code = $"Height = {Rows} \n" +
                        $"Width = {Columns} \n" +
                        $"{code.Replace('\n', ',')}";

                        ResultDict.Add(item.Key, new ImageData(item.Value.Index, temp_image, code));
                    }
                    _view.ClearTree();
                    LetterDict.Clear();
                    LetterDict = ResultDict;
                    _view.UpdateTree(ResultDict);
            }
            else
            {
                _messager.ShowExclamation("This command for Project mode! \n Use menu comand: Convert Letter Size To...");
            }
            StopWait();
        }

        /* Close Project Click */
        private void _view_CloseProjectClick(object sender, EventArgs e)
        {
            if (IsProjectMode == true)
            {
                Wait();
                _view.ClearTree();
                _view.FontFieldClear();
                IsProjectMode = false;
                LetterDict.Clear();
                GlobalImageListIndex = 0;
                Rows = 0;
                Columns = 0;
                _view.ListPixel.Clear();
                _view.ResultText = "";
                StopWait();
            }
        }

        /* Exit Click */
        private void _view_ExitClick(object sender, EventArgs e)
        {
            _messager.ShowMessage("good bye");
        }

        /*Generate PCode Click */
        private void _view_GeneratePCodeClick(object sender, EventArgs e)
        {
            if (IsProjectMode == true)
            {
                Wait();
                string respons = "";
                if ((respons = _logic.BuildProject(LetterDict, Rows, Columns)) != null)
                {
                    _view.ResultText = respons;
                }
                StopWait();
            }
        }

        /* Tree Node Double Click */
        private void _view_TreeNodeDoubleClick(object sender, EventArgs e)
        {
            Wait();
            string Key = ((MyEventArgs)e).Data["Key"];
 
            string[] TextForParsing = LetterDict[Key].Code.Split('\n');
            Rows = int.Parse(TextForParsing[0].Split('=')[1]);
            Columns = int.Parse(TextForParsing[1].Split('=')[1]);
            // Clear The Font Field
            _view.FontFieldClear();
            // Draw The Font Field
            _view.DrawField(Rows, Columns);

            List<char> list = _logic.ConvertTextToListPixels(LetterDict[Key].Code);

            RefreshPixelField(list);
            StopWait();
        }

        /* Open Project Click */
        private void _view_OpenProjectClick(object sender, EventArgs e)
        {
            Wait();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(((MyEventArgs)e).Data["FileName"], FileMode.Open, FileAccess.Read, FileShare.Read);
            LetterDict = (Dictionary<string, ImageData>)formatter.Deserialize(stream);
            stream.Close();

            Rows = int.Parse(LetterDict.First().Value.Code.Split('\n')[0].Split('=')[1]);
            Columns = int.Parse(LetterDict.First().Value.Code.Split('\n')[1].Split('=')[1]);

            _view.FontFieldClear();
            GlobalImageListIndex = 0;
            _view.ListPixel.Clear();
            _view.ResultText = "";
            _view.ClearTree();
            _view.UpdateTree(LetterDict);
            IsProjectMode = true;
            StopWait();
        }

        /* Save Project Click */
        private void _view_SaveProjectClick(object sender, EventArgs e)
        {
            if (IsProjectMode == true)
            {
                Wait();
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(((MyEventArgs)e).Data["FileName"], FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, LetterDict);
                stream.Close();
                StopWait();
            }
        }

        /* Save File Click */
        private void _view_SaveFileClick(object sender, EventArgs e)
        {
            Wait();
            InputDialog inputDialog = new InputDialog();
            if (IsProjectMode == true && inputDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK  && inputDialog.Value != "")
            {
                string StrBinCode = _logic.GenerateCode(_view.ListPixel, _view.SetPixelColor);
                string StrHexCode = _logic.ConvertToHex(StrBinCode);

                _view.ResultText = $"Height = {Rows} \n" +
                    $"Width = {Columns} \n" +
                    $"{StrHexCode.Replace('\n', ',')}";

                 Image img_temp = _logic.ConvertStrBinToImage(StrBinCode, Columns, Rows);
                _view.Image = img_temp;

                if (!LetterDict.ContainsKey(inputDialog.Value))
                {
                    LetterDict.Add(inputDialog.Value, new ImageData(GlobalImageListIndex, img_temp, _view.ResultText));
                    GlobalImageListIndex++;
                }
                else
                {
                    LetterDict[inputDialog.Value].Code = _view.ResultText;
                    LetterDict[inputDialog.Value].Image = img_temp;
                }
                _view.ClearTree();
                _view.UpdateTree(LetterDict);
            }
            StopWait();
        }

        /* New Project Click */
        private void _view_NewProjectClick(object sender, EventArgs e)
        {
            DialogForm dialogForm = new DialogForm();
            if (dialogForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Wait();
                Rows = dialogForm.NewFontHeight;
                Columns = dialogForm.NewFontWidth;

                // Clear The Font Field
                _view.FontFieldClear();
                // Draw The Font Field
                _view.DrawField(Rows, Columns);
                StopWait();
            }

            LetterDict = new Dictionary<string, ImageData>();
            _view.ClearTree();
            IsProjectMode = true;
            GlobalImageListIndex = 0;

        }

        /* Draw font letter from Image */
        private void _view_DrawFromImage(object sender, EventArgs e)
        {
            Wait();
            string FileName = ((MyEventArgs)e).Data["Image 1"];
            if (IsProjectMode == true && Rows != 0 && Columns != 0 && ((MyEventArgs)e).Data.Count() > 2)
            {
                _view.FontFieldClear();
                foreach (var item in ((MyEventArgs)e).Data.Values.Where(i => i.Contains('\\')))
                {
                    List<char> list = _logic.ConvertImageToListPixels(item, Columns, Rows, out Image img);
                    string StrBinCode = _logic.GenerateBinCodeFromCharList(list);
                    string StrHexCode = _logic.ConvertToHex(StrBinCode);

                    _view.ResultText = $"Height = {Rows} \n" +
                    $"Width = {Columns} \n" +
                    $"{StrHexCode.Replace('\n', ',')}";

                    Image img_temp = _logic.ConvertStrBinToImage(StrBinCode, Columns, Rows);
                    _view.Image = img_temp;
                    string name = item.Split('\\').Last().Split('.')[0];

                    if (!LetterDict.ContainsKey(name))
                    {
                        LetterDict.Add(name, new ImageData(GlobalImageListIndex, img_temp, _view.ResultText));
                        GlobalImageListIndex++;
                    }
                    else
                    {
                        LetterDict[name].Code = _view.ResultText;
                        LetterDict[name].Image = img_temp;
                    }                 
                }
                _view.ClearTree();
                _view.UpdateTree(LetterDict);
            }
            else if (Rows != 0 && Columns != 0)
            {
                IEnumerable<IPixel> pixels = from pixel in _view.ListPixel
                                             where pixel.BackColor == _view.SetPixelColor
                                             select pixel;
                foreach (IPixel item in pixels)
                {
                    item.BackColor = _view.ResetPixelColor;
                }
                List<char> list = _logic.ConvertImageToListPixels(FileName, Columns, Rows, out Image img);
                _view.Image = img;
                RefreshPixelField(list);
            }
            else
            {
                _messager.ShowError("Field is Empty! \n You have to create new field");
            }
            StopWait();
        }

        /* Generate New Field */
        private void _view_NewFileClick(object sender, EventArgs e)
        {
            Wait();
            DialogForm dialogForm = new DialogForm();
            if (dialogForm.ShowDialog() == System.Windows.Forms.DialogResult.OK && IsProjectMode != true)
            {
                
                Rows = dialogForm.NewFontHeight;
                Columns = dialogForm.NewFontWidth;

                // Clear The Font Field
                _view.FontFieldClear();
                // Draw The Font Field
                _view.DrawField(Rows, Columns);
               
            }
            StopWait();
        }

        private void Wait()
        {
            _view.StatusLabel = "Working...";
        }

        private void StopWait()
        {
            _view.StatusLabel = "Done!";
        }

        /* Draw from Buffer */
        private void _view_DrawFromOutBuffer(object sender, EventArgs e)
        {
            Wait();
            string[] TextForParsing = _view.ResultText.Split('\n');
            Rows = int.Parse(TextForParsing[0].Split('=')[1]);
            Columns = int.Parse(TextForParsing[1].Split('=')[1]);
            // Clear The Font Field
            _view.FontFieldClear();
            // Draw The Font Field
            _view.DrawField(Rows, Columns);

            List<char> list = _logic.ConvertTextToListPixels(_view.ResultText);

            RefreshPixelField(list);
            StopWait();
        }

        /* Generate Code */
        private void _view_GenerateFCodeClick(object sender, EventArgs e)
        {
            string StrBinCode = _logic.GenerateCode(_view.ListPixel, _view.SetPixelColor);
            string StrHexCode = _logic.ConvertToHex(StrBinCode);
                
            _view.ResultText = $"Height = {Rows} \n" +
                $"Width = {Columns} \n" +
                $"{StrHexCode.Replace('\n', ',')}";

            _view.Image = _logic.ConvertStrBinToImage(StrBinCode, Columns, Rows);
        }

        /* Refresh Pixel Field */
        private void RefreshPixelField(List<char> list)
        {
            for (int i = 0, n = Rows * Columns; i < n; i++)
            {
                if (list[i] == '1')
                {
                    _view.ListPixel[i].BackColor = _view.SetPixelColor;
                }
            }
        }
    }
}
