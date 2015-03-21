using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
    public int id;
    public bool PickedUp = false;

    void Update()
    {
        if(this.PickedUp)
        {
            this.transform.position = Player.Instance.ItemsPos.position + (new Vector3(1, 0, 0) * Player.Instance.ItemDistance * Player.Instance.Items.IndexOf(this));
        }
    }

	void OnMouseDown()
    {
        if(PickedUp)
        {
            foreach (Burden burden in Player.Instance.Burdens)
            {
                foreach (Item item in burden.SolvedBy)
                {
                    if (this.id == item.id)
                    {
                        Player.Instance.RemoveBurden(burden);
						Player.Instance.RemoveItem(this);
                        GameObject.Destroy(this.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
