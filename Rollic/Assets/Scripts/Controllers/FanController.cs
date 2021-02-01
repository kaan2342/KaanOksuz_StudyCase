using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _direction, _scale;
    [SerializeField]
    private float _speed = 10f;

    private bool _isActive = false;
    private float _timer = 12f;
    
    void Update()
    {
        if (_isActive)
        {
            transform.Rotate(_direction * _speed * Time.deltaTime);
            
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                Deactivate();
            } 
        }
    }

    public void Activate()
    {
        _timer = 4.2f;
        transform.DOScale(_scale, 0.2f);
        _isActive = true;
    }

    private void Deactivate()
    {
        _isActive = false;
        transform.DOScale(Vector3.zero, 0.2f);
    }
}