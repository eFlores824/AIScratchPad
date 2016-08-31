using Assets.FinalLab.Scripts.BehaviorTree;
using UnityEngine;
using System;

namespace Assets.FinalLab.Scripts.Routines.Movement
{
    public class TrackEnemy : Routine
    {
        private Transform theTransform;
        private SprintContainer sprinter;

        private Node[] toHome = null;
        private Node[] path = null;
        private int currentIndex = 0;

        public TrackEnemy(Transform theTransform, SprintContainer sprinter, Node enemyBase, Node teamBase)
        {
            this.theTransform = theTransform;
            this.sprinter = sprinter;

            toHome = AStar.getPath(teamBase, enemyBase);
            Array.Reverse(toHome);
        }

        private int FindCutoffNode()
        {
            float leastDistance = float.MaxValue;
            int closestIndex = 0;
            for (int i = 0; i < toHome.Length; ++i)
            {
                Node currentNode = toHome[i];
                float distance = (theTransform.position - currentNode.theTransform.position).magnitude;
                if (distance < leastDistance)
                {
                    leastDistance = distance;
                    closestIndex = i;
                }
            }
            return closestIndex;
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
                int closestIndex = FindCutoffNode();
                Node destination = toHome[closestIndex];
                Node[] thePath = AStar.getPath(destination, AStar.findNearestTargetNode(theTransform.position, destination.transform.position));

                int length = toHome.Length - closestIndex - 1;
                Node[] finalPath = new Node[thePath.Length + length];
                Array.Copy(thePath, finalPath, thePath.Length);
                Array.Copy(toHome, closestIndex + 1, finalPath, thePath.Length, length);

                path = finalPath;
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
