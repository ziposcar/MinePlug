using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MinePlug
{
    public partial class main
    {
        static Pieces init(int row, int col, ref int[] happyFace)
        {
            Pieces board = new Pieces(row, col);
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Bitmap screenCut = new Bitmap(width, height);
            Graphics Front = Graphics.FromImage(screenCut);
            Front.CopyFromScreen(0, 0, 0, 0, Screen.AllScreens[0].Bounds.Size);
            Front.Dispose();

            LockBitmap baseScreen = new LockBitmap(screenCut);
            baseScreen.LockBits();

            LockBitmap tmp = null;
            bool Catch, catchTmp;
            Catch = false;
            int rBegin = 0, cBegin = 0;
            bool begin = false;
            if (firstRow == 0)
            {
                for (int faceRound = 14; faceRound < 19; ++faceRound)
                {
                    tmp = new LockBitmap(State.index[faceRound]);
                    tmp.LockBits();
                    for (int r = 0; r < height; r++)
                    {
                        for (int c = 0; c < width; c++)
                        {
                            if (26 + c < width && 26 + r < height)
                            {
                                catchTmp = true;
                                for (int rr = 0; rr < 26; rr += 2)
                                {
                                    for (int cc = 0; cc < 26; cc += 2)
                                    {
                                        if (tmp.GetPixel(cc, rr) != baseScreen.GetPixel(cc + c, rr + r))
                                        {
                                            catchTmp = false;
                                            break;
                                        }
                                    }
                                    if (!catchTmp) break;
                                }
                                if (catchTmp)
                                {
                                    happyFace[0] = board.happyFace[0] = r;
                                    firstRow = r + 40;
                                    happyFace[1] = board.happyFace[1] = firstCol = c;
                                    begin = true;
                                    tmp.UnlockBits();
                                    break;
                                }
                            }
                        }
                        if (begin) break;
                    }
                    if (begin)
                    {
                        if (faceRound == 14) break;
                        else
                        {
                            Click(happyFace[0] + 15, happyFace[1] + 15, false, true);
                            System.Threading.Thread.Sleep(50);
                            Click(happyFace[0] + 15, happyFace[1] + 15, false, true);
                            break;
                        }
                    }
                    tmp.UnlockBits();
                }
                if (!begin)
                {
                    tmp.UnlockBits();
                    //Console.WriteLine("line 76");
                    return null;
                }
                for (int r = firstRow; r < height; r++)
                {
                    for (int c = cBegin; c < width; c++)
                    {
                        if (16 + c < width && 16 + r < height)
                        {
                            for (int i = 0; i < 14; i++)
                            {
                                tmp = new LockBitmap(State.index[i]);
                                tmp.LockBits();
                                catchTmp = true;
                                for (int rr = 0; rr < 16; ++rr)
                                {
                                    for (int cc = 0; cc < 16; ++cc)
                                    {
                                        if (tmp.GetPixel(cc, rr) != baseScreen.GetPixel(cc + c, rr + r))
                                        {
                                            catchTmp = false;
                                            break;
                                        }
                                    }
                                    if (!catchTmp)
                                        break;
                                }
                                if (catchTmp)
                                {
                                    firstRow = r;
                                    firstCol = c;
                                    Catch = true;
                                    tmp.UnlockBits();
                                    break;
                                }
                                tmp.UnlockBits();
                            }
                        }
                        if (Catch)
                            break;
                    }
                    if (Catch)
                        break;
                }
                if (!Catch)
                {
                    //Console.WriteLine("line 122");
                    return null;
                }
            }
            int Row = 0, Col = 0;
            for (int r = firstRow; r < height; r += 16)
            {
                for (int c = firstCol; c < width; c += 16)
                {
                    Catch = false;
                    for (int i = 0; i < 14; i++)
                    {
                        tmp = new LockBitmap(State.index[i]);
                        tmp.LockBits();
                        if (16 + c < width && 16 + r < height)
                        {
                            catchTmp = true;
                            for (int rr = 0; rr < 16; rr += 2)
                            {
                                for (int cc = 0; cc < 16; cc += 2)
                                {
                                    if (tmp.GetPixel(cc, rr) != baseScreen.GetPixel(cc + c, rr + r))
                                    {
                                        catchTmp = false;
                                        break;
                                    }
                                }
                                if (!catchTmp)
                                    break;
                            }
                            if (catchTmp)
                            {
                                board.board[Row, Col].state = i;
                                Catch = true;
                                tmp.UnlockBits();
                                break;
                            }
                        }
                        tmp.UnlockBits();
                    }
                    if (Catch)
                        ++Col;
                    else
                        break;
                }
                ++Row;
                if (Row == row)
                    break;
                if (Col != col)
                {
                    //Console.WriteLine("line 172");
                    //screenCut.Save("C:\\Users\\dell\\Desktop\\172.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    return null;
                }
                Col = 0;
            }
            if (Row != row)
            {
                //Console.WriteLine("line 179");
                return null;
            }

            board.row = Row;
            board.col = Col;

            return board;
        }
    }
}
