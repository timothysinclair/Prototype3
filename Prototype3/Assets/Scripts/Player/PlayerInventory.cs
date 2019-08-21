using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool[] foodsCollected;
    private int numFoods = 3;

    private float heightOffset = 1.0f;

    public GameObject[] foodPrefabs;

    public List<GameObject> foodsHolding;
    public Transform foodHoldingTransform;

    private Vector3 oldPosition;
    public AudioClip foodPickup;
    private AudioSource audioSource;


    private void Awake()
    {
        foodsCollected = new bool[numFoods];

        for (int i = 0; i < numFoods; i++)
        {
            foodsCollected[i] = false;
        }

        foodsHolding = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();

        oldPosition = this.transform.position;
    }

    public PlayerInventory()
    {
        
    }

    public void CollectFood(FoodType foodType)
    {
        if (!foodsCollected[(int)foodType])
        {
            foodsCollected[(int)foodType] = true;
            audioSource.PlayOneShot(foodPickup);
        }
        

        // foodsHolding.Add(Instantiate(foodPrefabs[(int)foodType]));
        // foodsHolding[foodsHolding.Count - 1].transform.localPosition = Vector3.zero;

        // foodsHolding[foodsHolding.Count - 1].GetComponent<SinFloat>().amp = 0.0f;

    }

    public bool QueryFood(FoodType foodType)
    {
        return foodsCollected[(int)foodType];
    }


    private void LateUpdate()
    {
        //for (int i = 0; i < foodsHolding.Count; i++)
        //{
        //    // foodsHolding[i].transform.position = oldPosition + new Vector3(0.0f, heightOffset * i, 0.0f);
        //}

        //oldPosition = this.transform.localPosition;
    }

}
