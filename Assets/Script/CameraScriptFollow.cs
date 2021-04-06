using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptFollow : MonoBehaviour
{
    //Public variable
    [Tooltip("vitesse de deplacement de la cam ")]
    public float VitesseDeplacement;
    [Tooltip("vitesse de rotation de la cam ")]
    public float VitesseRotation;
    [Tooltip("l'objet a suivre en position")]
    public Transform MotoToFollow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, MotoToFollow.position, VitesseDeplacement);// bouge la camera vers l'objet à suivre
        transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);// set la camera rotation // pas vraiment utile
    }
}
