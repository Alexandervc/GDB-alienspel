using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour
{
    public Elevator Elevator;
    public float WaitTime = 2.0f;

    private Movement move = null;

    private bool wasSpawningAtRandom = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print(other.tag);

            this.move = other.GetComponentInParent<Movement>();
            this.StartCoroutine(this.Timer(this.WaitTime));

            this.wasSpawningAtRandom = other.GetComponentInParent<Player>().spawnAtRandom;
            other.GetComponentInParent<Player>().spawnAtRandom = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            this.move = null;
            this.Elevator.Activated = false;

            other.GetComponentInParent<Player>().spawnAtRandom = this.wasSpawningAtRandom;
        }
    }

    IEnumerator Timer(float time)
    {
        float timeSpend = 0F;

        while (timeSpend < time)
        {
            if (this.move == null)
            {
                yield break;
            }

            timeSpend += Time.deltaTime;
            yield return null;
        }

        this.Elevator.Activated = true;
        this.Elevator.Move = this.move;
    }
}
