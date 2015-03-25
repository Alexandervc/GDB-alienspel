using UnityEngine;
using System.Collections;

public class LuckUsedEvent : ItemUseEvent 
{
    public Burden burdenToSpawn;
    public Item itemToSpawn;

    public override void Fire()
    {
        Player.Instance.spawnBurden(this.burdenToSpawn);
        Player.Instance.SpawnItem(this.itemToSpawn.gameObject);
    }
}
