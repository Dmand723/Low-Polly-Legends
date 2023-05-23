using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Keypad : MonoBehaviour
{
    public string InteractTag;
    public string correctPasscode;
    public string passcodeInput;
    public bool unlocked;
    public bool debug;
    public UnityEvent UnlockEvent;
    public string getInteractTag()
    {
        return InteractTag;
    }
    public void num1()
    {
        passcodeInput += 1.ToString();
        if(debug)
        {
            print("pressed1");
        }
    }
    public void num2() 
    { 
        passcodeInput+= 2.ToString();
        if (debug)
        {
            print("pressed2");
        }
    } 

    public void num3() 
    { 
        passcodeInput+= 3.ToString();
        if (debug)
        {
            print("pressed3");
        }
    }
    public void num4() 
    {
        passcodeInput+= 4.ToString();
        if (debug)
        {
            print("pressed4");
        }
    }
    public void num5() 
    { 
        passcodeInput+= 5.ToString();
        if (debug)
        {
            print("pressed5");
        }
    }
    public void num6() 
    { 
        passcodeInput+= 6.ToString();
        if (debug)
        {
            print("pressed6");
        }
    }
    public void num7() 
    { 
        passcodeInput+= 7.ToString();
        if (debug)
        {
            print("pressed7");
        }
    }
    public void num8() 
    { 
        passcodeInput+= 8.ToString();
        if (debug)
        {
            print("pressed8");
        }
    }
    public void num9() 
    { 
        passcodeInput+= 9.ToString();
        if (debug)
        {
            print("pressed9");
        }
    }
    public void num0() 
    { 
        passcodeInput+= 0.ToString();
        if (debug)
        {
            print("pressed0");
        }
    }
    public void star()
    {
        clearInput();
        if (debug)
        {
            print("pressedStar");
        }
    }
    public void pound()
    {
        tryPasscode();
        if (debug)
        {
            print("pressedPound");
        }
    }
    private void tryPasscode()
    {
        if(passcodeInput == correctPasscode)
        {
            unlocked= true;
            UnlockEvent.Invoke();
            if (debug)
            {
                print("Unlocked");
            }
        }
        else
        {
            if (debug)
            {
                print("Incorrect PassCode");
            }
            clearInput();
        }
    }

    private void clearInput()
    {
        passcodeInput = null;
    }
    


}
