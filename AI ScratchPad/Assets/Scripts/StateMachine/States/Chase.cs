using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    class Chase : State
    {
        public bool Reached { get; set; }

        private bool sawEnemy;

        private Transform theEnemy;
        private Transform theTransform;
        private SprintContainer sprinter;

        Node[] path;
        private int currentIndex;

        public Chase(Transform theTransform, Transform theEnemy, SprintContainer sprinter)
        {
            this.theTransform = theTransform;
            this.theEnemy = theEnemy;
            this.sprinter = sprinter;
        }

        public override void OnEnter()
        {
            currentIndex = 0;
            path = AStar.getPath(AStar.findNearestNode(theEnemy.position), AStar.findNearestNode(theTransform.position));
            sawEnemy = false;
        }

        public override void OnExecute(float dt)
        {
            bool seeEnemy = canSeeEnemy();
            if (!sawEnemy && seeEnemy)
            {
                Node[] thePath = AStar.getPath(AStar.findNearestNode(theEnemy.position), AStar.findNearestTargetNode(theTransform.position, theEnemy.position));
                currentIndex = 0;
                path = thePath;
                Reached = false;
                sprinter.startSprinting();
            }
            if (Reached) { return; }

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
                Reached = currentIndex == path.Length;
            }
            sawEnemy = seeEnemy;
        }
        public override void OnExit()
        {
            sprinter.stopSprinting();
        }

        private bool canSeeEnemy()
        {
            Vector3 toEnemy = theEnemy.position - theTransform.position;
            bool hitSomething = Physics.Raycast(theTransform.position, toEnemy, toEnemy.magnitude);
            return !hitSomething;
        }
    }
}
