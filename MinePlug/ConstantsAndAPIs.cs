using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace MinePlug
{
    public class State
    {
        public const int PIECEUP = 0;
        public const int PIECE1 = 1;
        public const int PIECE2 = 2;
        public const int PIECE3 = 3;
        public const int PIECE4 = 4;
        public const int PIECE5 = 5;
        public const int PIECE6 = 6;
        public const int PIECE7 = 7;
        public const int PIECE8 = 8;
        public const int PIECEDOWN = 9;
        public const int FLAG = 10;
        public const int MINE = 11;
        public const int REDMINE = 12;
        public const int WRONG = 13;

        public static Bitmap [] index = {
            ImageSources.pieceup,
            ImageSources.piece1,
            ImageSources.piece2,
            ImageSources.piece3,
            ImageSources.piece4,
            ImageSources.piece5,
            ImageSources.piece6,
            ImageSources.piece7,
            ImageSources.piece8,
            ImageSources.piecedown,
            ImageSources.flag,
            ImageSources.mine,
            ImageSources.redmine,
            ImageSources.wrong,
            ImageSources.happy,
            ImageSources.death,
            ImageSources.win,
            ImageSources.facedown,
            ImageSources.morsedown
        };
    }
    public partial class main
    {
        const int MOUSEEVENTF_MOVE = 0x0001;        //移动鼠标
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;    //模拟鼠标左键按下
        const int MOUSEEVENTF_LEFTUP = 0x0004;      //模拟鼠标左键抬起
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;   //模拟鼠标右键按下
        const int MOUSEEVENTF_RIGHTUP = 0x0010;     //模拟鼠标右键抬起
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  //模拟鼠标中键按下
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;    //模拟鼠标中键抬起
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;    //标示是否采用绝对坐标

        static int width = Screen.PrimaryScreen.Bounds.Width;
        static int height = Screen.PrimaryScreen.Bounds.Height;

        private static void Click(int x, int y, bool right = false, bool raw = false)
        {
            if (!raw)
            {
                y = y * 16 + 8 + firstCol;
                x = x * 16 + 8 + firstRow;
            }
            NativeMethods.mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, y * 65536 / width, x * 65536 / height, 0, 0);
            NativeMethods.mouse_event(
                right?
                (MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP)
                :(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP),
                0, 0, 0, 0);
        }
    }
}
