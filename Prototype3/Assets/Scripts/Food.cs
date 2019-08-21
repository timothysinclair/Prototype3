using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    FishPlatter,
    FruitPlatter,
    GreenBananas,
    Hangi
}

public class Food : MonoBehaviour
{
    public FoodType type;

    private void OnTriggerEnter(Collider other)
    {
        if (type == FoodType.Hangi) { return; }

        var player = other.gameObject.GetComponent<PlayerInventory>();

        if (player)
        {
            player.CollectFood(type);
            GetComponent<Animator>().SetTrigger("Collected");
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void PlayCollectSound()
    {

    }
}
