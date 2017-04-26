namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PlayerIssueOrderEventArgs : EventArgs
    {
        private bool m_attackMove;
        private GameObjectOrder m_order;
        private bool m_process;
        private GameObject m_target;
        private Vector3 m_targetPosition;

        public PlayerIssueOrderEventArgs(GameObjectOrder order, Vector3 targetPosition, GameObject target, [MarshalAs(UnmanagedType.U1)] bool attackMove)
        {
            this.m_order = order;
            this.m_targetPosition = targetPosition;
            this.m_target = target;
            this.m_attackMove = attackMove;
            this.m_process = true;
        }

        public bool IsAttackMove =>
            this.m_attackMove;

        public GameObjectOrder Order
        {
            get => 
                this.m_order;
            set
            {
                this.m_order = value;
            }
        }

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }

        public GameObject Target =>
            this.m_target;

        public Vector3 TargetPosition
        {
            get => 
                this.m_targetPosition;
            set
            {
                this.m_targetPosition = value;
            }
        }

        public delegate void PlayerIssueOrderEvent(Obj_AI_Base sender, PlayerIssueOrderEventArgs args);
    }
}

