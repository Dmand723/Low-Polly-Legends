using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TextMeshPro text;
    [Header("Look sensitivity")]
    public float sensX;
    public float sensY;

    [Header("Clamping")]
    public float minY;
    public float maxY;

    [Header("Spectator")]
    
    public float startSpectatorMoveSpeed;

    [Header("Current")]
    public float spectatorMoveSpeed;
    public float rotX;
    public float rotY;
    public bool isSpectator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        spectatorMoveSpeed = startSpectatorMoveSpeed;
    }
    public void LateUpdate()
    {
        // get mouse inputs
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        rotY = Mathf.Clamp(rotY, minY, maxY);

        // if we are dead
        if(isSpectator )
        {
            // rotate the cam vertically
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0);

            // movement 
            float x = Input.GetAxis("Horizontal");
            float y = 0;
            float z = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.E))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                y = -1;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
               spectatorMoveSpeed= startSpectatorMoveSpeed * 2;
            }
            else if ( Input.GetKeyUp(KeyCode.LeftShift))
            {
                spectatorMoveSpeed = startSpectatorMoveSpeed;
            }
            Vector3 dir = transform.right*x + transform.up*y+transform.forward*z;

            transform.position += dir * spectatorMoveSpeed * Time.deltaTime;
            ray();
        }

       
        //if we are good
        else
        {
            // look camrea up/down
            transform.localRotation = Quaternion.Euler(-rotY, 0, 0);
            // look camrea left/right
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, rotX, 0);
        }
        
    }
    
    private void ray()
    {
        Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), transform.forward * 1);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 5, out hit, 10))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                int playerhealth = hit.transform.gameObject.GetComponent<PlayerController>().gethealth();
                text.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                text.text = playerhealth.ToString();
            }
            
        }
    }

    public void setSpectatior()
    {
        isSpectator = true;
        transform.parent = null;
        
    }

}
