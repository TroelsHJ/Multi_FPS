using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    private Camera sceneCam;
    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCam = Camera.main;
            if (sceneCam != null)
            {
                sceneCam.gameObject.SetActive(false);
            }
        }
    }


    void Update()
    {

    }
}
