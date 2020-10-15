using System.Linq;

namespace MyCellAutomaton.ConwayLifeGame
{
    /// <summary>
    /// 近傍をムーア近傍(周囲8マス)で定めるライフゲームのセルを表すクラス.
    /// </summary>
    public class LifeGameCell
    {
        private readonly LifeGameCell[] neighborhoods;

        /// <summary>
        /// 左上に隣接するセル.
        /// </summary>
        public virtual LifeGameCell UpperLeft { get { return neighborhoods[0]; } set { neighborhoods[0] = value; } }
        /// <summary>
        /// 上に隣接するセル.
        /// </summary>
        public virtual LifeGameCell Upper { get { return neighborhoods[1]; } set { neighborhoods[1] = value; } }
        /// <summary>
        /// 右上に隣接するセル.
        /// </summary>
        public virtual LifeGameCell UpperRight { get { return neighborhoods[2]; } set { neighborhoods[2] = value; } }
        /// <summary>
        /// 左に隣接するセル.
        /// </summary>
        public virtual LifeGameCell Left { get { return neighborhoods[3]; } set { neighborhoods[3] = value; } }
        /// <summary>
        /// 右に隣接するセル.
        /// </summary>
        public virtual LifeGameCell Right { get { return neighborhoods[4]; } set { neighborhoods[4] = value; } }
        /// <summary>
        /// 左下に隣接するセル.
        /// </summary>
        public virtual LifeGameCell LowerLeft { get { return neighborhoods[5]; } set { neighborhoods[5] = value; } }
        /// <summary>
        /// 下に隣接するセル.
        /// </summary>
        public virtual LifeGameCell Lower { get { return neighborhoods[6]; } set { neighborhoods[6] = value; } }
        /// <summary>
        /// 右下に隣接するセル.
        /// </summary>
        public virtual LifeGameCell LowerRight { get { return neighborhoods[7]; } set { neighborhoods[7] = value; } }

        /// <summary>
        /// セルの生存状態.
        /// </summary>
        public virtual bool IsAlive { get { return currentState.IsAlive; } }

        private CellState currentState = CellState.GetEmptyState();
        private CellState nextState = CellState.GetEmptyState();

        private int CountAliveNeighborhoods() => neighborhoods.Count(_ => _.IsAlive);

        /// <summary>
        /// 次の状態を更新します. 現在の状態は変化しません.
        /// </summary>
        public void CalcNextState() { nextState = currentState.GetNextState(CountAliveNeighborhoods()); }

        /// <summary>
        /// 現在の状態を更新します. 必ず先にCalcNextState()を呼び出しておいてください.
        /// </summary>
        public void GetNext() { currentState = nextState; }

        /// <summary>
        /// 近傍を全て空のセルで埋めたセルを生成します.
        /// </summary>
        /// <param name="isAlive">生成するセルの初期状態.</param>
        /// <returns></returns>
        public static LifeGameCell Create(bool isAlive = false)
        {
            return new LifeGameCell(isAlive, Enumerable.Repeat<LifeGameCell>(EmptyLifeGameCell.Instance, 8).ToArray());
        }

        protected LifeGameCell() { }

        private LifeGameCell(bool isAlive, LifeGameCell[] cells)
        {
            this.neighborhoods = cells;
            currentState = isAlive ? CellState.GetLivingState() : CellState.GetDeadState();
            nextState = currentState;
        }
        public static LifeGameCell EmptyCell => EmptyLifeGameCell.Instance;



        private class EmptyLifeGameCell : LifeGameCell
        {
            public override bool IsAlive => false;
            public static readonly EmptyLifeGameCell Instance = new EmptyLifeGameCell();

            // 近傍は全てEmpty. Setは無効.
            public override LifeGameCell UpperLeft { get => this; set { } }
            public override LifeGameCell Upper { get => this; set { } }
            public override LifeGameCell UpperRight { get => this; set { } }

            public override LifeGameCell Left { get => this; set { } }
            public override LifeGameCell Right { get => this; set { } }

            public override LifeGameCell LowerLeft { get => this; set { } }
            public override LifeGameCell Lower { get => this; set { } }
            public override LifeGameCell LowerRight { get => this; set { } }

            private EmptyLifeGameCell() { }
        }
    }

    public static class LifeGameExtensions
    {
        public static void ConnectRight(this LifeGameCell leftCell, LifeGameCell rightCell)
        {
            leftCell.Right = rightCell;
            rightCell.Left = leftCell;
        }
        public static void ConnectLowerRight(this LifeGameCell upperLeftCell, LifeGameCell lowerRightCell)
        {
            upperLeftCell.LowerRight = lowerRightCell;
            lowerRightCell.UpperLeft = upperLeftCell;
        }
        public static void ConnectLower(this LifeGameCell upperCell, LifeGameCell lowerCell)
        {
            upperCell.Lower = lowerCell;
            lowerCell.Upper = upperCell;
        }
        public static void ConnectLowerLeft(this LifeGameCell upperRightCell, LifeGameCell lowerLeftCell)
        {
            upperRightCell.LowerLeft = lowerLeftCell;
            lowerLeftCell.UpperRight = upperRightCell;
        }

        public static void SetAroundCells(this LifeGameCell centerCell, LifeGameCell rightCell, LifeGameCell lowerRightCell, LifeGameCell lowerCell, LifeGameCell lowerLeftCell)
        {
            centerCell.ConnectRight(rightCell);
            centerCell.ConnectLowerRight(lowerRightCell);
            centerCell.ConnectLower(lowerCell);
            centerCell.ConnectLowerLeft(lowerLeftCell);
        }
    }
}
