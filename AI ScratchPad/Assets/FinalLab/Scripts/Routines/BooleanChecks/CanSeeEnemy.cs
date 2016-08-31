using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines.BooleanChecks
{
    public class CanSeeEnemy : Routine
    {
        private Transform theTransform;
        private Transform enemyTransform;

        public CanSeeEnemy(Transform theTransform, Transform enemyTransform)
        {
            this.theTransform = theTransform;
            this.enemyTransform = enemyTransform;
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            state = AStar.pathClear(theTransform.position, enemyTransform.position) ? RoutineState.Success : RoutineState.Failure;
        }
    }
}
