using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    #region componets
    public GameObject LoadingImage;
    public TextMeshProUGUI MessageTxt;

    [Header("Menu Screens")]
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject roomLobbyScreen;
    public GameObject roomSelectLobbyScreen;

    [Header("Main Screen Componets")]
    public TMP_InputField name_input;
    public Button createRoomButton_ms;
    public Button findRoomButton_ms;

    [Header("Create Room Screen Componets")]
    public TMP_InputField roomName_crs;
    public Button createRoomButton_crs;
    public Button backButton_crs;

    [Header("Room Lobby Components")]
    public Button startGameButton_rls;
    public TextMeshProUGUI playerListTextBox;
    public TextMeshProUGUI roomNameText;
    public TMP_Dropdown levelSelect;

    [Header("Room Selection Lobby Componets")]
    public RectTransform roomListContainer;
    public GameObject roomButtonPrefab;

    [SerializeField]
    private List<GameObject> roomButtonList = new List<GameObject>();
    [SerializeField]
    private List<RoomInfo> roomInfoList = new List<RoomInfo>();

    public string name;
    public string roomName;
    public string selected_level;

    #endregion

    #region unity Methods
    private void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            name_input.interactable = true;
            EventSystem.current.SetSelectedGameObject(name_input.gameObject);
            LoadingImage.SetActive(false);
            hideMessage();
        }
        // making bttns non interacble until network is established
        createRoomButton_ms.interactable = false;
        findRoomButton_ms.interactable = false;
        name_input.interactable = false;
        LoadingImage.SetActive(true);
        showMessage("Conecting to Server");
        // unhide our curor
        Cursor.lockState = CursorLockMode.None;

        // check if in game
        if(PhotonNetwork.InRoom)
        {
            // go to the lobby 

            // make room visible
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;



        }
    }
    #endregion

    #region all Screen
    void setScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        roomLobbyScreen.SetActive(false);
        roomSelectLobbyScreen.SetActive(false);

        screen.SetActive(true);
        if(screen == mainScreen)
        {
            EventSystem.current.SetSelectedGameObject(name_input.gameObject);
        }
        if(screen == createRoomScreen)
        {
            EventSystem.current.SetSelectedGameObject(roomName_crs.gameObject);
        }
        if(screen == roomSelectLobbyScreen)
        {
            updateRoomSelectLobby();
        }
    }

    public void showMessage(string messageText)
    {
        MessageTxt.text = messageText;
        MessageTxt.gameObject.SetActive(true);
    }

    public void hideMessage()
    {
        MessageTxt.text = null;
        MessageTxt.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        setScreen(mainScreen);
    }
    #endregion

    #region main Screen

    public override void OnConnectedToMaster()
    {
        name_input.interactable = true;
        EventSystem.current.SetSelectedGameObject(name_input.gameObject);
        LoadingImage.SetActive(false);
        hideMessage();
        
    }

    public void onPlayerNameValueChanged()
    {
         name = name_input.text;
       if(name.Length > 2 && name.Length < 15 )
        {
            if(!createRoomButton_ms.IsInteractable() )
            {
                hideMessage();
                createRoomButton_ms.interactable = true;
                findRoomButton_ms.interactable = true;
            }
        }
       else if(name.Length > 14)
        {
            showMessage("Name too long.");
            createRoomButton_ms.interactable = false;
            findRoomButton_ms.interactable = false;
        }
       else if (name.Length < 3)
        {
            showMessage("Name too short.");
            createRoomButton_ms.interactable = false;
            findRoomButton_ms.interactable = false;
        }
       
        
        
    }

    public void OnCreateRoomButton_ms()
    {
        PhotonNetwork.NickName = name;
        roomName_crs.text = null;
        createRoomButton_crs.interactable = false;
       
        setScreen(createRoomScreen);
    }

    public void OnFindRoomButton_ms()
    {
        PhotonNetwork.NickName = name;
        setScreen(roomSelectLobbyScreen);
    }

    #endregion

    #region create Room

    public void OnRoomNameChanged()
    {
        roomName = roomName_crs.text;
        if (name.Length > 2 && name.Length < 11)
        {
            if (!createRoomButton_crs.IsInteractable())
            {
                hideMessage();
                createRoomButton_crs.interactable = true;
               
            }
        }
        else if (name.Length > 10)
        {
            showMessage("Room name too long.");
            createRoomButton_crs.interactable = false;
            
        }
        else if (name.Length < 3)
        {
            showMessage("Room name too short.");
            createRoomButton_crs.interactable = false;
            
        }
    }

    public void OnCreateRoom_crs()
    {
        NetworkManager.instance.createRoom(roomName);
        
    }
    #endregion

    #region room Lobby

    public override void OnJoinedRoom()
    {
        setScreen(roomLobbyScreen);
        photonView.RPC("updateRoomLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    void updateRoomLobbyUI()
    {
        // update room name
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        // update player list
        playerListTextBox.text = "";
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerListTextBox.text += player.NickName + "\n";
        }

        //disable or enable buttons
        startGameButton_rls.interactable = PhotonNetwork.IsMasterClient;
        levelSelect.interactable = PhotonNetwork.IsMasterClient;

        selected_level = "Arena1";

        selected_level = levelSelect.options[0].text;

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updateRoomLobbyUI();
    }

    public void onLeaveRoomBttn_rl()
    {
        PhotonNetwork.LeaveRoom();
        setScreen(mainScreen);
    }

    public void onSelectLevelChange()
    {
        selected_level = levelSelect.options[levelSelect.value].text;
    }

    public void onStartGameBttn_rl()
    {
        //Make the room not visable;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        NetworkManager.instance.photonView.RPC("changeScenes", RpcTarget.All, selected_level);
    }


    #endregion

    #region room Select

    public void OnCreateRoomButton_rsl()
    {
        roomName_crs.text = null;
        createRoomButton_crs.interactable = false;
        setScreen(createRoomScreen);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfoList = roomList;
    }

    public GameObject createNewRoomBttn()
    {
        GameObject bttn = Instantiate(roomButtonPrefab, roomListContainer.transform);
        roomButtonList.Add( bttn );
        return bttn;
    }

    public void onJoinRoomBttn_rls(string roomName)
    {
        foreach (RoomInfo roomInfo in roomInfoList)
        {
            if(roomInfo.Name == roomName)
            {
                NetworkManager.instance.joinRoom(roomName);
                break;
            }
        }
       
    }

    public void onRefreshBttn()
    {
        updateRoomSelectLobby();
    }

    public void updateRoomSelectLobby()
    {
        foreach (GameObject bttn in roomButtonList)
        {
            bttn.SetActive(false);
        }
        foreach (RoomInfo room in roomInfoList)
        {
            if (room.PlayerCount <= 0)
            {
                roomInfoList.Remove(room);
            }
        }
        for(int i = 0;i < roomInfoList.Count;i++)
        {
            GameObject bttn = i >= roomButtonList.Count ? createNewRoomBttn(): roomButtonList[i];
            bttn.SetActive(true);
            bttn.transform.Find("roomNameText").GetComponent<TextMeshProUGUI>().text = roomInfoList[i].Name;
            bttn.transform.Find("playerCountText").GetComponent<TextMeshProUGUI>().text = roomInfoList[i].PlayerCount.ToString()+ "/" + roomInfoList[i].MaxPlayers.ToString();
            string rn = roomInfoList[i].Name;
            Button bttncomp = bttn.GetComponent<Button>();
            bttncomp.onClick.RemoveAllListeners();
            bttncomp.onClick.AddListener(() => { onJoinRoomBttn_rls(rn);
                print("this is a lambda funtion");});
        }
    }

    #endregion
}

