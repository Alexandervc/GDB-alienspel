using UnityEngine;
using System.Collections;

public class ItemWrapper : MonoBehaviour {
	public string Button;
	public GameObject Item;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DeleteItem() {
		if (this.Item != null) {
			InputManager.Instance.Wrappers.Remove(this);
			this.renderer.material.color = Color.yellow;
			Player.Instance.RemoveItem(Item.GetComponent<Item>());
			Destroy (Item, 0.5f);
			Destroy (this.gameObject, 0.5f);
		}
	}

	public void UseItem() {
		if (this.Item != null) {
			this.Item.GetComponent<Item>().Use(this);
		}
	}

	public IEnumerator ResetColor() {
		yield return new WaitForSeconds (1f);
		this.renderer.material.color = Color.white;
	}
}
