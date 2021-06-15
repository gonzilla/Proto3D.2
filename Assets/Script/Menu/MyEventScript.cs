using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyEventScript : MonoBehaviour
{
    private bool selected;


    public void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        selected = true;
       
    }

    public void Deselect()
    {
        if (!selected) return;

        EventSystem.current.SetSelectedGameObject(null);
    }
}
