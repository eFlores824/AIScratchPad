using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class SeekEnemyFlag : State
    {
        private bool reached = false;
        private int currentIndex = 0;
         
        public bool Reached { get { return reached; } }

        private Node destination;

        private Node[] path;
        private Transform theTransform;
        private SprintContainer sprint;

        public SeekEnemyFlag(Transform theTransform, SprintContainer sprint, Node destination)
        {
            this.theTransform = theTransform;
            this.sprint = sprint;
            this.destination = destination;
        }

        public override void OnEnter()
        {
            currentIndex = 0;
            reached = false;
            path = AStar.getPath(destination, AStar.findNearestNode(theTransform.position));
            if (path == null)
            {
                Debug.Log("This is null");
            }
        }

        public override void OnExecute(float dt)
        {
            if (!reached)
            {
                Vector3 toDestination = path[currentIndex].theTransform.position - theTransform.position;
                toDestination.y = 0.0f;
                Vector3 movement = toDestination.normalized * sprint.currentSpeed * dt;
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
            }
        }
    }
}
