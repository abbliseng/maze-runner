using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public void SpawnPlayerinos(Vector4 spawnBounds)
    {
        Vector2 randomPosition = new Vector2(Random.Range(spawnBounds.x, spawnBounds.y), Random.Range(spawnBounds.z, spawnBounds.w));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
