using System.Collections.Generic;

namespace Assets.FinalLab.Scripts.BehaviorTree
{
    public class Selector : Routine
    {
        List<Routine> routines;

        public Selector(List<Routine> routines)
        {
            this.routines = routines;
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            state = RoutineState.Running;
            int failureCount = 0;
            foreach (Routine routine in routines)
            {
                routine.Run(dt);
                if (routine.State == RoutineState.Failure)
                {
                    ++failureCount;
                }
                else if (routine.State == RoutineState.Success)
                {
                    state = RoutineState.Success;
                    break;
                }
            }
            if (failureCount == routines.Count)
            {
                state = RoutineState.Failure;
            }
        }
    }
}
