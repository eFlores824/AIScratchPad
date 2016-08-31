namespace Assets.FinalLab.Scripts.BehaviorTree
{
    public abstract class Routine
    {
        public enum RoutineState
        {
            Success,
            Failure,
            Running
        }

        protected RoutineState state;

        public RoutineState State
        {
            get { return state; }
        }

        public void Start()
        {
            state = RoutineState.Running;
        }

        public abstract void Reset();

        public abstract void Run(float dt);
    }
}
