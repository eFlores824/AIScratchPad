using Assets.FinalLab.Scripts.BehaviorTree;

namespace Assets.FinalLab.Scripts.Routines.BooleanChecks
{
    public class FlagCaptured : Routine
    {
        public Flag flagChecking;

        public FlagCaptured(Flag flagChecking)
        {
            this.flagChecking = flagChecking;
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            state = flagChecking.captured ? RoutineState.Success : RoutineState.Failure;
        }
    }
}
