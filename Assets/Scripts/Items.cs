using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Items : MonoBehaviour
{
    public bool interactable;
    public bool pickupable;
    public int amount;
    public Transform textObject;
    public TextMeshPro promptDisplay;
    public Plane texturePlane;

    private GameObject closestPlayer;


    private void Update()
    {
        float minDist = -1f;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (minDist == -1f || dist < minDist)
            {
                minDist = dist;
                closestPlayer = player;
            }
        }
        if (minDist < 3f)
        {
            //textObject.gameObject.SetActive(true);
        }else
        {
            //textObject.gameObject.SetActive(false);
        }

        //textObject.LookAt(closestPlayer.transform);
        //textObject.Rotate(0, 180, 0);
        transform.LookAt(closestPlayer.transform);
        // transform.Rotate(0, 180, 0);
    }

    public void PickUp()
    {
        
    }
}
