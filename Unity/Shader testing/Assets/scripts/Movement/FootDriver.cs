using UnityEngine;
using System.Collections;

public class FootDriver : MonoBehaviour 
{
    public bool WASD = true;

    public float Speed = 0.5f;

    public float MaxDistance;
    public float ExtendedDistance;

    public Transform AttachmentPoint;

    public bool Grounded = false;

    public bool Extending = false;

    public float CurrentDistance
    {
        get
        {
            return (this.transform.position - AttachmentPoint.position).magnitude;
        }
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = Vector3.zero;

        if(WASD)
        {
            if (Input.GetKey(KeyCode.W)) direction += new Vector3(0, 1, 0);
            if (Input.GetKey(KeyCode.A)) direction += new Vector3(-1, 0, 0);
            if (Input.GetKey(KeyCode.S)) direction += new Vector3(0, -1, 0);
            if (Input.GetKey(KeyCode.D)) direction += new Vector3(1, 0, 0);
        }
        else
        {
            if (Input.GetKey(KeyCode.I)) direction += new Vector3(0, 1, 0);
            if (Input.GetKey(KeyCode.J)) direction += new Vector3(-1, 0, 0);
            if (Input.GetKey(KeyCode.K)) direction += new Vector3(0, -1, 0);
            if (Input.GetKey(KeyCode.L)) direction += new Vector3(1, 0, 0);
        }

        direction *= this.Speed * Time.deltaTime;

        Vector3 distance = this.transform.position - AttachmentPoint.position;

        this.Extending = false;
        if (distance.magnitude < this.ExtendedDistance)
        {
            Vector3 projDir = Vector3.Project(direction, distance);

            if (projDir.normalized == distance.normalized)
            {
                this.Extending = true;
            }
        }

        if (this.Grounded && direction.y < 0) direction.y = 0;

        this.transform.position += direction;

        if(distance.magnitude > this.MaxDistance)
        {
            this.transform.position = AttachmentPoint.position + distance.normalized * this.ExtendedDistance;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            this.Grounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            this.Grounded = false;
        }
    }
}
