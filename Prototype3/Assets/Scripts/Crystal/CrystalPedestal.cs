using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalHolder))]
public class CrystalPedestal : MonoBehaviour
{

    private CrystalHolder myHolder;

    private void Awake()
    {
        myHolder = GetComponent<CrystalHolder>();
    }

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerHolder = other.GetComponent<CrystalHolder>();
            myHolder.Swap(playerHolder);
        }
    }
}
