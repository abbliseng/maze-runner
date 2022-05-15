using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Flashlight : MonoBehaviourPunCallbacks
{

    public GameObject lightSource;
    // public AudioSource clickSound;
    public bool failSafe = false;

    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lightSource.activeSelf && !failSafe)
            {
                photonView.RPC("turnOffFlashlight", RpcTarget.All);
            } else if (!lightSource.activeSelf && !failSafe)
            {
                photonView.RPC("turnOnFlashlight", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void turnOffFlashlight()
    {
        failSafe = true;
        lightSource.SetActive(false);
        StartCoroutine(FailSafe());
    }
    [PunRPC]
    void turnOnFlashlight()
    {
        failSafe = true;
        lightSource.SetActive(true);
        StartCoroutine(FailSafe());
    }
    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }
}
