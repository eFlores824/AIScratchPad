using System.Collections.Generic;

namespace Assets.FinalLab.Scripts.BehaviorTree
{
    public class Sequence: Routine
    {
        List<Routine> routines;

        public Sequence(List<Routine> routines)
        {
            this.routines = routines;
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            state = RoutineState.Success;
            foreach (Routine routine in routines)
            {
                routine.Run(dt);
                if (routine.State == RoutineState.Failure)
                {
                    state = RoutineState.Failure;
                    break;
                }
                else if (routine.State == RoutineState.Running)
                {
                    state = RoutineState.Running;
                }
            }
        }
    }
}
