using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoverAndRotate : MonoBehaviour
{
    public float rotationSpeedX;
    public float rotationSpeedY;
    //public bool hoverOn = false;
    //public float hoverSpeed;
    //public float hoverRange;

    public float moveSpeed = 0;
    public int moveDirectionX = 0;
    public int moveDirectionZ = 0;
    public float xLimit;
    public float zLimit;

    float initalPosition;
    int hoverDirection = 1;

    void Start()
    {
        initalPosition = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeedX);
        transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeedY);
        //Hover();
        MoveObject();        
    }

    //void Hover()
    //{
    //    if (hoverOn)
    //    {
    //        if (transform.position.y > (initalPosition + hoverRange))
    //        {
    //            hoverDirection = -1;
    //        }
    //        else if (transform.position.y < (initalPosition - hoverRange))
    //        {
    //            hoverDirection = 1;
    //        }

    //        transform.Translate(Vector3.up * hoverDirection * Time.deltaTime * hoverSpeed);
    //    }
    //}

    void MoveObject()
    {
        if (moveSpeed > 0)
        {
            if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
            {
                transform.Translate(new Vector3(moveDirectionX, 0, moveDirectionZ) * Time.deltaTime * moveSpeed, Space.World);
                if (transform.position.x * xLimit >= xLimit * xLimit)
                {
                    transform.position = new Vector3(xLimit, transform.position.y, transform.position.z);
                }
                if (transform.position.z * zLimit >= zLimit * zLimit)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zLimit);
                }
            }
        }
    }
}
