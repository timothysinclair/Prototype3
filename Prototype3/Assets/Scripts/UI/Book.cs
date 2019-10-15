using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public List<Page> pages;
    public int index = 0;

    public void Open()
    {
        Cursor.visible = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Refresh(pages[index]);
    }
    public void Next()
    {
        Deactivate(pages[index].gameObject);
        if (index < (pages.Count - 1)) index++;

        Refresh(pages[index]);
    }

    public void Previous()
    {
        Deactivate(pages[index].gameObject);
        if (index > 0) index--;
        Refresh(pages[index]);
    }

    public void Refresh(Page page)
    {
        Activate(page.gameObject);
        if (index >= (pages.Count - 1))
        {
            Deactivate(page.next.gameObject);
            Activate(page.previous.gameObject);
        }
        else if (index <= 0)
        {
            Deactivate(page.previous.gameObject);
            Activate(page.next.gameObject);
        }
        else
        {
            Activate(page.next.gameObject);
            Activate(page.previous.gameObject);
        }
    }

    public void Activate(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void Deactivate(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
