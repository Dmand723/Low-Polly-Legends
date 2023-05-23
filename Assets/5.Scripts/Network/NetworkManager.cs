using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    public int maxPlayers = 16;

    public static NetworkManager instance;
    private void Awake()
    {
       instance = this;
       DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // connect to the master server
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        print("We have conected to the master server");
        PhotonNetwork.JoinLobby();
    }

    // called when we create a new room
    public void createRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    // callled when we join a room
    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        print("we have joined " + PhotonNetwork.CurrentRoom.Name);
    }

    /*public override void OnJoinedLobby()
    {
        print("joined the lobby" + PhotonNetwork.CurrentRoom.Name);
        print(PhotonNetwork.GetPing().ToString());
    }*/

    [PunRPC]
    public void changeScenes(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void onQuitBttn()
    {
        Application.Quit();
    }
}


