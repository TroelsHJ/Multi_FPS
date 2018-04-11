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
    public ParticleSystem smokeEffect;

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

    [Client]
    void Shoot()
    {
        muzzleFlash.Play();
        smokeEffect.Play();
        RaycastHit targetHit;
        if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out targetHit))
        {
            if (targetHit.transform.CompareTag("Player"))
            {
                CmdFire(targetHit.transform.name, weaponDamage, scaleChange);
                selfPlayer.CmdChangeScale(scaleChange);
            }
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



