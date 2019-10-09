using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            GameManager.Instance.GetCurrentLevel().GetCurrentPuzzle().RespawnPlayer();
        }
    }
}
