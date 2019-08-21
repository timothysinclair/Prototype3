using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempTextbox : MonoBehaviour
{
    public TextMeshProUGUI textObject;

    public void SetText(string newText)
    {
        textObject.text = newText;
    }

    public void CloseTextbox()
    {
        Destroy(this.gameObject);
    }
}
