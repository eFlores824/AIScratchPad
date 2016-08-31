using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    class Cutoff : State
    {
        private int currentIndex = 0;

        public bool Reached { get; set; }

        private Node teamFlag;

        private Node[] path;
        private Node[] movementPath;

        private Transform theTransform;
        private SprintContainer sprint;

        public Cutoff(Transform theTransform, SprintContainer sprint, Node enemyFlag, Node teamFlag)
        {
            this.theTransform = theTransform;
            this.teamFlag = teamFlag;
            this.sprint = sprint;
            path = AStar.getPath(enemyFlag, teamFlag);
        }

        public override void OnEnter()
        {
            int closestIndex = FindCutoffNode();
            Node[] thePath = AStar.getPath(path[closestIndex], AStar.findNearestNode(theTransform.position));
            if (thePath != null)
            {
                int reverseLength = closestIndex - 2;
                if (reverseLength <= 0) {
                    reverseLength = closestIndex + 1;
                }
                Node[] reversedPath = new Node[reverseLength];
                Array.Copy(path, 2, reversedPath, 0, reversedPath.Length);
                Array.Reverse(reversedPath);
                movementPath = new Node[reversedPath.Length + thePath.Length];
                Array.Copy(thePath, movementPath, thePath.Length);
                Array.Copy(reversedPath, 0, movementPath, thePath.Length, reversedPath.Length);
            }
            else
            {
                movementPath = AStar.getPath(teamFlag, AStar.findNearestNode(theTransform.position));
            }
            currentIndex = 0;
            Reached = false;
            sprint.startSprinting();
        }

        private int FindCutoffNode()
        {
            float leastDistance = float.MaxValue;
            int closestIndex = 0;
            for (int i = 0; i < path.Length; ++i)
            {
                Node currentNode = path[i];
                float distance = (theTransform.position - currentNode.theTransform.position).magnitude;
                if (distance < leastDistance)
                {
                    leastDistance = distance;
                    closestIndex = i;
                } 
            }
            return closestIndex;
        }

        public override void OnExecute(float dt)
        {
            Vector3 toDestination = movementPath[currentIndex].theTransform.position - theTransform.position;
            toDestination.y = 0.0f;
            Vector3 movement = toDestination.normalized * sprint.currentSpeed * dt;
            if (movement.magnitude > toDestination.magnitude)
            {
                movement = toDestination;
            }
            theTransform.position += movement;
            Vector3 firstPosition = theTransform.position;
            Vector3 secondPosition = movementPath[currentIndex].theTransform.position;
            firstPosition.y = 0.0f;
            secondPosition.y = 0.0f;
            if (firstPosition == secondPosition)
            {
                ++currentIndex;
                Reached = currentIndex == movementPath.Length;
                if (Reached)
                {
                    currentIndex = movementPath.Length - 1;
                }
            }
        }
    }
}
