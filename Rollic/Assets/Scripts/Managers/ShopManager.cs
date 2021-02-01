using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IManageable
{
    [SerializeField]
    private Button _unlockRandomBtn;
    
    [SerializeField]
    private SkinButton _skinButtonPrefab;

    [SerializeField]
    private Transform _container;

    [SerializeField]
    private TextMeshProUGUI _costText;

    private int _equippedSkinId;
    private List<SkinButton> _skinButtons;

    public void Init()
    {
        EventBroker.OnChangeSkin += EventBroker_OnChangeSkin;

        _equippedSkinId = GameManager.SaveData.EquippedSkinId;
        _skinButtons = new List<SkinButton>();
        SkinData[] skinDatas = ContentManager.instance.skinDatas;
        for (int i = 0; i < skinDatas.Length; i++)
        {
            SkinButton skinButton = Instantiate(_skinButtonPrefab, _container);
            skinButton.Set(skinDatas[i].ShopSprite, i);
            _skinButtons.Add(skinButton);
        }
    }

    public void Set()
    {
        _costText.text = Price().ToString();

        List<int> unlockedSkinIds = GameManager.SaveData.UnlockedSkinIds;
        for (int i = 0; i < unlockedSkinIds.Count; i++)
        {
            _skinButtons[unlockedSkinIds[i]].Unlock();
        }

        _skinButtons[_equippedSkinId].Equip();
    }

    private void EventBroker_OnChangeSkin(int index)
    {
        if (index != GameManager.SaveData.EquippedSkinId)
        {
            GameManager.SaveData.EquippedSkinId = index;
            _skinButtons[_equippedSkinId].UnEquip();
            _equippedSkinId = index;
        }
    }

    public void OnClickBuyRandom()
    {
        if (GameManager.SaveData.SoftCurrency >= Price())
        {
            _unlockRandomBtn.interactable = false;
            StartCoroutine(RandomAnim());
        }
    }

    private static int Price()
    {
        return GameManager.SaveData.UnlockedSkinIds.Count * 50;
    }

    private IEnumerator RandomAnim()
    {
        int i = 0;
        int randomNum = 0;
        while (i < 7)
        {
            i++;

            randomNum = UnityEngine.Random.Range(0, _skinButtons.Count);
            SkinButton randSkinButton = _skinButtons[randomNum];
            randSkinButton.lockedOutline.enabled = true;

            yield return new WaitForSeconds(0.1f);

            randSkinButton.lockedOutline.enabled = false;
        }

        UnlockSkin(randomNum);
    }

    private void UnlockSkin(int index)
    {
        if (IsAllBought())
        {
            return;
        }

        if (!IsSkinBought(index))
        {
            int price = Price();
            GameManager.SaveData.SoftCurrency -= price;

            CurrencyModel currencyModel = new CurrencyModel()
            {
                amount = -price,
                currencyType = CurrencyType.Soft
            };

            EventBroker.InvokeCurrencyChanged(currencyModel);

            GameManager.SaveData.UnlockedSkinIds.Add(index);
            _skinButtons[index].Unlock();
            _skinButtons[index].Equip();
            _costText.text = Price().ToString();
            _unlockRandomBtn.interactable = true;
        }
        else
        {
            int randNum = UnityEngine.Random.Range(0, _skinButtons.Count);
            UnlockSkin(randNum);
        }
    }

    private bool IsAllBought()
    {
        return _skinButtons.Count == GameManager.SaveData.UnlockedSkinIds.Count;
    }

    private bool IsSkinBought(int index)
    {
        var unlockedSkins = GameManager.SaveData.UnlockedSkinIds;
        
        return unlockedSkins.Contains(index);
    }
}