using UnityEngine;
using System.Collections;

public class Cross : MonoBehaviour {
	public GameObject item;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (this.item != null) {
			Player.Instance.RemoveItem(item.GetComponent<Item>());
			Destroy (item);
			Destroy (this.gameObject);
		}
	}
}
