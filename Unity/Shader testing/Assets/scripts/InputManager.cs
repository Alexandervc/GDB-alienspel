using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	public static InputManager Instance = null;

	public GameObject bin;

	public string[] ItemButtons;

	public List<ItemWrapper> Wrappers;

	void Awake() {
		InputManager.Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}

	public ItemWrapper GetItemWrapper(string name) {
		foreach (ItemWrapper w in Wrappers) {
			if(w.Button.Equals(name)) {
				return w;
			}
		}
		return null;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftControl) || Input.GetKeyDown (KeyCode.RightControl)) {
			bin.renderer.material.color = Color.yellow;
		}
		if (Input.GetKeyUp (KeyCode.LeftControl) || Input.GetKeyUp (KeyCode.RightControl)) {
			bin.renderer.material.color = Color.white;
		}

		foreach (string itemButton in ItemButtons) {
			if(Input.GetKeyDown(itemButton)) {
				ItemWrapper wrapper = GetItemWrapper(itemButton);
				if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
					// Delete item
					wrapper.DeleteItem();
				} else {
					// Use item
					wrapper.UseItem();
				}
			}
		}
	}
}
