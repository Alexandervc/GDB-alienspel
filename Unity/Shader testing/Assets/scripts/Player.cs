using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;

    public GameObject[] BurdenPrefabs;

    public List<Burden> Burdens;
    public int MaxBurdens = 6;

    public List<Item> Items;
    public int MaxItems = 5;

    public Transform ItemsPos;
    public float ItemDistance = 0.1f;

    public float Speed = 1f;

    private bool spawning = false;

    void Awake()
    {
        Player.Instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        this.Burdens = new List<Burden>();
        this.Items = new List<Item>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.Burdens.Count < this.MaxBurdens && !spawning)
        {
            this.spawning = true;

            this.StartCoroutine(this.SpawnBurdenAfter(Random.Range(0.5f, 3.5f)));
        }

        this.transform.position += new Vector3(1, 0, 0) * this.Speed * Time.deltaTime;
	}
    
    IEnumerator SpawnBurdenAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        this.SpawnRandomBurden();
    }

    private void SpawnRandomBurden()
    {
        int index;
        bool newIndex;
        do
        {
            newIndex = true;
            index = Random.Range(0, this.BurdenPrefabs.Length);

            foreach(Burden burden in this.Burdens)
            {
                if(burden.Id == this.BurdenPrefabs[index].GetComponent<Burden>().Id)
                {
                    newIndex = false;
                }
            }
        }
        while(!newIndex);

        GameObject obj = (GameObject)GameObject.Instantiate(this.BurdenPrefabs[index]);
        obj.transform.position = this.transform.position + new Vector3(0, 1, 0) * (1f * (this.Burdens.Count + 1) + 1f);

        this.Burdens.Add(obj.GetComponent<Burden>());

        this.Speed = this.Speed * obj.GetComponent<Burden>().SpeedModifier;

        this.spawning = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item")
        {
            if(this.Items.Count < this.MaxItems)
            {
                this.Items.Add(other.GetComponent<Item>());
                other.transform.position = ItemsPos.position + (new Vector3(1, 0, 0) * ItemDistance * (this.Items.Count - 1));
                other.GetComponent<Item>().PickedUp = true;
            }     
        }
    }

    public bool HasItem(Item item)
    {
        foreach(Item i in this.Items)
        {
            if(item == i)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveBurden(Burden burden)
    {
        this.Burdens.Remove(burden);

        this.Speed /= burden.SpeedModifier;

        GameObject.Destroy(burden.gameObject);
    }
}
