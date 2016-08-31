namespace Assets.Scripts.StateMachine.States
{
    public abstract class State
    {

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public abstract void OnExecute(float dt);
    }

}