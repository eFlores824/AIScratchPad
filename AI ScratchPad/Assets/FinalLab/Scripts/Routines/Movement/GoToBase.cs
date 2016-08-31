using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines.Movement
{
    class GoToBase : Routine
    {
        private Transform theTransform;
        private Transform enemyTransform;
        private SprintContainer sprinter;
        private Node teamFlag;

        private int currentIndex = 0;
        private Node[] path = null;
        private float timePassedSinceSeen = 1.3f;

        public GoToBase(Transform theTransform, Transform enemyTransform, SprintContainer sprinter, Node teamFlag)
        {
            this.theTransform = theTransform;
            this.sprinter = sprinter;
            this.teamFlag = teamFlag;
            this.enemyTransform = enemyTransform;
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
                Node starting = AStar.findNearestTargetNode(theTransform.position, teamFlag.transform.position);
                path = AStar.getPath(teamFlag, starting);
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

            timePassedSinceSeen += dt;
            timePassedSinceSeen = Mathf.Clamp(timePassedSinceSeen, 0.0f, 1.3f);
            if (AStar.pathClear(theTransform.position, enemyTransform.position) && timePassedSinceSeen == 1.3f)
            {
                timePassedSinceSeen = 0.0f;
                Node[] generatedPath = AStar.avoidPositionPath(teamFlag, AStar.findFurthestTargetNearNode(theTransform.position, enemyTransform.position), enemyTransform.position);
                if (generatedPath != null)
                {
                    path = generatedPath;
                }
                currentIndex = 0;
            }

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
