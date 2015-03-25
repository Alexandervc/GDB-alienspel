using UnityEngine;
using System.Collections;

public class FoodUsedEvent : ItemUseEvent
{
    public override void Fire()
    {
        Player.Instance.spawnAtRandom = true;
    }
}
