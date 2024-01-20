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

    [SerializeField] private float speed = 15;
    float CurrentDePosition;

    [HideInInspector] public bool CanMove = true;

    private Vector2 input;
    private bool isRight = true;


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

        //move character according to keyboard input
        Vector2 input = GetInput();
        MovePlayer(input);

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

    private void MovePlayer(Vector2 velocity)
    {
        rb.velocity = velocity.normalized * speed;
    }

    private Vector2 GetInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        return input;
    }

    void Turn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
