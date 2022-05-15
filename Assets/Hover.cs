using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    private Vector3 startMarker;
    private Vector3 endMarker;
    public bool up;
    public float hoverSpeed = 5f;
    private float startTime;
    private float journeyLength;

    private void Start()
    {
        startMarker = transform.position;
        endMarker = transform.position + new Vector3(0, .05f, 0);

        startTime = Time.time;
    }
    void Update()
    {
        float currTime = Time.time - startTime;
        float frac = currTime / hoverSpeed;

        if (up)
        {
            transform.position = Vector3.Lerp(startMarker, endMarker, frac);
        } else
        {
            transform.position = Vector3.Lerp(endMarker, startMarker, frac);
        }

        if (frac >= 1)
        {
            startTime = Time.time;
            up = !up;
        }
        // transform.position = Vector3.Lerp(startMarker, endMarker, )
    }
}
