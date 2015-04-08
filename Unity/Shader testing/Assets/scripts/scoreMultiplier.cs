using UnityEngine;
using System.Collections;

public class scoreMultiplier : MonoBehaviour 
{
    public int Multiplier = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponentInParent<Player>().Score *= this.Multiplier;
            GameObject.Destroy(this.gameObject);
        }
    }
}
