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

	public void Use(ItemWrapper wrapper)
    {
        if(PickedUp)
        {
            foreach (Burden burden in Player.Instance.Burdens)
            {
                foreach (Item item in burden.SolvedBy)
                {
                    if (this.id == item.id)
                    {
						InputManager.Instance.Wrappers.Remove(wrapper);
						wrapper.renderer.material.color = Color.green;

                        Player.Instance.RemoveBurden(burden);
						Player.Instance.RemoveItem(this);
						Player.Instance.Score++;

                        ItemUseEvent useEvent = this.GetComponent<ItemUseEvent>();
                        if (useEvent != null) useEvent.Fire();

                        GameObject.Destroy(this.gameObject, 0.5f);
						GameObject.Destroy(wrapper.gameObject, 0.5f);
                        return;
                    }
                }
            }
			if(Player.Instance.Score > 0) 
			{
				Player.Instance.Score--;
			}
			else
			{
				Player.Instance.GameOver();
			}
			wrapper.renderer.material.color = Color.red;
			this.StartCoroutine(wrapper.ResetColor());
        }
    }
}
