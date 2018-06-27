using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;

namespace MinePlug
{
    //对于最小的一块区域定义的类
    public class OnePiece
    {
        private int val;
        public int NumOfPieces;     //周围未点开的块数
        public int NumOfFlags;      //周围flag
        public int NumOfMines;      //周围应有的雷数

        public int state
        {
            get{ return val; }
            set{
                val = value;
                if (val <= 8 && val >= 0)
                {
                    NumOfMines = val;
                }
            }
        }
        public int NumOfMinesLeft
        {
            get { return NumOfMines - NumOfFlags; }
        }
        public int NumOfPiecesLeft
        {
            get { return NumOfPieces - NumOfFlags; }
        }
        public OnePiece()
        {
            NumOfMines = NumOfFlags = NumOfPieces = val = 0;
        }
    }

    //对于整个雷区定义的类
    public class Pieces
    {
        //雷区数据
        public OnePiece[,] board;
        public int row, col;
        public int[] happyFace;

        public int NumOfPieces;     // Pieces that left at on the boart
        public int NumOfFlags;      // Flags that left at on the boart
        public int NumOfMines;      // Mines that left at on the boart

        public Pieces(int row, int col)
        {
            board = new OnePiece[row, col];
            happyFace = new int[2];
            NumOfFlags = NumOfPieces = NumOfMines = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    board[i, j] = new OnePiece();
                }
            }
        }

        //二次初始化
        //为每个块获得周围数据
        public bool CalNums()
        {
            int[] point = new int[2];
            int[] positionsAround;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    positionsAround = RoundPoints(i, j);
                    if (board[i, j].state >= 11)
                        return false;
                    for (int round = 0; positionsAround[round] != -1; round += 2)
                    {
                        point[0] = positionsAround[round];
                        point[1] = positionsAround[round + 1];
                        if (board[point[0], point[1]].state == 0 || board[point[0], point[1]].state == 10)
                        {
                            ++board[i, j].NumOfPieces;
                            if (board[point[0], point[1]].state == 10)
                                ++board[i, j].NumOfFlags;
                        }
                    }
                }
            }
            return true;
        }

        //返回点(x,y)周围点的坐标数组
        public int[] RoundPoints(int x, int y, int r = 1)
        {
            int[] ans = new int[(r * 2 + 1) * (r * 2 + 1) * 2 - 1];
            int top = 0;
            for (int i = Math.Max(0, x - r); i <= Math.Min(row - 1, x + r); i++)
                for (int j = Math.Max(0, y - r); j <= Math.Min(col - 1, y + r); j++)
                    if (i != x || j != y)
                    {
                        ans[top++] = i;
                        ans[top++] = j;
                    }
            ans[top] = -1;
            return ans;
        }
    }


    public partial class main
    {
        static int firstRow = 0, firstCol = 0;
        //插旗操作
        static bool Flag(ref Pieces board)
        {
            bool sthDone = false;
            int[] positionsAround;
            int[] point = new int[2];
            bool[,] vis = new bool[board.row, board.col];
            for (int i = 0; i < board.row; i++)
            {
                for (int j = 0; j < board.col; j++)
                {
                    if (board.board[i, j].NumOfPieces == board.board[i, j].NumOfMines)
                    {
                        positionsAround = board.RoundPoints(i, j);
                        for (int round = 0; positionsAround[round] != -1; round += 2)
                        {
                            point[0] = positionsAround[round];
                            point[1] = positionsAround[round + 1];
                            if (board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                            {
                                Click(point[0], point[1], true);
                                // board.board[point[0], point[1]].state = 10;
                                vis[point[0], point[1]] = true;
                                sthDone = true;
                                //Thread.Sleep(50);
                            }
                        }
                    }
                }
            }
            return sthDone;
        }

        //排雷操作
        static bool DeMine(ref Pieces board)
        {
            bool sthDone = false;
            int[] positionsAround;
            int[] point = new int[2];
            bool[,] vis = new bool[board.row, board.col];
            for (int i = 0; i < board.row; i++)
            {
                for (int j = 0; j < board.col; j++)
                {
                    if (board.board[i, j].NumOfMines != 0 && board.board[i, j].NumOfFlags == board.board[i, j].NumOfMines)
                    {
                        positionsAround = board.RoundPoints(i, j);
                        for (int round = 0; positionsAround[round] != -1; round += 2)
                        {
                            point[0] = positionsAround[round];
                            point[1] = positionsAround[round + 1];
                            if (board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                            {
                                Click(point[0], point[1]);
                                vis[point[0], point[1]] = true;
                                sthDone = true;
                                //Thread.Sleep(50);
                            }
                        }
                    }
                }
            }
            return sthDone;
        }

        static bool InEx(ref Pieces board)
        {
            bool sthDone = false;
            int[] positionsAround2;
            int[] pointB = new int[2], pointA = new int[2], point = new int[2];
            bool[,] vis = new bool[board.row, board.col];

            for (int i = 0; i < board.row; i++)
            {
                for (int j = 0; j < board.col; j++)
                {
                    if (board.board[i, j].state <= 8 && board.board[i, j].state >= 1)
                    {
                        positionsAround2 = board.RoundPoints(i, j, 2);
                        pointA[0] = i;
                        pointA[1] = j;
                        for (int round = 0; positionsAround2[round] != -1; round += 2)
                        {
                            pointB[0] = positionsAround2[round];
                            pointB[1] = positionsAround2[round + 1];
                            if (board.board[pointB[0], pointB[1]].state <= 8 && board.board[pointB[0], pointB[1]].state >= 1)
                            {
                                int[] A = pointA;
                                int[] B = pointB;

                                int[] positionsAroundA, positionsAroundB, positionsAroundC;
                                int[] ar = new int[2];
                                int[] br = new int[2];
                                int a = board.board[A[0], A[1]].NumOfPiecesLeft,
                                    b = board.board[B[0], B[1]].NumOfPiecesLeft,
                                    c,
                                    al = board.board[A[0], A[1]].NumOfMinesLeft,
                                    bl = board.board[B[0], B[1]].NumOfMinesLeft,
                                    clMax, clMin;
                                positionsAroundA = board.RoundPoints(A[0], A[1]);
                                positionsAroundB = board.RoundPoints(B[0], B[1]);
                                positionsAroundC = new int[17];
                                int top = 0;
                                for (int roundA = 0; positionsAroundA[roundA] != -1; roundA += 2)
                                {
                                    ar[0] = positionsAroundA[roundA];
                                    ar[1] = positionsAroundA[roundA + 1];
                                    if (ar[0] != -2 && board.board[ar[0], ar[1]].state == State.PIECEUP)
                                    {
                                        for (int roundB = 0; positionsAroundB[roundB] != -1; roundB += 2)
                                        {
                                            br[0] = positionsAroundB[roundB];
                                            br[1] = positionsAroundB[roundB + 1];
                                            if (br[0] != -2 && board.board[br[0], br[1]].state == State.PIECEUP)
                                            {
                                                if (ar[0] == br[0] && ar[1] == br[1])
                                                {
                                                    positionsAroundC[top++] = ar[0];
                                                    positionsAroundC[top++] = ar[1];
                                                    positionsAroundB[roundB + 1] = positionsAroundB[roundB] = -2;
                                                    positionsAroundA[roundA + 1] = positionsAroundA[roundA] = -2;
                                                }
                                            }
                                        }
                                    }
                                }
                                positionsAroundC[top++] = -1;
                                c = top / 2;
                                clMax = Math.Min(Math.Min(al, bl), c);
                                clMin = Math.Max(Math.Max(al - (a - c), bl - (b - c)), 0);
                                if (clMax == clMin)
                                {
                                    int cl = clMax;
                                    if (a - c == al - cl)
                                    {
                                        for (int roundC = 0; positionsAroundA[roundC] != -1; roundC += 2)
                                        {
                                            point[0] = positionsAroundA[roundC];
                                            point[1] = positionsAroundA[roundC + 1];
                                            if (point[0] != -2 && board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                                            {
                                                Click(point[0], point[1], true);
                                                // board.board[point[0], point[1]].state = 10;
                                                vis[point[0], point[1]] = true;
                                                sthDone = true;
                                                //Thread.Sleep(50);
                                            }
                                        }
                                    }
                                    if (al == cl)
                                    {
                                        for (int roundC = 0; positionsAroundA[roundC] != -1; roundC += 2)
                                        {
                                            point[0] = positionsAroundA[roundC];
                                            point[1] = positionsAroundA[roundC + 1];
                                            if (point[0] != -2 && board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                                            {
                                                Click(point[0], point[1]);
                                                vis[point[0], point[1]] = true;
                                                sthDone = true;
                                                //Thread.Sleep(50);
                                            }
                                        }
                                    }
                                    if (c == cl)
                                    {
                                        for (int roundC = 0; positionsAroundC[roundC] != -1; roundC += 2)
                                        {
                                            point[0] = positionsAroundC[roundC];
                                            point[1] = positionsAroundC[roundC + 1];
                                            if (board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                                            {
                                                Click(point[0], point[1], true);
                                                // board.board[point[0], point[1]].state = 10;
                                                vis[point[0], point[1]] = true;
                                                sthDone = true;
                                                //Thread.Sleep(50);
                                            }
                                        }
                                    }
                                    if (cl == 0)
                                    {
                                        for (int roundC = 0; positionsAroundC[roundC] != -1; roundC += 2)
                                        {
                                            point[0] = positionsAroundC[roundC];
                                            point[1] = positionsAroundC[roundC + 1];
                                            if (board.board[point[0], point[1]].state == 0 && vis[point[0], point[1]] == false)
                                            {
                                                Click(point[0], point[1]);
                                                vis[point[0], point[1]] = true;
                                                sthDone = true;
                                                //Thread.Sleep(50);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sthDone;
        }
        static bool randGo(ref Pieces board)
        {
            bool sthDone = false, firstTime = true;
            int[] positionsAround;
            int[] point = new int[2];
            for (int i = 0; i < board.row; i++)
            {
                for (int j = 0; j < board.col; j++)
                {
                    if (board.board[i, j].NumOfMines != 0)
                    {
                        if (firstTime)
                        {
                            firstTime = false;
                            point[0] = i;
                            point[1] = j;
                        }
                        else if (board.board[point[0], point[1]].NumOfPiecesLeft != 0 && board.board[i, j].NumOfPiecesLeft != 0
                            && board.board[i,j].NumOfMinesLeft * board.board[point[0],point[1]].NumOfPiecesLeft
                            < board.board[i, j].NumOfPiecesLeft * board.board[point[0], point[1]].NumOfMinesLeft)
                        {
                            point[0] = i;
                            point[1] = j;
                        }
                    }
                }
            }
            if (!firstTime)
            {
                positionsAround = board.RoundPoints(point[0], point[1]);
                for (int round = 0; positionsAround[round] != -1; round += 2)
                {
                    point[0] = positionsAround[round];
                    point[1] = positionsAround[round + 1];
                    if (board.board[point[0], point[1]].state == 0)
                    {
                        Click(point[0], point[1]);
                        sthDone = true;
                        break;
                    }
                }
            }
            if (!sthDone)
            {
                int[][] v = new int[board.row * board.col][];
                int top = 0;
                for (int i = 0; i < board.row; i++)
                    for (int j = 0; j < board.col; j++)
                        if (board.board[i, j].state == 0)
                            v[top++] = new int[2] { i, j };
                if (top > 0)
                {
                    top = new Random().Next(top);
                    Click(v[top][0], v[top][1]);
                }
            }
            return sthDone;
        }


    }
}
