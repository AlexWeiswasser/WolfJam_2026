using System;
using UnityEngine;

public class LightGrowScript : MonoBehaviour
{
    [SerializeField] Vector2 scaleRange;
    [SerializeField] float scaleSpeed = 1;
    private float timePassed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float scale = ((scaleRange.y - scaleRange.x) / 2) + scaleRange.x;
        transform.localScale = new Vector2(scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        float t = 0.5f * Mathf.Sin((timePassed + Time.deltaTime) * scaleSpeed) + 0.5f;
        float scale = Mathf.Lerp(scaleRange.x, scaleRange.y, t);
        transform.localScale = new Vector2(scale, scale);
        timePassed += Time.deltaTime;
    }
}
