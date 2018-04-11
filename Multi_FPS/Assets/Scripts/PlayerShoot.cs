using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    public Camera shootCam;
    public float weaponDamage = 10f;
    public float scaleChangeFactor = 0.1f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem shootSmokeEffect;
    public GameObject hitEffect;

    private Vector3 scaleChange;
    private Player selfPlayer;

    private void Start()
    {
        scaleChange = new Vector3(scaleChangeFactor, scaleChangeFactor, scaleChangeFactor);
        selfPlayer = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    // Is called on the server when the player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // Is called on all clients when we need to display shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        muzzleFlash.Play();
        shootSmokeEffect.Play();
    }

    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoImpactEffect(_pos, _normal);
    }

    [ClientRpc]
    void RpcDoImpactEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _tempHitEffect = (GameObject)Instantiate(hitEffect, _pos, Quaternion.LookRotation(_normal));
        Destroy(_tempHitEffect, 1f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // We are shooting, call the OnShoot method on the server.
        CmdOnShoot();

        RaycastHit targetHit;
        if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out targetHit))
        {
            if (targetHit.transform.CompareTag("Player"))
            {
                CmdFire(targetHit.transform.name, weaponDamage, scaleChange);
                selfPlayer.CmdChangeScale(scaleChange);
            }
            // We hit something - call OnHit on server with data
            CmdOnHit(targetHit.point, targetHit.normal);
        }
    }

    [Command]
    private void CmdFire(string targetName, float damage, Vector3 scaleDamage)
    {
        Player targetPlayer = GameManager.GetPlayer(targetName);

        targetPlayer.RpcTakeHealthDamage(damage, this.transform.name);
        targetPlayer.RpcChangeScale(-scaleDamage);
    }
}



