using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;
using Photon.Pun;
using Photon.Realtime;
using System.ComponentModel;
using System.Data;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviourPun 
{
    #region Componets
    [Header("Components")]
    public Rigidbody rig;
    public LayerMask layerMaskinteract;
    public CameraController camera;
    public AudioSource audioSource;
   // public GameObject Inventory;
    public int punID;
    public Player photonPlayer;
    public MeshRenderer meshR;
    public Color skin;

    [Header("movemnet stats")]
    public float moveSpeed;
    public float jumpForce;

    [Header("player stats")]
    public int curHp;
    public int maxHp;
    public int lives;
    public int MaxAmmo;
    public int curAmmo;
    public int score;
    public bool isDead;

    [Header("End game stats")]
    public int damageTaken;
    public int kills;
    public int deaths;
    public int damageDelt;
    public int shotsFired;
    public int shotsHit;

    [Header("Options")]
    public ValueAttribute InteractButton;

    [Header("Wepons")]
    public bool has_pistol_1;
    public bool has_pistol_2;
    public bool has_rifle_3;
    public bool has_sniper;
    public bool has_smg_1;
    public bool has_shotGun;
   
    public int gunIndex;
    public Transform[] gunsList;
    public PlayerWeapon selectedWeapon;

    [Header("others")]
    private int curAttackerID;
    private bool isFlashing;

    [Header("Debug")]
    public bool debug;
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<CameraController>();
        meshR = GetComponent<MeshRenderer>();
        curAmmo = MaxAmmo;
        curHp = maxHp;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Inventory.SetActive(false);
        gunIndex = 0;
        swapGun(gunIndex);

    }

    // Update is called once per frame
    void Update()
    {
       
        if(!photonView.IsMine)
        {
            return;
        }
       /* if(isDead)
        {
            return;
        }*/
  
        move();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tryJump();  
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Tryinteract();
        }
        /* if(Input.GetKeyDown(KeyCode.Tab))
         {
             Inventory.SetActive(true);
         }
         if(Input.GetKeyUp(KeyCode.Tab))
         {
             Inventory.SetActive(false);
         }*/
        if(selectedWeapon.isAuto)
        {
            if (Input.GetMouseButton(0))
            {
                print("trigger pulled");
                pullTrigger();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("trigger pulled");
                pullTrigger();
            }
        }
        
       
        if (Input.GetKeyDown(KeyCode.R))
        {
            tryReload();
        }
        gunSwapKeyPress(gunIndex);
    }

    [PunRPC]
    public void Initialize(Player player)
    {
        punID = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[punID - 1] = this;

        if(!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            rig.isKinematic = true;
        }
        skin = GameManager.instance.playerColors[punID - 1];
        meshR.material.color = skin;

    }

    private void move()
    {
        //get inputs 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;
        dir.y = rig.velocity.y;

        rig.velocity = dir;
    }

    private void tryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, 1.5f))
        {
            rig.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }
    }

    private void pullTrigger()
    {
        selectedWeapon.tryShoot();
    }

    public void heal()
    {

    }

    public void addAmmo()
    {

    }

    [PunRPC]
    public void takeDamage(int attacker, int dmg)
    {
        if (isDead)
        {
            return;
        }
        curHp -= dmg;
        curAttackerID = attacker;

        // flash damage
        photonView.RPC("DamageFlash", RpcTarget.Others);
        //update ui

        //if hp == 0 kill player
        if(curHp <= 0)
        {
            photonView.RPC("die", RpcTarget.All);
        }
    }

    [PunRPC] //do nect time
    public void DamageFlash()
    {
        if (isFlashing)
        {
            return;
        }
        StartCoroutine(damageFLashCoRoutine(Color.red));

       
    }
    IEnumerator damageFLashCoRoutine(Color color)
    {
        isFlashing = true;
        for (int i = 0; i < 3; i++)
        {
            meshR.material.color = color;
            yield return new WaitForSeconds(.05f);
            meshR.material.color = skin;
        }
        isFlashing = false;
    }

    [PunRPC]
    public void die()
    {
        //skin = Color.black;
        //meshR.material.color = Color.black;
        curHp = 0;
        isDead = true;
        
        GameManager.instance.playersAlive--;
        if(PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.checkWinCondition();
        }

        if (photonView.IsMine)
        {
            if(curAttackerID !=0)
            {
                GameManager.instance.getPlayer(curAttackerID).photonView.RPC("addKill", RpcTarget.All);
                
            }
            camera.setSpectatior();
            rig.isKinematic = true; transform.position = new Vector3(0, -1000, 0); 
            //make it so you are laying on the ground and can be kicked around lol
           // rig.constraints = RigidbodyConstraints.None;

        }
        GameManager.instance.photonView.RPC("spawnGravestone", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, transform);
    }

    [PunRPC]
    public void addKill()
    {
        kills++;
    }

    public void gunSwapKeyPress(int index)
    {
         int newIndex = index;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            newIndex++;
            if (newIndex < 0)
            {
                newIndex = gunsList.Length-1;
            }
            if (newIndex > gunsList.Length -1)
            {
                newIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            newIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            newIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            newIndex = 5;
        }
        else
        {
            return;
        }
        
        swapGun(newIndex);
        gunIndex = newIndex;
    }
    
    public void swapGun(int index)
    {
        foreach(Transform gun in gunsList)
        {
            gun.gameObject.SetActive(false);
        }
        gunsList[index].gameObject.SetActive(true);
        selectedWeapon = gunsList[index].GetComponent<PlayerWeapon>();
    }

    [PunRPC]
    public void spawnBullet(Vector3 muzzlePos, Vector3 dir, int index)
    {
        gunsList[index].GetComponent<PlayerWeapon>().spawnBullet(muzzlePos, dir);
    }
   
    public void tryReload()
    {
        if (curAmmo >= selectedWeapon.maxClip)
        {
            curAmmo -= selectedWeapon.maxClip - selectedWeapon.curClip;

            selectedWeapon.reload();
        }
        else
        {
            if (selectedWeapon.curClip + curAmmo <= selectedWeapon.maxClip)
            {
                selectedWeapon.reload(curAmmo);
                curAmmo = 0;
            }
            else
            {
                int neededbullets = selectedWeapon.maxClip - selectedWeapon.curClip;
                selectedWeapon.reload(neededbullets);
                curAmmo -= neededbullets;
            }
        }
    }

    public void Tryinteract()
    {

        Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y+1, this.transform.position.z), camera.transform.forward*1);
        RaycastHit hit;
        if(Physics.SphereCast(ray,5, out hit, 10))
        {
            if(hit.transform.gameObject.CompareTag("Interactable"))
            {
                 hit.transform.gameObject.GetComponent<Interactable>().Interact.Invoke();
            }
            
        }

        if (debug)
        {
            print("draw?");
            Debug.DrawRay(this.transform.position, camera.transform.forward * 3, Color.red, 2);
        }
    }
   public int gethealth()
    {
        return curHp;
    }
}
