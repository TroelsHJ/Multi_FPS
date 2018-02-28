using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
    public bool destroyOnDeath;
    private NetworkStartPosition[] spawnPositions;


    private void Start()
    {
        if (isLocalPlayer)
        {
            spawnPositions = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }

    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick a spawn point at random
            if (spawnPositions != null && spawnPositions.Length > 0)
            {
                spawnPoint = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

}
