using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
    public FootDriver Foot1;
    public FootDriver Foot2;

    public float Speed = 5.0f;
    public float MinXDistance = 1.0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Foot1.Grounded && !Foot2.Grounded)
        {
            this.MoveTowardsFoot(Foot2, Foot1);
        }
        else if (!Foot1.Grounded && Foot2.Grounded)
        {
            this.MoveTowardsFoot(Foot1, Foot2);
        }
        else if(!Foot1.Grounded && !Foot2.Grounded)
        {
            Vector3 movement = new Vector3(0, -1, 0) * this.Speed * Time.deltaTime;
            this.transform.position += movement;
        }

        if((Foot1.Grounded && Foot1.Extending) ||
            (Foot2.Grounded && Foot2.Extending))
        {
            Vector3 movement = new Vector3(0, 1, 0) * this.Speed * Time.deltaTime;
            this.transform.position += movement;

            Foot1.transform.position -= movement;
            Foot2.transform.position -= movement;
        }
	}

    private void MoveTowardsFoot(FootDriver lifted, FootDriver grounded)
    {
        if(lifted.CurrentDistance > lifted.ExtendedDistance)
        {
            Vector3 difference = lifted.transform.position - this.transform.position;

            difference.y = 0;
            difference.z = 0;

            if(difference.x > this.MinXDistance)
            {
                Vector3 movement = difference.normalized * this.Speed * Time.deltaTime;

                this.transform.position += movement;

                Foot1.transform.position -= movement;
                Foot2.transform.position -= movement;
            }
        }
    }
}
