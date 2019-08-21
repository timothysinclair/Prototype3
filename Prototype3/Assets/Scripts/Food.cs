using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    None,
    FishPlatter,
    FruitPlatter,
    GreenBananas,
    PaniPopo,
    RawFish
}

public class Food : MonoBehaviour
{
    public FoodType type;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerInventory>();

        if (player)
        {
            player.CollectFood(type);
            Destroy(this.gameObject);
        }
    }
}
