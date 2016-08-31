using UnityEngine;

public class SprintContainer : MonoBehaviour {

    public float originalSpeed;
    public float currentSpeed;
    public float speedMultiplier;
    public float maxSprintTime;

    private float sprintTime;
    private bool sprinting = false;

	void Update () {
	    if (sprinting)
        {
            sprintTime += Time.deltaTime;
            sprintTime = Mathf.Clamp(sprintTime, 0.0f, maxSprintTime);
            sprinting = sprintTime < maxSprintTime;
        }
        else
        {
            sprintTime -= Time.deltaTime / 2.0f;
        }
        currentSpeed = sprinting ? originalSpeed * speedMultiplier : originalSpeed;
	}

    public void startSprinting()
    {
        if (sprintTime < maxSprintTime)
        {
            sprinting = true;
        }
    }

    public void stopSprinting()
    {
        sprinting = false;
    }
}
