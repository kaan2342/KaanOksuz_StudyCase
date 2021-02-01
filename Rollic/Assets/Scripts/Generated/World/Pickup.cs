using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y + 0.5f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);   
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 80 * Time.deltaTime);
    }
}
