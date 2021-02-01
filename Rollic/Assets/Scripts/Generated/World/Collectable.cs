using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    private Stage _stage;

    private bool _isCollected;

    public void Activate(Stage stage)
    {
        _stage = stage;
        _rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.Collector) && !_isCollected)
        {
            _stage.Collected();
            _isCollected = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(StringConstants.Breaker))
        {
            Destroy(gameObject, 1f);
        }
    }
}
