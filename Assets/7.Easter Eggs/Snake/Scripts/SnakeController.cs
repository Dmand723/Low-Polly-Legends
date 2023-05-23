 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Moving")]
    private bool movingUp;
    private bool movingDown;
    private bool movingLeft;
    private bool movingRight;
    public float moveAmmout;

    [Header("Turning")]
    public Vector2[] turnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (movingUp)
        {
          //  transform.position += moveAmmout;
        }
        if (movingDown)
        {

        }
        if (movingLeft)
        {

        }
        if(movingRight)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            switchMovement(movingDown);
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            switchMovement(movingUp);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            switchMovement(movingLeft);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            switchMovement(movingRight);
        }
    }

    public void switchMovement(bool moveBool)
    {
        movingUp = false;
        movingLeft = false; 
        movingRight = false;
        movingDown = false;

        moveBool = true;
    }
}
