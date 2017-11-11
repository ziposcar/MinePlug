using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Collections;

namespace MinePlug
{
    public partial class main
    {
        static void Main(string[] args)
        {
            int Case;
            while (true)
            {
                Console.WriteLine("hello?\ninput numbers for what you want:\n1.run the plug of winmine\n0.exit\n");
                Case = Convert.ToInt32(Console.ReadLine());
                bool running = false;
                int[] happyFace = new int[2];
                switch (Case)
                {
                    case 1:
                        int row, col;
                        Console.WriteLine("please input the number of rows:");
                        row = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("please input the number of columns:");
                        col = Convert.ToInt32(Console.ReadLine());
                        Pieces board = null;
                        while (true)
                        {
                            board = init(row, col, ref happyFace);
                            if (!running)
                                Console.WriteLine("running!");
                            if (board == null)
                            {
                                Thread.Sleep(500);
                                board = init(row, col, ref happyFace);
                            }
                            if (board == null)
                            {
                                firstRow = 0;
                                firstCol = 0;
                                Console.WriteLine("faild...");
                                Console.WriteLine("input '1' for retry,\ninput '2' for restart,\ninput '0' for exit:\n");
                                int CaseWhenFaild = Convert.ToInt32(Console.ReadLine());
                                running = false;
                                if (CaseWhenFaild == 1)
                                {
                                    continue;
                                }
                                else if (CaseWhenFaild == 2)
                                {
                                    break;
                                }
                                else if (CaseWhenFaild == 0)
                                {
                                    return;
                                }
                            }
                            else running = true;
                            if (board.CalNums() == false)
                            {
                                if (happyFace[0] == 0)
                                    Console.WriteLine(0);
                                else
                                    Click(happyFace[0] + 15, happyFace[1] + 15, false, true);
                                continue;
                            }
                            // testPrint(ref board);
                            bool sthDone = Flag(ref board);
                            sthDone = DeMine(ref board) || sthDone;
                            if (!sthDone)
                                if (!InEx(ref board))
                                    randGo(ref board);
                        }
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }
        }

        static string output(int v)
        {
            return (v > 9 ? " " : "  ") + v.ToString();
        }

        static void testPrint(ref Pieces board)
        {
            for (int i = 0; i < board.row; i++)
            {
                for (int j = 0; j < board.col; j++)
                {
                    Console.Write(output(board.board[i, j].state));
                }
                Console.WriteLine();
            }
        }
    }

}
