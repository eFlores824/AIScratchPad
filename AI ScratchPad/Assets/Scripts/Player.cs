using UnityEngine;

public abstract class Player : MonoBehaviour {

    public GameObject enemy;
    public GameObject FlagContainer;
    public Node teamFlag;
    public Flag teamFlagObject;
    public Flag enemyFlagObject;
    public TextManager textManager;

    protected Transform theTransform;
    protected Transform enemyTransform;
    protected Vector3 initialEnemyFlagPosition;
    protected Vector3 initialTeamFlagPosiiton;

    protected void Begin()
    {
        enemyTransform = enemy.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
    }

    protected abstract void CollisionCallback();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == enemy && enemyFlagObject.captured)
        {
            enemyFlagObject.captured = false;
            enemyFlagObject.theTransform.position = initialEnemyFlagPosition;
            enemyFlagObject.theTransform.parent = FlagContainer.transform;

            theTransform.position = teamFlag.theTransform.position;
            CollisionCallback();
        }
    }
}
