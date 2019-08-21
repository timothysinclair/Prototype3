using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public Button previous;
    public Button next;

    public void Activate(Button button)
    {
        button.gameObject.SetActive(true);
    }

    public void Deactivate(Button button)
    {
        button.gameObject.SetActive(false);
    }
}
