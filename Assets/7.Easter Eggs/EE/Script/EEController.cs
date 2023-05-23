using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EEController : MonoBehaviour
{
   
    public GameObject GreenNum;
    public GameObject BlueNum;
    public GameObject RedNum;
    public GameObject PinkNum;
    public Keypad keypad;
 
    public Transform[] Greenspawnpoints;
    public Transform[] Bluespawnpoints;
    public Transform[] Redspawnpoints;
    public Transform[] Pinkspawnpoints;

    public GameObject GreenNumUI;
    public GameObject BlueNumUI;
    public GameObject RedNumUI;
    public GameObject PinkNumUI;

    public string green;
    public string blue;
    public string red;
    public string pink;
    private void Awake()
    {
        Transform GreenSpawn = Greenspawnpoints[Random.Range(0, Greenspawnpoints.Length)];
        Transform BlueSpawn = Bluespawnpoints[Random.Range(0, Bluespawnpoints.Length)];
        Transform RedSpawn = Redspawnpoints[Random.Range(0, Redspawnpoints.Length)];
        Transform PinkSpawn = Pinkspawnpoints[Random.Range(0, Pinkspawnpoints.Length)];
        green = GreenNum.GetComponentInChildren<TextMeshPro>().text = Random.Range(0,9).ToString();
        blue = BlueNum.GetComponentInChildren<TextMeshPro>().text = Random.Range(0,9).ToString();
        red = RedNum.GetComponentInChildren<TextMeshPro>().text = Random.Range(0,9).ToString();
        pink = PinkNum.GetComponentInChildren<TextMeshPro>().text = Random.Range(0,9).ToString();

        GreenNum.transform.position = GreenSpawn.position;
        BlueNum.transform.position = BlueSpawn.position;
        RedNum.transform.position = RedSpawn.position;
        PinkNum.transform.position = PinkSpawn.position;

        GreenNum.transform.rotation = GreenSpawn.rotation;
        BlueNum.transform.rotation = BlueSpawn.rotation;
        RedNum.transform.rotation = RedSpawn.rotation;
        PinkNum.transform.rotation = PinkSpawn.rotation;

        keypad.correctPasscode = green +  blue + red + pink;
        
    }
}
