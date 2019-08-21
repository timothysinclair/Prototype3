using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notebook : MonoBehaviour
{
    public Sprite[] notebookImages;
    public Image targetImage;

    private int notebookPosition = 0;

    public void NextPage()
    {
        SetNotebookPosition((notebookPosition + 1) % 3);
    }

    private void SetNotebookPosition(int newPosition)
    {
        targetImage.sprite = notebookImages[newPosition];
        notebookPosition = newPosition;
    }
}
