using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using TMPro;
using Photon.Pun;
using UnityEngine.Accessibility;
using Unity.VisualScripting;

public class ChatBox : MonoBehaviourPun
{

    public TextMeshProUGUI chatLog;
    public TMP_InputField chatInput;
    

    public static ChatBox instance;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        chatLog.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            Cursor.lockState = CursorLockMode.None;
            if(EventSystem.current.currentSelectedGameObject == chatInput.gameObject)
            {
                onChatInputSend();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(chatInput.gameObject);
            }
            
        }
        
        
    }

    public void onChatInputSend()
    {
        if (chatInput.text.Length >= 2)
        {
            string name;
            PlayerController curPlayer = GameManager.instance.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            Color my_color = GameManager.instance.playerColors[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            string hexCC = toRGBHex(my_color);
            if (curPlayer.isDead)
            {
                 name = PhotonNetwork.LocalPlayer.NickName + "(Dead)";
            }
            else
            {
                name = PhotonNetwork.LocalPlayer.NickName;
            }
            photonView.RPC("log", RpcTarget.All, name, chatInput.text, hexCC);
            chatInput.text = "";
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
    

    [PunRPC]
    void log(string playerName, string message, string color)
    {
        chatLog.text += string.Format("<color={2}><b>{0}:</color></b> {1}\n", playerName, message, color);
        
    }
    public static string toRGBHex(Color c)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }
    public static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte) (f * 255);
    }
}
