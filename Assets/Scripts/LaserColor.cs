using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserColor : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Color _color1;
    private Color _color2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _color1 = new Color(Random.value, Random.value, Random.value);
        _color2 = new Color(Random.value, Random.value, Random.value);
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = _color1;
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f; // Oscillates between 0 and 1
        _renderer.color = Color.Lerp(_color1, _color2, t);
    }
}
