using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines.Movement
{
    public class ChaseEnemy : Routine
    {
        private Transform theTransform;
        private Transform enemyTransform;
        private SprintContainer sprinter;

        private Node[] path = null;
        private int currentIndex = 0;

        public ChaseEnemy(Transform theTransform, Transform enemyTransform, SprintContainer sprinter)
        {
            this.theTransform = theTransform;
            this.enemyTransform = enemyTransform;
            this.sprinter = sprinter; 
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
                Node destination = AStar.findNearestNode(enemyTransform.position);
                Node start = AStar.findNearestTargetNode(theTransform.position, enemyTransform.position);
                path = AStar.getPath(destination, start);
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
