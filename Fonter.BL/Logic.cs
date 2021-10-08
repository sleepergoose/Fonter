using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Drawing;


namespace Fonter.BL
{
    public interface ILogic
    {
        string GenerateCode(List<IPixel> ListPixels, Color SetColor);
        string ConvertToHex(string StringBinary);
        List<char> ConvertTextToListPixels(string TextForParsing);
        List<char> ConvertImageToListPixels(string fileName, int column, int row, out Image img);
        List<char> ConvertImageToListPixels(Image image, int column, int row);
        Image ConvertStrBinToImage(string BinaryString, int Columns, int Rows);
        string BuildProject(Dictionary<string, ImageData> LetterDict, int Rows, int Columns);
        string GenerateBinCodeFromCharList(List<char> list);
    }

    public class Logic : ILogic
    {
        public string GenerateBinCodeFromCharList(List<char> list)
        {
            string strBin = "";
            foreach (var item in list)
            {
                strBin += item;
            }
            if (strBin.Length % 32 != 0)
            {
                int temp = 32 - strBin.Length % 32;
                //return strBin.PadLeft(temp, '0');
                for (int i = 0; i < temp; i++)
                {
                    strBin += "0";
                }
            }
            return strBin;
        }


        /// <summary>
        /// Генератор строки с бинарным представлением массива пикселей
        /// </summary>
        /// <param name="ListPixels">List of Pixel from Field</param>
        /// <returns></returns>
        public string GenerateCode(List<IPixel> ListPixels, Color SetColor)
        {
            // Variables for string binary result  
            string strBin = "";
            foreach (IPixel item in ListPixels)
            {
                if (item.BackColor == SetColor)
                {
                    strBin += "1";
                }
                else
                {
                    strBin += "0";
                }
            }

            /* Backward order */
            //for (int i = ListPixels.Count - 1; i >= 0; i--)
            //{
            //    if (ListPixels[i].BackColor == Color.Green)
            //    {
            //        strBin += "1";
            //    }
            //    else
            //    {
            //        strBin += "0";
            //    }
            //}
            if (strBin.Length % 32 != 0)
            {
                int temp = 32 - strBin.Length % 32;
                //return strBin.PadLeft(temp, '0');
                for (int i = 0; i < temp; i++)
                {
                    strBin += "0";
                }
            }
            return strBin;
        }

        /// <summary>
        /// Method for building Project Code
        /// </summary>
        /// <param name="LetterDict">Main project dictionary</param>
        /// <param name="Rows">Height of letter in font</param>
        /// <param name="Columns">Width of letter in font</param>
        /// <returns>Total font code</returns>
        public string BuildProject(Dictionary<string, ImageData>  LetterDict, int Rows, int Columns)
        {
            if (LetterDict.Count != 0)
            {
                int resolution = LetterDict.First().Value.Code.Split('\n')[2].Split(',').Where(i => i.Contains('x')).Count();
                string result = $"uint32_t font{Rows}x{Columns}[][{resolution}] = " + "{ ";

                foreach (var item in LetterDict)
                {
                    string temp = "{" + item.Value.Code.Split('\n')[2].Substring(0, item.Value.Code.Split('\n')[2].Length - 1) + "}, \n";
                    result += temp;
                }
                result = result.Substring(0, result.Length - 3);
                result += " }";
                return result;
            }
            else
            {
                return null;
            }          
        }

        /// <summary>
        /// Convert string view of binary to string view of hex
        /// </summary>
        /// <param name="StringBinary">String for convertion</param>
        /// <returns></returns>
        public string ConvertToHex(string StringBinary)
        {
            string result = "";

            for (int j = 0, n = StringBinary.Length / 32; j < n; j++)
            {
                UInt32 sum = 0;
                string subString = StringBinary.Substring(32 * j, 32);
                
                for (int i = 0; i < subString.Length; i++)
                {
                    sum += (UInt32)(Math.Pow(2, 31 - i) * (subString[i] - '0'));
                }
                result += "0x" + sum.ToString("x") + "\n";
            }
            return result;
        }

        /// <summary>
        /// Convert given text to List<char> - Array from Out Buffer
        /// </summary>
        /// <param name="TextForParsing">Text for conversation</param>
        /// <returns></returns>
        public List<char> ConvertTextToListPixels(string TextForParsing)
        {
            List<char> list = new List<char>();

            string[] text = TextForParsing.Split('\n');
            int Rows = int.Parse(text[0].Split('=')[1]);
            int Columns = int.Parse(text[1].Split('=')[1]);
            string[] raw = text[2].Split(',');
            var str = raw.Where(i => i.Contains("x"));
            string[] hex_arr = str.ToArray();

            foreach (var item in hex_arr)
            {
                string r = item.Split('x')[1]; //.Split(',')[0];
                UInt32 num = Convert.ToUInt32(r, 16);

                for (int j = 0; j < 32; j++)
                {
                    UInt32 w = (UInt32)(1 << 31 - j);
                    if ((num & w) == w)
                    {
                        list.Add('1');
                    }
                    else
                    {
                        list.Add('0');
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Convert image to List of char ('1' or '0')
        /// </summary>
        /// <param name="fileName">Total path to Bitmap</param>
        /// <param name="imgWidth">Width of need Pixel Field</param>
        /// <param name="imgHeight">Height of need Pixel Field</param>
        /// <returns>List<char></returns>
        public List<char> ConvertImageToListPixels(string fileName, int imgWidth, int imgHeight, out Image img)
        {
            Bitmap bitmap = new Bitmap(new Bitmap(fileName), new Size(imgWidth, imgHeight)); 
            List<char> list = new List<char>();
            img = bitmap;
            for (int y = 0, height = bitmap.Height; y < height; y++)
            {
                for (int x = 0, width = bitmap.Width; x < width; x++)
                {
                    byte R = bitmap.GetPixel(x, y).R;
                    byte G = bitmap.GetPixel(x, y).G;
                    byte B = bitmap.GetPixel(x, y).B;
                    //float br = bitmap.GetPixel(x, y).GetBrightness();

                    if (R < 200 && G < 200 && B < 200)
                    {
                        list.Add('1');
                    }
                    else
                    {
                        list.Add('0');
                    }
                }
            }
            return list;
        }

        public List<char> ConvertImageToListPixels(Image bitmap, int column, int row)
        {
            List<char> list = new List<char>();
                        for (int y = 0, height = bitmap.Height; y < height; y++)
            {
                for (int x = 0, width = bitmap.Width; x < width; x++)
                {
                    byte R = ((Bitmap)bitmap).GetPixel(x, y).R;
                    byte G = ((Bitmap)bitmap).GetPixel(x, y).G;
                    byte B = ((Bitmap)bitmap).GetPixel(x, y).B;
                    //float br = bitmap.GetPixel(x, y).GetBrightness();

                    if (R < 200 && G < 200 && B < 200)
                    {
                        list.Add('1');
                    }
                    else
                    {
                        list.Add('0');
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Convertion String Binary Code to Image
        /// </summary>
        /// <param name="BinaryString">String Binary Code</param>
        /// <param name="Columns">Image width</param>
        /// <param name="Rows">Image height</param>
        /// <returns>New Image</returns>
        public Image ConvertStrBinToImage(string BinaryString, int Columns, int Rows)
        {
            Bitmap bitmap = new Bitmap(Columns, Rows);
            int i = 0;
            for (int y = 0, height = bitmap.Height; y < height; y++)
            {
                for (int x = 0, width = bitmap.Width; x < width; x++)
                {
                    if (BinaryString[i++] == '1')
                    {
                        bitmap.SetPixel(x, y, Color.Green);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }
            return bitmap;
        }
    }

    [Serializable]
    public class ImageData
    {
        public int Index { get; set; }
        public Image Image { get; set; }
        public string Code { get; set; }

        public ImageData(int _Index, Image _Image, string _Code)
        {
            this.Index = _Index;
            this.Image = _Image;
            this.Code = _Code;
        }
    }
}
