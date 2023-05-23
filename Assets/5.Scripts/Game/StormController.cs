using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormController : MonoBehaviour
{

    public float shrinkWaitTime, shrinkAmmout, shrinkDir, minShrinkAmmount, lastShrinkTime, targetDieameter, lastDmgTime;
    public int playerDamage;
    public bool shrinking;


    // Start is called before the first frame update
    void Start()
    {
        lastShrinkTime = Time.time;
        targetDieameter = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayer();
        if(shrinking)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * targetDieameter, (shrinkAmmout / shrinkDir) * Time.deltaTime);
            if (transform.localScale.x == targetDieameter)
            {
                shrinking = false;
            }



        }
        else
        {
            if (Time.time - lastShrinkTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmmount)
            {
                shrink();
            }
        }
    }

    void shrink()
    {
        shrinking = true;
        if (transform.localScale.x - shrinkAmmout > minShrinkAmmount)
        {
            targetDieameter -=shrinkAmmout;
        }
        else
        {
            targetDieameter = shrinkAmmout;
        }
        lastShrinkTime = Time.time + shrinkDir;
    }

    void checkPlayer()
    {
        if (Time.time - lastDmgTime > 1.0f)
        {
            lastDmgTime = Time.time;

            foreach (PlayerController player in GameManager.instance.players) 
            {
                if (player.isDead || !player)
                {
                    continue;
                }
                if (Vector3.Distance(Vector3.zero, player.transform.position) >= transform.localScale.x)
                {
                    player.photonView.RPC("takeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }
}
