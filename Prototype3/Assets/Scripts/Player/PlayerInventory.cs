using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool[] foodsCollected;
    private int numFoods = 5;

    private void Awake()
    {
        foodsCollected = new bool[numFoods];

        for (int i = 0; i < numFoods; i++)
        {
            foodsCollected[i] = false;
        }
    }

    public PlayerInventory()
    {
        
    }

    public void CollectFood(FoodType foodType)
    {
        foodsCollected[(int)foodType] = true;
    }

    public bool QueryFood(FoodType foodType)
    {
        return foodsCollected[(int)foodType];
    }
}
