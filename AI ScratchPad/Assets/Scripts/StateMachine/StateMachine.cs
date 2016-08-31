using Assets.Scripts.StateMachine.States;

public class StateMachine {

    public State currentState;

    public StateMachine(State initialState)
    {
        currentState = initialState;
        currentState.OnEnter();
    }
	
	public void Execute (float dt) {
        currentState.OnExecute(dt);
	}

    public void SwitchStates(State newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}