using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Vars
    public string weponName;

    [Header("Components")]
    public PlayerController owner;
    public Transform muzzlePos;
    public Animator animator;
    public AudioSource audioS;
    public AudioClip[] FXClips;

    [Header("WeponStats")]
    public int curClip;
    public int maxClip;
    public int index;
    public bool isAuto;
   

    [Header("Shooting Stats")]
    public float fireRate;
    public float lastShotTime;
    public float recoil;

    [Header("prefabs")]
    public GameObject bulletPrefab;
    public GameObject flashPrefab;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        owner = GetComponentInParent<Transform>().GetComponentInParent<PlayerController>();
        //muzzlePos = GameObject.FindWithTag("muzzle").GetComponent<Transform>();
    }

    #endregion

    #region gun methods 
    public void tryShoot()
    {
        
        if(curClip <=0 || Time.time - lastShotTime < fireRate)
        {
            return;
        }
        curClip--;
        lastShotTime = Time.time;
        //update ui

        //spawn a bullet
       owner.photonView.RPC("spawnBullet", RpcTarget.All, muzzlePos.position, Camera.main.transform.forward,index);
        if(owner.debug)
        {
            print("you shot " + weponName);
        }
        //play sound
        audioS.PlayOneShot(FXClips[index]);
        //play animation
        GameObject flash = Instantiate(flashPrefab, muzzlePos.position, Quaternion.identity);
        flash.transform.forward = Camera.main.transform.forward;
        Destroy(flash, .5f);
    }

    public void reload()
    {
        if(curClip == maxClip)
        {
            return;
        }
        curClip = maxClip;
    }
    public void reload(int bullets)
    {
        curClip += bullets;
    }


    public void spawnBullet(Vector3 muzzlepos, Vector3 dir)
    {
        GameObject bulletobj = Instantiate(bulletPrefab, muzzlepos, Quaternion.identity);
        bulletobj.transform.forward = dir;
        Bullet bullet = bulletobj.GetComponent<Bullet>();
        bullet.initilized(dir, owner.punID, owner.photonView.IsMine);
    }
    #endregion
}
