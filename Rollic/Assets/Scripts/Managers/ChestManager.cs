using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour, IManageable
{
    [SerializeField]
    private GameObject _chestPanel;
    [SerializeField]
    private Button _continueBtn;
    [SerializeField]
    private Image[] _keyImages;
    [SerializeField]
    private Sprite _keyEmpty,
        _keyCollected;
    [SerializeField]
    private ChestButton[] chestButtons;

    public static int TotalReward;

    public void Init()
    {
        EventBroker.OnCurrencyChanged += EventBroker_OnCurrencyChanged;
        EventBroker.OnNextLevel += EventBroker_OnNextLevel;
    }

    public void Set()
    {
        for (int i = 0; i < GameManager.SaveData.KeyCount; i++)
        {
            _keyImages[i].sprite = _keyCollected;
        }
        EventBroker_OnNextLevel();
    }

    private void EventBroker_OnCurrencyChanged(CurrencyModel currencyModel)
    {
        if (currencyModel.currencyType == CurrencyType.Key)
        {
            int keyCount = GameManager.SaveData.KeyCount;
            for (int i = 0; i < _keyImages.Length; i++)
            {
                if (i < keyCount)
                {
                    _keyImages[i].sprite = _keyCollected;
                }
                else
                {
                    _keyImages[i].sprite = _keyEmpty;
                }
            }
            if (currencyModel.amount < 0 && GameManager.SaveData.KeyCount == 0)
            {
                _continueBtn.interactable = true;
            }
        }
    }

    private void EventBroker_OnNextLevel()
    {
        if (GameManager.SaveData.KeyCount == 3)
        {
            for (int i = 0; i < chestButtons.Length; i++)
            {
                chestButtons[i].SetReward();
            }
            _chestPanel.SetActive(true);
        }
    }

    public void OnClickContinueBtn()
    {
        GameManager.SaveData.SoftCurrency += TotalReward;

        CurrencyModel currencyModel = new CurrencyModel()
        {
            amount = TotalReward,
            currencyType = CurrencyType.Soft
        };
        EventBroker.InvokeCurrencyChanged(currencyModel);
        TotalReward = 0;
    }
}
