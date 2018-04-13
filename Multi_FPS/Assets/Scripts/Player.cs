using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public const float maxHealth = 100;
    public float currentHealth;
    public bool destroyOnDeath;
    public float scaleMax = 2f;
    public float scaleMin = 0.2f;
    [SyncVar]
    public int kills;
    public Renderer[] rendererToDisableOnDeath;


    private NetworkStartPosition[] spawnPositions;
    private Vector3 currentScale;
    private CursorLockMode lockMode;



    void Awake()
    {
        lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
        Cursor.visible = false;
    }

    private void Start()
    {
        SetDefaults();
        spawnPositions = FindObjectsOfType<NetworkStartPosition>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.SetKills(kills);
        }
    }

    void SetDefaults()
    {
        currentHealth = maxHealth;
        PlayerCanvas.canvas.SetHealth((int)currentHealth);
        currentScale = Vector3.one;
        transform.localScale = currentScale;

    }

    [ClientRpc]
    public void RpcTakeHealthDamage(float damageAmount, string playerWhoShot)
    {
        currentHealth -= damageAmount;
        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.SetHealth((int)currentHealth);
        }
        if (currentHealth <= 0)
        {
            GameManager.AddKillToPlayer(playerWhoShot);
            //Respawn();
            CmdDisableRenderers();
            StartCoroutine(PlayerKilled());
        }
    }

    [Command]
    public void CmdChangeScale(Vector3 scaleChange)
    {
        RpcChangeScale(scaleChange);
    }

    [ClientRpc]
    public void RpcChangeScale(Vector3 scaleChange)
    {
        if (currentScale.x <= scaleMin)
        {
            currentScale = new Vector3(scaleMin + 0.01f, scaleMin + 0.01f, scaleMin + 0.01f);
            transform.localScale = currentScale;
            transform.position += new Vector3(0, scaleChange.y, 0);
        }
        else if (currentScale.x > scaleMax)
        {
            currentScale = new Vector3(scaleMax, scaleMax, scaleMax);
            transform.localScale = currentScale;
            transform.position += new Vector3(0, scaleChange.y, 0);
        }
        else if (currentScale.x > scaleMin && currentScale.x <= scaleMax)
        {
            currentScale += scaleChange;
            transform.localScale = currentScale;
            transform.position += new Vector3(0, scaleChange.y, 0);
        }
    }

    void Respawn()
    {
        Vector3 spawnPoint = Vector3.zero;

        if (spawnPositions != null && spawnPositions.Length > 0)
        {
            spawnPoint = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].transform.position;
        }

        transform.position = spawnPoint;
        SetDefaults();
    }

    [Command]
    public void CmdDisableRenderers()
    {
        RpcDisableRenderers();
    }

    [ClientRpc]
    private void RpcDisableRenderers()
    {
        SetLocalGameText();

        for (int i = 0; i < rendererToDisableOnDeath.Length; i++)
        {
            rendererToDisableOnDeath[i].enabled = false;
        }

    }

    private void SetLocalGameText()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        PlayerCanvas.canvas.WriteGameStatusText("You died");
        PlayerCanvas.canvas.crossHair.enabled = false;
    }

    private void EnableRenderers()
    {
        PlayerCanvas.canvas.WriteGameStatusText("");
        PlayerCanvas.canvas.crossHair.enabled = true;

        for (int i = 0; i < rendererToDisableOnDeath.Length; i++)
        {
            rendererToDisableOnDeath[i].enabled = true;
        }

    }

    IEnumerator PlayerKilled()
    {
        Respawn();
        yield return new WaitForSeconds(3f);
        EnableRenderers();
    }
}
