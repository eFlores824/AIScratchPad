using Assets.FinalLab.Scripts.BehaviorTree;
using Assets.FinalLab.Scripts.Routines.BooleanChecks;
using Assets.FinalLab.Scripts.Routines.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FinalLab.Scripts.Routines
{
    public class RoutineManager : MonoBehaviour
    {
        public TextManager pointSystem;

        public Flag teamFlag;
        public Flag enemyFlag;

        public GameObject enemy;
        public GameObject flagContainer;

        public Node teamFlagNode;
        public Node enemyFlagNode;

        private Transform theTransform;

        private Routine tree;

        void Start()
        {
            AStar.SetAllNodes();

            theTransform = GetComponent<Transform>();
            Transform enemyTransform = enemy.GetComponent<Transform>();
            SprintContainer sprinter = GetComponent<SprintContainer>();

            //Boolean Checks
            CanSeeEnemy seeEnemy = new CanSeeEnemy(theTransform, enemyTransform);
            CloserToHome closerHome = new CloserToHome(theTransform, enemyTransform, teamFlagNode);
            FlagCaptured teamFlagCaptured = new FlagCaptured(teamFlag);
            FlagCaptured enemyFlagCaptured = new FlagCaptured(enemyFlag);

            //Movement Leaves
            GoToBase returnHome = new GoToBase(theTransform, enemyTransform, sprinter, teamFlagNode);
            SeekEnemyFlag getFlag = new SeekEnemyFlag(theTransform, sprinter, enemyFlagNode);
            ChaseEnemy followEnemy = new ChaseEnemy(theTransform, enemyTransform, sprinter);
            TrackEnemy findEnemy = new TrackEnemy(theTransform, sprinter, teamFlagNode, enemyFlagNode);

            //Do this if team flag not captured
            Sequence regularReturnHome = new Sequence(new List<Routine> { enemyFlagCaptured, returnHome });
            Selector flagNotCapturedSelector = new Selector(new List<Routine> { regularReturnHome, getFlag });

            //Do this if team flag captured
            Sequence closerHomeSequence = new Sequence(new List<Routine>() { closerHome, returnHome });
            Sequence seeEnemySequence = new Sequence(new List<Routine>() { seeEnemy, followEnemy });
            Selector seeEnemySelector = new Selector(new List<Routine>() { seeEnemySequence, findEnemy });
            Selector haveFlagSelector = new Selector(new List<Routine>() { closerHomeSequence, seeEnemySelector });
            Sequence haveFlagSequence = new Sequence(new List<Routine>() { enemyFlagCaptured, haveFlagSelector });
            Selector flagCapturedSelector = new Selector(new List<Routine>() { haveFlagSequence, seeEnemySelector });
            Sequence flagCapturedSequence = new Sequence(new List<Routine>() { teamFlagCaptured, flagCapturedSelector });
            tree = new Selector(new List<Routine>() { flagCapturedSequence, flagNotCapturedSelector });
        }

        void Update()
        {
            tree.Run(Time.deltaTime);
            captureFlagIfNear();
            scorePointIfNear();
        }

        private void captureFlagIfNear()
        {
            if (!enemyFlag.captured)
            {
                Vector3 toEnemyFlag = theTransform.position - enemyFlagNode.theTransform.position;
                toEnemyFlag.y = 0;
                if (toEnemyFlag.magnitude <= 0.5f)
                {
                    enemyFlag.theTransform.parent = theTransform;
                    enemyFlag.captured = true;
                }
            }
        }

        private void scorePointIfNear()
        {
            if (enemyFlag.captured)
            {
                Vector3 toTeamBase = theTransform.position - teamFlagNode.theTransform.position;
                toTeamBase.y = 0;
                if (toTeamBase.magnitude <= 0.5f)
                {
                    enemyFlag.captured = false;
                    Vector3 initialEnemyPosition = enemyFlagNode.theTransform.position;
                    initialEnemyPosition.y = enemyFlag.theTransform.position.y;
                    enemyFlag.theTransform.position = initialEnemyPosition;
                    enemyFlag.theTransform.parent = flagContainer.transform;

                    teamFlag.captured = false;
                    Vector3 initialTeamPosition = teamFlagNode.theTransform.position;
                    initialTeamPosition.y = teamFlagNode.theTransform.position.y;
                    teamFlag.theTransform.parent = flagContainer.transform;
                    teamFlag.theTransform.position = initialTeamPosition;

                    if (gameObject.name.Contains("Blue"))
                    {
                        pointSystem.addBlueScore(1);
                    }
                    else
                    {
                        pointSystem.addRedScore(1);
                    }
                }
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == enemy && enemyFlag.captured)
            {
                enemyFlag.captured = false;
                Vector3 initialEnemyPosition = enemyFlagNode.theTransform.position;
                initialEnemyPosition.y = enemyFlag.theTransform.position.y;
                enemyFlag.theTransform.position = initialEnemyPosition;
                enemyFlag.theTransform.parent = flagContainer.transform;

                theTransform.position = teamFlagNode.theTransform.position;
            }
        }
    }
}
