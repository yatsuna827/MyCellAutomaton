using System;
using System.Linq;

namespace MyCellAutomaton.ConwayLifeGame
{
    public class LifeGameBoard
    {
        private readonly LifeGameCell[][] board;
        public int Height { get; private set; }
        public int Width { get; private set; }

        /// <summary>
        /// 盤面の世代を進めます.
        /// </summary>
        public void AlternateGeneration()
        {
            foreach (var row in board)
                foreach (var cell in row)
                    cell.CalcNextState();

            foreach (var row in board)
                foreach (var cell in row)
                    cell.GetNext();
        }

        /// <summary>
        /// 指定した座標のセルの状態(生:true, 死:false)を取得します. 座標は左上基準です.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool this[int h, int w] => board[h][w].IsAlive;

        private LifeGameBoard(LifeGameCell[][] board)
        {
            this.board = board;
            this.Height = board.Length;
            this.Width = board[0].Length;
        }

        /// <summary>
        /// 長方形の盤面を作成します. 
        /// </summary>
        /// <param name="height">盤面の縦幅</param>
        /// <param name="width">盤面の横幅</param>
        /// <returns></returns>
        public static LifeGameBoard CreateBoard(int height, int width)
        {
            var board = new LifeGameCell[height][];
            foreach (var i in Enumerable.Range(0, height))
            {
                board[i] = new LifeGameCell[width];
                foreach (var k in Enumerable.Range(0, width))
                {
                    board[i][k] = LifeGameCell.Create(false);
                }
            }

            var empty = LifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, height))
            {
                foreach (var k in Enumerable.Range(0, width))
                {
                    var right = k == board[i].Length - 1 ? empty : board[i][k + 1];
                    var lowerRight = i == board.Length - 1 || k == board[i].Length - 1 ? empty : board[i + 1][k + 1];
                    var lower = i == board.Length - 1 ? empty : board[i + 1][k];
                    var lowerLeft = i == board.Length - 1 || k == 0 ? empty : board[i + 1][k - 1];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }

        /// <summary>
        /// 長方形の盤面を作成します. 
        /// </summary>
        /// <param name="height">盤面の縦幅</param>
        /// <param name="width">盤面の横幅</param>
        /// <param name="initialStateSelector">座標(h,w)の初期状態(生:true, 死:false)を指定する関数</param>
        /// <returns></returns>
        public static LifeGameBoard CreateBoard(int height, int width, Func<int, int, bool> initialStateSelector)
        {
            var board = new LifeGameCell[height][];
            foreach (var i in Enumerable.Range(0, height))
            {
                board[i] = new LifeGameCell[width];
                foreach (var k in Enumerable.Range(0, width))
                {
                    board[i][k] = LifeGameCell.Create(initialStateSelector?.Invoke(i, k) ?? false);
                }
            }

            var empty = LifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, height))
            {
                foreach (var k in Enumerable.Range(0, width))
                {
                    var right = k == board[i].Length - 1 ? empty : board[i][k + 1];
                    var lowerRight = i == board.Length - 1 || k == board[i].Length - 1 ? empty : board[i + 1][k + 1];
                    var lower = i == board.Length - 1 ? empty : board[i + 1][k];
                    var lowerLeft = i == board.Length - 1 || k == 0 ? empty : board[i + 1][k - 1];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }


        /// <summary>
        /// 両端が繋がった長方形の盤面を作成します. 
        /// </summary>
        /// <param name="height">盤面の縦幅</param>
        /// <param name="width">盤面の横幅</param>
        /// <returns></returns>
        public static LifeGameBoard CreateLoopBoard(int height, int width)
        {

            var board = new LifeGameCell[height][];
            foreach (var i in Enumerable.Range(0, height))
            {
                board[i] = new LifeGameCell[width];
                foreach (var k in Enumerable.Range(0, width))
                {
                    board[i][k] = LifeGameCell.Create(false);
                }
            }

            var empty = LifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, height))
            {
                foreach (var k in Enumerable.Range(0, width))
                {
                    var right = board[i][(k + 1) % width];
                    var lowerRight = board[(i + 1) % height][(k + 1) % width];
                    var lower = board[(i + 1) % height][k];
                    var lowerLeft = board[(i + 1) % height][(k - 1 + width) % width];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }

        /// <summary>
        /// 両端が繋がった長方形の盤面を作成します. 
        /// </summary>
        /// <param name="height">盤面の縦幅</param>
        /// <param name="width">盤面の横幅</param>
        /// <param name="initialStateSelector">座標(h,w)の初期状態(生:true, 死:false)を指定する関数</param>
        /// <returns></returns>
        public static LifeGameBoard CreateLoopBoard(int h, int w, Func<int, int, bool> initialStateSelector)
        {

            var board = new LifeGameCell[h][];
            foreach (var i in Enumerable.Range(0, h))
            {
                board[i] = new LifeGameCell[w];
                foreach (var k in Enumerable.Range(0, w))
                {
                    board[i][k] = LifeGameCell.Create(initialStateSelector?.Invoke(i, k) ?? false);
                }
            }

            var empty = LifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, h))
            {
                foreach (var k in Enumerable.Range(0, w))
                {
                    var right = board[i][(k + 1) % w];
                    var lowerRight = board[(i + 1) % h][(k + 1) % w];
                    var lower = board[(i + 1) % h][k];
                    var lowerLeft = board[(i + 1) % h][(k - 1 + w) % w];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }
    }
}
