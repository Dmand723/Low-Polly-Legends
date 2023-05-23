using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviourPun
{
    #region Componets

    public float postGameTime = 5;

    public static GameManager instance;
    [Header("Player Vars")]
    public string playerPrefabLocation;
    public PlayerController[] players;
    public Transform[] spawnPoints;
    public List<Transform> tempSPs;
    public int playersAlive;

    private int playersInGame;

    public Color[] playerColors;

    public GameObject gravestonePrefab;

    #endregion



    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        postGameTime = 5;
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        playersAlive = players.Length;
        foreach(Transform Point in spawnPoints)
        {
            tempSPs.Add(Point);
        }
        
        photonView.RPC("imInGame", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void imInGame()
    {
        playersInGame++;
        if(PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("spawnPlayer", RpcTarget.All);
        }
        
    }

    public Transform randomSpawnpoint()
    {
      
        int rng = Random.Range(0,tempSPs.Count);
        Transform spawnPoint = tempSPs[rng];
        tempSPs.Remove(spawnPoint);
        return spawnPoint;
    }

    [PunRPC]
    void spawnPlayer()
    {
        print("cherckPOlayerCount");
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, randomSpawnpoint().position, Quaternion.identity);
        playerObj.GetComponent<PlayerController>().photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    public PlayerController getPlayer(int playerId)
    {
        return players.First(x => x.punID == playerId);
    }
    public PlayerController getPlayer(GameObject playerobj)
    {
        return players.First(x => x.gameObject == playerobj);
    }

    public void checkWinCondition()
    {
        if(playersAlive <=1)
        {
            int winnerID = players.First(x => !x.isDead).punID;
            photonView.RPC("winGame", RpcTarget.All, winnerID);
        }
    }
    [PunRPC]
    void winGame(int winID)
    {
        //  set the ui win 

        Invoke("goBackToMenu", postGameTime);
    }
    
    void goBackToMenu()
    {
        NetworkManager.instance.changeScenes("Menu");
    }

    [PunRPC]
    public void spawnGravestone(string name, Transform pos)
    {
        GameObject grave =  Instantiate(gravestonePrefab);
        grave.transform.position = new Vector3(pos.position.x, .06f, pos.position.z);
        grave.GetComponentInChildren<TextMeshPro>().text = "Here Lies " + name;
    }
}
