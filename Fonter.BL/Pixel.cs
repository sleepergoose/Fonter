using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fonter.BL
{
    public interface IPixel
    {
        event EventHandler PixelMouseDown;
        event EventHandler PixelMouseEnter;

        int Column { get; set; }
        int Row { get; set; }
        Color BackColor { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        Control Parent { get; set; }

        void Show();
    }

    [Serializable]
    public class Pixel : Button, IPixel
    {
        public event EventHandler PixelMouseDown;
        public event EventHandler PixelMouseEnter;

        public int Column { get; set; }
        public int Row { get; set; }

        public Pixel(Color backcolor, int top = 0, int left = 0, int width = 15, int height = 15)
        {
            this.Top = top;
            this.Left = left;
            this.Height = height;
            this.Width = width;
            this.BackColor = backcolor;
            this.Text = "";
            this.FlatStyle = FlatStyle.Flat;
            this.MouseDown += Pixel_MouseDown;
            this.MouseEnter += Pixel_MouseEnter;
        }

        private void Pixel_MouseEnter(object sender, EventArgs e)
        {
            PixelMouseEnter(sender, e);
        }

        private void Pixel_MouseDown(object sender, MouseEventArgs e)
        {
            PixelMouseDown(sender, (MouseEventArgs)e);
        }
    }
}
