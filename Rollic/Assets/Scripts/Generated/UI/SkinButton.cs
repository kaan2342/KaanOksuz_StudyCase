using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [HideInInspector]
    public int index;

    public Image lockedOutline;

    [SerializeField]
    private Image _skinIcon,
        _unlockedBG;

    public bool IsUnlocked { get { return _unlockedBG.isActiveAndEnabled; } }

    public void Set(Sprite icon, int index)
    {
        this.index = index;
        _skinIcon.sprite = icon;
    }

    public void Unlock()
    {
        _unlockedBG.gameObject.SetActive(true);
        lockedOutline.gameObject.SetActive(false);
    }

    public void Equip()
    {
        _unlockedBG.sprite = ContentManager.instance.equippedSprite;
        EventBroker.InvokeChangeSkin(index);
    }

    public void UnEquip()
    {
        _unlockedBG.sprite = ContentManager.instance.unlockedSprite;
    }
}
