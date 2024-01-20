using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextController : MonoBehaviour
{
    [HideInInspector] public float Damage = 0;
    public bool isFriend = false;
    TextMeshPro textMesh;
    static private readonly float BigDamage = 100;

    Vector3 direction = Vector3.zero;

    float stopwatch = 0;

    void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        textMesh = transform.Find("Text").GetComponent<TextMeshPro>();
        textMesh.text = Damage.ToString("f0");

        direction = new Vector3(1, 3, 0).normalized;

        float size = Damage / 100f;

        if (!isFriend)
        {
            textMesh.color = Color.white;

            if (Damage > 0 && Damage < BigDamage)
            {
                Color[] a =
   {
                    new Color(0, 0, 0),
                    new Color(1, 0.6078432f, 0.09803922f)
                };

                textMesh.colorGradient = new VertexGradient(a[0], a[1], a[0], a[1]);
            }
            else if (Damage >= BigDamage)
            {
                Color[] a =
                {
                    new Color(1, 1, 0),
                    new Color(1, 0, 1),
                    new Color(0, 1, 1),
                    new Color(0, 1, 0)
                };

                textMesh.colorGradient = new VertexGradient(a[0], a[1], a[2], a[3]);
            }
            else
            {
                Color[] a = //Damage is zero
                {
                    new Color(0, 0, 0),
                    new Color(0.8f, 0.8f, 0.8f)
                };

                textMesh.colorGradient = new VertexGradient(a[0], a[1], a[0], a[1]);
            }

            size *= 10;
            if (size > 1)
            {
                size = Mathf.Log(size + 2, 3);
            }
            else
            {
                size = 1 - ((1 - size) / 3);
            }
        }
        else
        {
            if (size > 1)
            {
                size = Mathf.Log(size + 2, 3);
            }
            else
            {
                size = 1 - ((1 - size) / 3);
            }
        }
        textMesh.fontSize *= size;
    }

    void Update()
    {
        stopwatch += Time.deltaTime;

        transform.position += direction * Time.deltaTime * 5;

        direction -= new Vector3(0, Time.deltaTime, 0);

        if (stopwatch > 2)
        {
            if (stopwatch > 3)
            {
                Destroy(gameObject);
            }
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1 - (stopwatch - 2));
        }
    }
}