using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;

    public GameObject[] BurdenPrefabs;
    public GameObject[] ItemPrefabs;

    public List<Burden> Burdens;
    public int MaxBurdensGoal = 6;

    public List<Item> Items;
    public int MaxItems = 5;

    public Transform ItemsPos;
    public float ItemDistance = 0.1f;

    public Transform SpawnItemPos;

    public float Speed = 1f;

	public int SolvedBurdens = 0;

	public float MaxSpawnDelay = 7.5f;
	public float MinSpawnDelay = 0.5f;
	public float DeltaSpawnDelay = 0.5f;

    public bool spawnAtRandom = true;

	public GameObject ItemWrapperPrefab;

	public Text WinLoseText;
	public Text ScoreText;
	public int Score = 0;

	private float spawnDelay = 1f;

	private int maxBurdens = 1;

    private bool spawningBurden = false;
    private bool spawningItem = false;

    void Awake()
    {
        Player.Instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        this.spawnDelay = this.MaxSpawnDelay;

        this.Burdens = new List<Burden>();
        this.Items = new List<Item>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.Burdens.Count < this.maxBurdens && (!spawningBurden) && spawnAtRandom)
        {
            this.spawningBurden = true;

            this.StartCoroutine(this.SpawnBurdenAfter(Random.Range(this.spawnDelay - this.DeltaSpawnDelay, this.spawnDelay)));
        }

        if (!this.spawningItem && spawnAtRandom)
        {
            this.spawningItem = true;

            this.StartCoroutine(this.SpawnItemAfter(Random.Range(this.spawnDelay - this.DeltaSpawnDelay, this.spawnDelay)));
        }

        this.transform.position += new Vector3(1, 0, 0) * this.Speed * Time.deltaTime;

		this.ScoreText.text = "Score: " + Score;
	}

    IEnumerator SpawnItemAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        this.SpawnRandomItem();
    }

    private void SpawnRandomItem()
    {
        int index = Random.Range(0, this.ItemPrefabs.Length);

        GameObject obj = (GameObject)GameObject.Instantiate(this.ItemPrefabs[index]);
        obj.transform.position = SpawnItemPos.position;

        this.spawningItem = false;
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

        this.spawningBurden = false;

		if(!this.spawnAtRandom)
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

				GameObject wrapper = (GameObject) Instantiate(this.ItemWrapperPrefab);
				wrapper.transform.position = other.transform.position;
				wrapper.transform.parent = other.transform;
				wrapper.GetComponent<ItemWrapper>().Item = other.gameObject;

				// Generate random button
				int random = -1;
				do {
					random = Random.Range(0, InputManager.Instance.ItemButtons.Length);
				} while(InputManager.Instance.GetItemWrapper(InputManager.Instance.ItemButtons[random]) != null);
				wrapper.GetComponent<ItemWrapper>().Button = InputManager.Instance.ItemButtons[random];

				InputManager.Instance.Wrappers.Add(wrapper.GetComponent<ItemWrapper>());
            }
        }
        else if (other.tag == "SpawnBurden")
        {
            SpawnBurdenEvent sbe = other.GetComponent<SpawnBurdenEvent>();

            this.spawnBurden(sbe.burdenToSpawn);

            Destroy(other.gameObject);
        }
		else if (other.tag == "Finish")
		{
			this.WinLoseText.text = "Finish";
			this.StartCoroutine(this.resetLevel());
		}
    }

	public void GameOver()
	{
		this.WinLoseText.text = "Game Over";
		this.StartCoroutine (this.resetLevel());
	}

	private IEnumerator resetLevel()
	{
		yield return new WaitForSeconds (2);
		Application.LoadLevel(0);
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
