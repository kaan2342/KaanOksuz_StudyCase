using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private Stage[] _stages;

    public Stage[] Stages
    {
        get
        {
            return _stages; ;
        }
    }
}