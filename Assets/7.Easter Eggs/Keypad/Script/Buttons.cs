using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour
{
    public Keypad Keypad;

    public UnityEvent OnInteract;

    
    private void OnCollisionEnter(Collision collision)
    {
        print("pressedButton");
        if (collision.gameObject.CompareTag(Keypad.getInteractTag()))
        {
            OnInteract.Invoke();
        }
    }

}
