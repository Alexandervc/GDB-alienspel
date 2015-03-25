using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;

    public GameObject[] BurdenPrefabs;

    public List<Burden> Burdens;
    public int MaxBurdensGoal = 6;

    public List<Item> Items;
    public int MaxItems = 5;

    public Transform ItemsPos;
    public float ItemDistance = 0.1f;

    public float Speed = 1f;

	public int SolvedBurdens = 0;

	public float MaxSpawnDelay = 7.5f;
	public float MinSpawnDelay = 0.5f;
	public float DeltaSpawnDelay = 0.5f;

    public bool spawnAtRandom = false;

	private float spawnDelay = 1f;

	private int maxBurdens = 1;

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
        if (this.Burdens.Count < this.maxBurdens && !spawning && spawnAtRandom)
        {
            this.spawning = true;

            this.StartCoroutine(this.SpawnBurdenAfter(Random.Range(this.spawnDelay - this.DeltaSpawnDelay, this.spawnDelay)));
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

		if(this.SolvedBurdens == 0)
		{
			this.spawnDelay = this.MaxSpawnDelay;
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item")
        {
            if(this.Items.Count < this.MaxItems)
            {
                this.Items.Add(other.GetComponent<Item>());
                other.GetComponent<Item>().PickedUp = true;
            }
        }
        else if (other.tag == "SpawnBurden")
        {
            SpawnBurdenEvent sbe = other.GetComponent<SpawnBurdenEvent>();

            this.spawnBurden(sbe.burdenToSpawn);

            Destroy(other.gameObject);
        }
    }

    public void spawnBurden(Burden burdenToSpawn)
    {
        foreach (GameObject pref in this.BurdenPrefabs)
        {
            Burden burden = pref.GetComponent<Burden>();
            if (burden.Id == burdenToSpawn.Id)
            {
                GameObject obj = (GameObject)GameObject.Instantiate(pref);
                obj.transform.position = this.transform.position + new Vector3(0, 1, 0) * (1f * (this.Burdens.Count + 1) + 1f);

                this.Burdens.Add(obj.GetComponent<Burden>());

                this.Speed = this.Speed * obj.GetComponent<Burden>().SpeedModifier;
                break;
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

		this.SolvedBurdens++;

		if(this.maxBurdens < this.MaxBurdensGoal) 
		{
			this.maxBurdens++;
		}

		if(this.spawnDelay > this.MinSpawnDelay) 
		{
			this.spawnDelay -= this.DeltaSpawnDelay;
		}

        this.Speed /= burden.SpeedModifier;

        GameObject.Destroy(burden.gameObject);
    }

	public void RemoveItem(Item item)
	{
		this.Items.Remove (item);
	}

    public void SpawnItem(GameObject item)
    {
        Vector3 spawnPos = this.transform.FindChild("SpawnItemPos").transform.position;

        GameObject obj = (GameObject)GameObject.Instantiate(item);
        obj.transform.position = spawnPos;
    }
}
