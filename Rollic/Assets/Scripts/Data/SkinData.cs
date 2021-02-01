using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Data", menuName = "Create Skin Data")]
public class SkinData : ScriptableObject
{
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Sprite _shopSprite;

    public Material Material
    {
        get
        {
            return _material; ;
        }
    }

    public Sprite ShopSprite
    {
        get
        {
            return _shopSprite; ;
        }
    }
}
