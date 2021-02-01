using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableContainer : MonoBehaviour
{
    public int passThreshold;

    [SerializeField]
    private Collectable[] _collectables;

    public void ActivateCollectables(Stage stage)
    {
        for (int i = 0; i < _collectables.Length; i++)
        {
            if (_collectables[i] != null)
            {
                _collectables[i].Activate(stage); 
            }
        }
    }

    public void DeactivateCollectables()
    {
        for (int i = 0; i < _collectables.Length; i++)
        {
            if (_collectables[i])
            {
                Destroy(_collectables[i].gameObject); 
            }
        }
    }
}
