using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    [SerializeField] float offset = 1.5f;
    [HideInInspector] public float percentage = 1;

    Vector2 defaultScale;
    Vector2 defaultPosition;
    Vector2 defaultSize;
    Transform pixel;
    Transform pixel1;

    Transform player;

    private void Start()
    {
        player = transform.parent.Find("Char");
        pixel = transform.Find("pixel");
        pixel1 = transform.Find("pixel (1)");

        defaultScale = pixel.localScale;
        defaultSize = new Vector3(pixel.localScale.x * transform.localScale.x, pixel.localScale.y * transform.localScale.y, 1);
        defaultPosition = pixel.localPosition;
    }

    void Update()
    {
        transform.position = player.position + new Vector3(0, offset, 0);

        pixel.localScale = new Vector3(percentage * defaultScale.x, defaultScale.y, 1);
        pixel.position = new Vector3(defaultPosition.x - ((1 - percentage) * defaultSize.x) / 2, defaultPosition.y, 0) +
            player.position + new Vector3(0, offset, 0);
        pixel1.position = player.position + new Vector3(0, offset, 0);
    }
}
