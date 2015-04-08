using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour 
{
    public float TravelTime = 5.0f;

    public Transform TravelTarget;

    public bool Activated = false;
    public Movement Move = null;

	
	// Update is called once per frame
	void Update () 
    {
	    if(this.Activated && Input.GetKeyDown(KeyCode.Space))
        {
            this.StartCoroutine(this.TravelToLocation(this.TravelTime, this.TravelTarget.position));
        }
	}


    IEnumerator TravelToLocation(float travelTime, Vector3 target)
    {
        float timeSpend = 0F;
        Vector3 startPos = this.transform.position;

        while(timeSpend < travelTime)
        {
            Vector3 newPos = Vector3.Lerp(startPos, target, timeSpend / travelTime);

            Move.MoveFeet(newPos - this.transform.position);

            this.transform.position = newPos;

            timeSpend += Time.deltaTime;

            yield return null;
        }

        Move.MoveFeet(target - this.transform.position);

        this.transform.position = target;

        this.TravelTarget.position = startPos;
    }
}
