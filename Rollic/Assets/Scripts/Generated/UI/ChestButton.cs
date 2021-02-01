using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _lockedPanel, _openedPanel;

    [SerializeField]
    private TextMeshProUGUI _earnedRewardText;

    private int _reward;

    public void SetReward()
    {
        _openedPanel.SetActive(false);
        _lockedPanel.SetActive(true);
        _reward = UnityEngine.Random.Range(0, 501);
        _reward = (_reward - _reward % 50) + 50;
        _earnedRewardText.text = "+ " + _reward.ToString("N0");
    }

    public void OnClick()
    {
        if (GameManager.SaveData.KeyCount > 0)
        {
            GameManager.SaveData.KeyCount--;
            _openedPanel.SetActive(true);
            _lockedPanel.SetActive(false);


            ChestManager.TotalReward += _reward;
            CurrencyModel currencyModel = new CurrencyModel()
            {
                amount = -_reward,
                currencyType = CurrencyType.Key
            };
            EventBroker.InvokeCurrencyChanged(currencyModel);
        }
    }
}
