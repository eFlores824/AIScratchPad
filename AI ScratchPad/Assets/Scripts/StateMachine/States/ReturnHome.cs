using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    class ReturnHome : State
    {
        private bool reached = false;
        //private bool sawEnemy = false;
        private int currentIndex = 0;
        private float timePassed = 2.5f;

        public bool Reached { get { return reached; } }

        private Node destination;

        private Node[] path;
        private Transform theTransform;
        private Transform enemyTransform;
        private SprintContainer sprinter;

        public ReturnHome(Transform theTransform, Transform enemyTransform, SprintContainer sprinter, Node destination)
        {
            this.theTransform = theTransform;
            this.enemyTransform = enemyTransform;
            this.destination = destination;
            this.sprinter = sprinter;
        }

        public override void OnEnter()
        {
            currentIndex = 0;
            timePassed = 2.5f;
            reached = false;
            path = AStar.getPath(destination, AStar.findNearestNode(theTransform.position));
        }

        public override void OnExecute(float dt)
        {
            if (reached)
            {
                return;
            }
            timePassed += Time.deltaTime;
            bool seeEnemy = canSeeEnemy();
            if (seeEnemy && timePassed >= 2.5f)
            {
                timePassed = 0.0f;
                Node[] generatedPath = AStar.avoidPositionPath(destination, AStar.findFurthestTargetNearNode(theTransform.position, enemyTransform.position), enemyTransform.position);
                if (generatedPath != null)
                {
                    path = generatedPath;
                }
                currentIndex = 0;
                sprinter.startSprinting();
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
                reached = currentIndex == path.Length;
            }
            //sawEnemy = seeEnemy;
        }

        public override void OnExit()
        {
            sprinter.stopSprinting();
        }

        private bool canSeeEnemy()
        {
            Vector3 toEnemy = enemyTransform.position - theTransform.position;
            bool hitSomething = Physics.Raycast(theTransform.position, toEnemy, toEnemy.magnitude);
            return !hitSomething;
        }
    }
}
