using UnityEngine;
using Assets.Scripts.StateMachine.States;

public class MachineManager : Player {

    #region Variables

    private bool wasFlagCaptured = false;
    private StateMachine machine;

    #endregion

    #region Objects

    public Node enemyFlag;
    #endregion

    #region Components

    #endregion

    #region States

    private ReturnHome bringFlagHome;
    private SeekEnemyFlag seekEnemyFlag;
    private Cutoff captureEnemy;
    private Chase chaseEnemy;
    
    #endregion

    void Start () {
        Begin();
        AStar.SetAllNodes();
        SprintContainer sprint = GetComponent<SprintContainer>();

        bringFlagHome = new ReturnHome(theTransform, enemy.GetComponent<Transform>(), sprint, teamFlag);
        seekEnemyFlag = new SeekEnemyFlag(theTransform, sprint, enemyFlag);
        captureEnemy = new Cutoff(theTransform, sprint, enemyFlag, teamFlag);
        chaseEnemy = new Chase(theTransform, enemyTransform, sprint);

        machine = new StateMachine(seekEnemyFlag);

        initialEnemyFlagPosition = enemyFlagObject.transform.position;
        initialTeamFlagPosiiton = teamFlagObject.transform.position;
    }
	
	void Update () {
        manageStateMachine();
        wasFlagCaptured = teamFlagObject.captured;
    }

    protected override void CollisionCallback()
    {
        machine.SwitchStates(seekEnemyFlag);
    }

    private void manageStateMachine() {
        machine.Execute(Time.deltaTime);
        #region teamFlagCaptured

        if (teamFlagObject.captured && !wasFlagCaptured)
        {
            bool haveFlag = enemyFlagObject.captured;
            if (haveFlag)
            {
                float toTeamFlag = (theTransform.position - teamFlagObject.theTransform.position).magnitude;
                float toEnemyFlag = (theTransform.position - enemyFlagObject.theTransform.position).magnitude;
                bool closerToTeamFlag = toTeamFlag < toEnemyFlag;
                if (closerToTeamFlag)
                {
                    machine.SwitchStates(bringFlagHome);
                }
                else
                {
                    machine.SwitchStates(captureEnemy);
                }
            }
            else
            {
                machine.SwitchStates(captureEnemy);
            }
        }

        #endregion
        #region seekEnemyFlag

        if (machine.currentState == seekEnemyFlag)
        {
            if (seekEnemyFlag.Reached)
            {
                enemyFlagObject.captured = true;
                enemyFlagObject.theTransform.parent = theTransform;
                machine.SwitchStates(bringFlagHome);
            }
        }

        #endregion
        #region bringFlagHome

        else if (machine.currentState == bringFlagHome)
        {
            if (enemyFlagObject.captured == false)
            {
                machine.SwitchStates(seekEnemyFlag);
            }
            else if (bringFlagHome.Reached)
            {
                enemyFlagObject.captured = false;
                enemyFlagObject.theTransform.position = initialEnemyFlagPosition;
                enemyFlagObject.theTransform.parent = FlagContainer.transform;

                teamFlagObject.captured = false;
                teamFlagObject.theTransform.position = initialTeamFlagPosiiton;
                teamFlagObject.theTransform.parent = FlagContainer.transform;

                if (gameObject.name.Contains("Blue"))
                {
                    textManager.addBlueScore(1);
                }
                else
                {
                    textManager.addRedScore(1);
                }
                machine.SwitchStates(seekEnemyFlag);
            }
        }

        #endregion
        #region captureEnemy

        else if (machine.currentState == captureEnemy)
        {
            bool haveFlag = enemyFlagObject.captured;
            if (!teamFlagObject.captured)
            {
                if (haveFlag)
                {
                    machine.SwitchStates(bringFlagHome);
                }
                else
                {
                    machine.SwitchStates(seekEnemyFlag);
                }
            }
            else if (captureEnemy.Reached)
            {
                machine.SwitchStates(seekEnemyFlag);
            }
            else if (canSeeEnemy())
            {
                machine.SwitchStates(chaseEnemy);
            }
        }

        #endregion
        #region chaseEnemy

        else if (machine.currentState == chaseEnemy)
        {
            bool haveFlag = enemyFlagObject.captured;
            if (!teamFlagObject.captured)
            {
                if (haveFlag)
                {
                    machine.SwitchStates(bringFlagHome);
                }
                else
                {
                    machine.SwitchStates(seekEnemyFlag);
                }
            }
            else if (chaseEnemy.Reached)
            {
                if (haveFlag)
                {
                    machine.SwitchStates(bringFlagHome);
                }
                else
                {
                    machine.SwitchStates(captureEnemy);
                }
            }
        }

        #endregion
    }

    private bool canSeeEnemy()
    {
        Vector3 toEnemy = enemyTransform.position - theTransform.position;
        bool hitSomething = Physics.Raycast(theTransform.position, toEnemy, toEnemy.magnitude);
        return !hitSomething;
    }
}
