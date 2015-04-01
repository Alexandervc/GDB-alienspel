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
						wrapper.sRenderer.color = Color.green;

						this.StartCoroutine(this.DoUse(wrapper, burden));

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
			wrapper.sRenderer.color = Color.red;
			this.StartCoroutine(wrapper.ResetColor());
		}
    }

	private IEnumerator DoUse(ItemWrapper wrapper, Burden burden) {
		yield return new WaitForSeconds(0.5f);

		InputManager.Instance.Wrappers.Remove(wrapper);
		
		Player.Instance.RemoveBurden(burden);
		Player.Instance.RemoveItem(this);
		Player.Instance.Score++;
		
		ItemUseEvent useEvent = this.GetComponent<ItemUseEvent>();
		if (useEvent != null) useEvent.Fire();
		
		GameObject.Destroy(this.gameObject);
		GameObject.Destroy(wrapper.gameObject);
	}
}
