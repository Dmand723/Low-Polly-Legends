using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("bulletStats")]
    public int dmg;
    public float speed;
    public float range;
    public int attackerID;
    public bool isMine;

    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
   
   
    public void initilized(Vector3 dir, int id, bool ismine )
    {
        rig.velocity = dir * speed;
        isMine = ismine;
        attackerID = id;
        Destroy(gameObject, range);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&& isMine)
        {
            PlayerController otherPlayer = GameManager.instance.getPlayer(other.gameObject);
            if(otherPlayer.punID != attackerID)
            {
                otherPlayer.photonView.RPC("takeDamage", otherPlayer.photonPlayer, attackerID, this.dmg);
            }
        }
        Destroy(gameObject);
    }
}
