using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines.Movement
{
    public class SeekEnemyFlag : Routine
    {
        private Transform theTransform;
        private SprintContainer sprinter;
        private Node enemyFlag;

        private Node[] path = null;
        private int currentIndex = 0;

        public SeekEnemyFlag(Transform theTransform, SprintContainer sprinter, Node enemyFlag)
        {
            this.theTransform = theTransform;
            this.sprinter = sprinter;
            this.enemyFlag = enemyFlag;
        }

        private void ResetPathIfNeeded()
        {
            bool isNodeNear = false;
            if (path != null && currentIndex != path.Length)
            {
                Node[] nearbyNodes = AStar.nearbyNodes(theTransform.position);
                foreach (Node n in nearbyNodes)
                {
                    isNodeNear = n.Equals(path[currentIndex]);
                    if (isNodeNear)
                    {
                        break;
                    }
                }
            }
            if (path == null || currentIndex == path.Length || !isNodeNear)
            {
                Node starting = AStar.findNearestTargetNode(theTransform.position, enemyFlag.transform.position);
                path = AStar.getPath(enemyFlag, starting);
                currentIndex = 0;
            }
        }

        public override void Reset()
        {
            Start();
        }

        public override void Run(float dt)
        {
            ResetPathIfNeeded();

            Vector3 toDestination = path[currentIndex].theTransform.position - theTransform.position;
            toDestination.y = 0.0f;
            Vector3 movement = toDestination.normalized * sprinter.currentSpeed * dt;
            if (movement.magnitude > toDestination.magnitude)
            {
                movement = toDestination;
            }
            theTransform.position += movement;
            Vector3 firstPosition = theTransform.position;
            Vector3 secondPosition = path[currentIndex].theTransform.position;
            firstPosition.y = 0.0f;
            secondPosition.y = 0.0f;
            if (firstPosition == secondPosition)
            {
                ++currentIndex;
            }
            state = RoutineState.Success;
        }
    }
}
