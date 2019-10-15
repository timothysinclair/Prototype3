using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Hangi : MonoBehaviour
{
    [SerializeField] private GameObject hangiActivator;

    public void ActivateHangi()
    {
        hangiActivator.SetActive(true);
        this.transform.DOMove(this.transform.position, 10.0f).OnComplete(ReturnToMenu);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("New Main Menu");
    }
}
