using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Laser : MonoBehaviour
{
    [SerializeField] private bool _timedActivation = false;
    [SerializeField] private float _minLengthInactive = 2;
    [SerializeField] private float _maxLengtheInactive = 3;
    [SerializeField] private float _minLengthActive = 2;
    [SerializeField] private float _maxLengtheActive = 3;
    
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private Color _color1;
    private Color _color2;
    
    float _lengthTimer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _color1 = new Color(Random.Range(0.5f, 1.2f), Random.Range(0.5f, 1.2f), Random.Range(0.5f, 1.2f));
        _color2 = new Color(Random.Range(0.5f, 1.2f), Random.Range(0.5f, 1.2f), Random.Range(0.5f, 1.2f));
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = _color1;
        
        _collider = GetComponent<Collider2D>();

        if (_collider.enabled)
        {
            _lengthTimer = Random.Range(_minLengthActive, _maxLengtheActive);
        }
        else
        {
            _lengthTimer = Random.Range(_minLengthInactive, _maxLengtheInactive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f; // Oscillates between 0 and 1
        _renderer.color = Color.Lerp(_color1, _color2, t);
        
        _lengthTimer -= Time.deltaTime;
        
        if(_lengthTimer <= 0f && _timedActivation)
        {
            if (_collider.enabled)
            {
                _collider.enabled = false;
                _renderer.enabled = false;
                _lengthTimer = Random.Range(_minLengthInactive, _maxLengtheInactive);
            }
            else
            {
                _collider.enabled = true;
                _renderer.enabled = true;
                _lengthTimer = Random.Range(_minLengthActive, _maxLengtheActive);
            }
        }
    }
}
