using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject DamageText;
    [SerializeField] List<GameObject> Spells;

    BarManager HpBar;

    Rigidbody2D rb;
    Camera MainCam;

    Vector3 MousePosFromPlayer;
    Vector3 KnocbackDirection = Vector3.zero;

    int CurrentSpell = 0;

    bool Shake = false;
    float ShakeStopWatch = 0;
    readonly float ShakeDuration = 0.1f;
    float ShakePower = 0.25f;

    float Speed = 15;
    float CurrentDePosition;

    float KnockbackStopwatch = 0;
    float KnockbackDuration = 0.05f;
    bool Knockbacking = false;

    [HideInInspector] public bool CanMove = true;

    bool Right = false;
    bool Left = false;
    bool Up = false;
    bool Down = false;

    bool isRightLooking = true;

    float MaxHp = 3500;
    float Hp;

    //Vector3 rotation = Vector3.zero;

    void Start()
    {
        HpBar = transform.parent.Find("HealthBar").GetComponent<BarManager>();
        rb = GetComponent<Rigidbody2D>();
        MainCam = Camera.main;

        CanMove = true;
        Hp = MaxHp;
    }

    void Update()
    {
        Vector3 mousePosition = MainCam.ScreenToWorldPoint(Input.mousePosition);
        MousePosFromPlayer = mousePosition - transform.position;

        #region Taking inputs for basic movement

        Right = Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);

        Left = Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow);

        Up = Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow);

        Down = Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow);
        #endregion

        #region Movement

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

            if (Knockbacking)
            {
                float percentageLeft = (KnockbackDuration - KnockbackStopwatch) / KnockbackDuration;
                rb.velocity = new Vector3(x + (KnocbackDirection.normalized.x * percentageLeft) * 30, y + (KnocbackDirection.normalized.y * percentageLeft) * 30, 0);
            }
            else
                rb.velocity = new Vector3(x , y, 0);
        }
        #endregion

        #region Move/Shake Camera and knockback

        Vector3 CamPos = transform.position + (MousePosFromPlayer / 3.5f);
        if (!Shake)
        {
            MainCam.transform.position = new Vector3(CamPos.x, CamPos.y, -10);
        }
        else //Shaking
        {
            MainCam.transform.position = new Vector3(CamPos.x, CamPos.y, -10) +
                new Vector3(Random.Range(-ShakePower, ShakePower), Random.Range(-ShakePower, ShakePower), 0);

            ShakeStopWatch += Time.deltaTime;
            if (ShakeStopWatch > ShakeDuration)
            {
                ShakeStopWatch = 0;
                Shake = false;
            }
        }

        if(Knockbacking)
        {
            KnockbackStopwatch += Time.deltaTime;
            if(KnockbackStopwatch > KnockbackDuration)
            {
                Knockbacking = false;
                KnockbackStopwatch = 0;
            }
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch(CurrentSpell)
            {
                case 0:
                    Knockback();
                    ShakeScreen(0.25f);
                    Instantiate(Spells[0], transform.position, Quaternion.identity, null).
                        GetComponent<ExplodingSpell>().direction = MousePosFromPlayer.normalized;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damaged((int)(Random.value * 200f));
        }
    }

    public void Damaged(int damage)
    {
        Hp -= damage;
        Instantiate(DamageText, transform.position, Quaternion.identity, null).GetComponent<DamageTextController>().Damage = damage;
        HpBar.percentage = Hp / MaxHp;

        Debug.Log(Hp);

        if(Hp < 0)
        {
            //die*********************************************************************************************
        }
    }

    void Knockback()//adds to time or starts
    {
        Knockbacking = true;
        KnocbackDirection = -1 * MousePosFromPlayer.normalized * Speed * 2;
        KnockbackStopwatch -= KnockbackDuration;
    }

    void ShakeScreen(float power)//adds to time or starts
    {
        ShakePower = power;
        Shake = true;
        ShakeStopWatch -= ShakeDuration;
    }

    void Turn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
