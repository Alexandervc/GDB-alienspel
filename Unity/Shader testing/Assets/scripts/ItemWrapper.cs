using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemWrapper : MonoBehaviour {
	public string Button;
	public GameObject Item;

	public SpriteRenderer sRenderer;
	public Text ButtonText;
	public Canvas Canvas;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DeleteItem() {
		if (this.Item != null) {
			sRenderer.color = Color.yellow;
			this.StartCoroutine(this.DoDelete());
		}
	}

	private IEnumerator DoDelete() {
		yield return new WaitForSeconds (0.5f);
		InputManager.Instance.Wrappers.Remove(this);
		Player.Instance.RemoveItem(Item.GetComponent<Item>());
		Destroy (Item);
		Destroy (this.gameObject);
	}

	public void UseItem() {
		if (this.Item != null) {
			this.Item.GetComponent<Item>().Use(this);
		}
	}

	public IEnumerator ResetColor() {
		yield return new WaitForSeconds (1f);
		sRenderer.color = Color.white;
	}
}
