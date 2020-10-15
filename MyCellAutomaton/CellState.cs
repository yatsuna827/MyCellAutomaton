namespace MyCellAutomaton.ConwayLifeGame
{
    /// <summary>
    /// セルの状態(生死)を表現する抽象クラス.
    /// </summary>
    abstract class CellState
    {
        public abstract bool IsAlive { get; }
        /// <summary>
        /// 次の状態を取得します.
        /// </summary>
        /// <param name="livingNeighborhoods"></param>
        /// <returns></returns>
        public abstract CellState GetNextState(int livingNeighborhoods);

        public static CellState GetLivingState() => LivingState.GetInstance();
        public static CellState GetDeadState() => DeadState.GetInstance();
        public static CellState GetEmptyState() => EmptyState.GetInstance();

        /// <summary>
        /// セルが生存している状態を表すCellStateの実装. 
        /// </summary>
        private class LivingState : CellState
        {
            public override bool IsAlive => true;
            public override CellState GetNextState(int neighborhoods)
            {
                if (neighborhoods == 2 || neighborhoods == 3)
                    return this;
                else
                    return DeadState.GetInstance();
            }
            private static readonly LivingState instance = new LivingState();
            public static LivingState GetInstance() => instance;
            private LivingState() { }
        }
        /// <summary>
        /// セルが死滅している状態を表すCellStateの実装.
        /// </summary>
        private class DeadState : CellState
        {
            public override bool IsAlive => false;
            public override CellState GetNextState(int neighborhoods)
            {
                if (neighborhoods == 3)
                    return LivingState.GetInstance();
                else
                    return this;
            }
            private static readonly DeadState instance = new DeadState();
            public static DeadState GetInstance() => instance;
            private DeadState() { }
        }
        /// <summary>
        /// セルが存在していない状態を表すCellStateの実装.
        /// </summary>
        private class EmptyState : CellState
        {
            public override bool IsAlive => false;
            public override CellState GetNextState(int _) => this;
            private static readonly EmptyState instance = new EmptyState();
            private EmptyState() { }
            public static EmptyState GetInstance() => instance;
        }
    }
}
