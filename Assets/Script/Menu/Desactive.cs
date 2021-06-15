using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Desactive : MonoBehaviour
{
    [SerializeField] string axe;
    [SerializeField] GameObject selected;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw(axe)!=0)
        {
            EventSystem.current.SetSelectedGameObject(selected);
            gameObject.SetActive(false);
        }
    }
}
