//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Hangi : MonoBehaviour
//{
//    public GameObject[] food;
//    public bool canActivate = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!canActivate) { return; }

//        var player = other.gameObject.GetComponent<PlayerController>();

//        //if (player)
//        //{
//        //    player.SetActionState(ActionState.eat);
//        //}
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (!canActivate) { return; }

//        var player = other.gameObject.GetComponent<PlayerController>();

//        if (player)
//        {
//            player.SetActionState(ActionState.jump);
//        }
//    }

//    public void ActivateFood(int numFood)
//    {
//        var player = FindObjectOfType<PlayerController>();
//        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
//        player.MakeHappy();

//        player.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);


//        for (int i = 0; i < numFood; i++)
//        {
//            if (i > food.Length - 1) { return; }

//            food[i].SetActive(true);
//        }
//    }

//}
