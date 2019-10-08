using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalHolder))]
public class CrystalPedestal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var thisHolder = GetComponent<CrystalHolder>();
            var playerHolder = other.GetComponent<CrystalHolder>();
            thisHolder.Swap(playerHolder);
        }
    }
}
