using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Camera MainCam;

    Vector3 MousePosFromPlayer;

    bool Shake = false;
    float ShakeStopWatch = 0;
    readonly float ShakeDuration = 0.2f;

    float Speed = 15;
    float CurrentDePosition;

    [HideInInspector] public bool CanMove = true;

    bool Right = false, OldRight = false;
    bool Left = false, OldLeft = false;
    bool Up = false;
    bool Down = false;

    bool isRightLooking = true;

    //Vector3 rotation = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MainCam = Camera.main;

        CanMove = true;
    }

    void Update()
    {
        Vector3 mousePosition = MainCam.ScreenToWorldPoint(Input.mousePosition);
        MousePosFromPlayer = mousePosition - transform.position;


        #region Taking inputs for basic movement

        OldLeft = Left;
        OldRight = Right;

        Right = Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);

        Left = Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow);

        Up = Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow);

        Down = Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow);
        #endregion

        #region Basic movement

        CurrentDePosition = Speed;

        if (CanMove)
        {
            float x = 0;
            float y = 0;

            if (Right)
            {
                if (!Left)
                {
                    x = CurrentDePosition;
                    if (!isRightLooking)
                    {
                        isRightLooking = true;
                        Turn();
                    }
                }
            }
            else if (Left)
            {
                x = -CurrentDePosition;
                if (isRightLooking)
                {
                    isRightLooking = false;
                    Turn();
                }
            }

            if (Up)
            {
                if (!Down)
                {
                    y = CurrentDePosition;
                }
            }
            else if (Down)
            {
                y = -CurrentDePosition;
            }

            if (x != 0 && y != 0)
            {
                x *= 0.707106781f;
                y *= 0.707106781f;
            }

            rb.velocity = new Vector3(x, y, 0);
        }
        #endregion

        #region Move/Shake Camera

        Vector3 CamPos = transform.position + (MousePosFromPlayer / 3.5f);
        if (!Shake)
        {
            MainCam.transform.position = new Vector3(CamPos.x, CamPos.y, -10);
        }
        else //Shaking
        {
            MainCam.transform.position = new Vector3(CamPos.x, CamPos.y, -10) +
                new Vector3(Random.Range(-0.45f, 0.45f), Random.Range(-0.45f, 0.45f), 0);

            transform.position += Time.deltaTime * -6f * MousePosFromPlayer.normalized;

            ShakeStopWatch += Time.deltaTime;
            if (ShakeStopWatch > ShakeDuration)
            {
                ShakeStopWatch = 0;
                Shake = false;
            }
        }
        #endregion
    }

    void Turn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
