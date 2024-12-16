using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellarTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject thePlayer;

    void OnTriggerEnter(Collider other) //Set trigger for teleportation
    {
        thePlayer.transform.position = teleportTarget.transform.position; //teleport to set game object
    }
}
