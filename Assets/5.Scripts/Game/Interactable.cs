using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    public UnityEvent Interact;

    public EEController EEController;
    
    [Header("UIPos")]
    public Transform NumPos;
    
    
    public void test()
    {
        print("interacted");
    }

    public void pickUpGreenNumPaper()
    {
        EEController.GreenNumUI.SetActive(true);
        EEController.GreenNumUI.GetComponentInChildren<TextMeshProUGUI>().text = EEController.green;
    }
    public void pickUpBlueNumPaper()
    {
        EEController.BlueNumUI.SetActive(true);
        EEController.BlueNumUI.GetComponentInChildren<TextMeshProUGUI>().text = EEController.blue;
    }
    public void pickUpRedNumPaper()
    {
        EEController.RedNum.SetActive(true);
        EEController.RedNumUI.GetComponentInChildren<TextMeshProUGUI>().text = EEController.red;
    }
    public void pickUpPinkNumPaper()
    {
        EEController.PinkNumUI.SetActive(true);
        EEController.PinkNumUI.GetComponentInChildren<TextMeshProUGUI>().text = EEController.pink;
    }
}
