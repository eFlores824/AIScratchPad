using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines.BooleanChecks
{
    public class CloserToHome : Routine
    {
        private Transform theTransform;
        private Transform enemyTransform;
        private Node teamFlag;

        public CloserToHome(Transform theTransform, Transform enemyTransform, Node teamFlag)
        {
            this.theTransform = theTransform;
            this.enemyTransform = enemyTransform;
            this.teamFlag = teamFlag;
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            float distanceHome = (theTransform.position - teamFlag.theTransform.position).magnitude;
            float distanceEnemy = (theTransform.position - enemyTransform.position).magnitude;
            state = distanceHome <= distanceEnemy ? RoutineState.Success : RoutineState.Failure;
        }
    }
}
