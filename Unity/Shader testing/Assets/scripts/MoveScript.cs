using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour 
{
    public float Speed = 0.5f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.position += new Vector3(1, 0 , 0) *  this.Speed * Time.deltaTime;
	}
}
