using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentManager : MonoBehaviour,IManageable
{
    public static ContentManager instance;

    public Sprite unlockedSprite,
        equippedSprite;

    public LevelData[] levelDatas;
    public SkinData[] skinDatas;
    
    public void Init()
    {
        instance = this;
    }

    public void Set()
    {
        
    }
}
