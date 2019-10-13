using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangi : MonoBehaviour
{
    [SerializeField] private GameObject hangiActivator;

    public void ActivateHangi()
    {
        hangiActivator.SetActive(true);
    }

}
