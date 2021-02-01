using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour, IManageable
{
    [Header("Start Panel")]
    [SerializeField]
    private GameObject _startPanel;
    [SerializeField]
    private TextMeshProUGUI _shopSoftCurText;

    [Header("In Game Panel")]
    [SerializeField]
    private GameObject _inGamePanel;

    [SerializeField]
    private Sprite 
        _stageEmpty,
        _stageComplete,
        _keyEmpty,
        _keyCollected;

    [SerializeField]
    private TextMeshProUGUI 
        _softCurText, 
        _dragToStartText,
        _currentLevelText,
        _nextLevelText;

    [SerializeField]
    private Image[] _keyImages,
        _stageIndicators;

    private int _stage;

    [Header("End Panel")]
    [SerializeField]
    private GameObject _endPanel;

    [SerializeField]
    private GameObject _winPanel, _losePanel;

    [SerializeField]
    private TextMeshProUGUI _earnedSoftCurText;

    public void Init()
    {
        EventBroker.OnPlay += EventBroker_OnPlay;
        EventBroker.OnStageFinished += EventBroker_OnStageFinished;
        EventBroker.OnLevelFinished += EventBroker_OnLevelFinished;
        EventBroker.OnCurrencyChanged += EventBroker_OnCurrencyChanged;

        _stage = 0;
        SetInGamePanelValues();
    }

    public void Set()
    {
        _dragToStartText.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        UpdateKeys();
        ResetIndicators();
    }

    private void UpdateKeys()
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
    }

    private void ResetIndicators()
    {
        for (int i = 0; i < _stageIndicators.Length; i++)
        {
            _stageIndicators[i].sprite = _stageEmpty;
        }
    }

    private void SetInGamePanelValues()
    {
        SaveData saveData = GameManager.SaveData;
        string softCurr = saveData.SoftCurrency.ToString("N0");
        _softCurText.text = softCurr;
        _shopSoftCurText.text = softCurr;
        int level = saveData.Level;
        _currentLevelText.text = level.ToString();
        _nextLevelText.text = (level + 1).ToString();
    }

    #region EventHandlers

    private void EventBroker_OnPlay()
    {
        DOTween.Kill(_dragToStartText.transform);
        _startPanel.SetActive(false);
        _inGamePanel.SetActive(true);
        _dragToStartText.transform.localScale = Vector3.one;
    }

    private void EventBroker_OnStageFinished(bool isSuccess)
    {
        if (isSuccess)
        {
            _stageIndicators[_stage].sprite = _stageComplete;
            _stage++;
        }
        else
        {
            _inGamePanel.SetActive(false);
            _endPanel.SetActive(true);
            _losePanel.SetActive(true);
        }
    }

    private void EventBroker_OnLevelFinished(double reward)
    {
        _earnedSoftCurText.text = reward.ToString("N0");
        _inGamePanel.SetActive(false);
        _endPanel.SetActive(true);
        _winPanel.SetActive(true);
    }

    private void EventBroker_OnCurrencyChanged(CurrencyModel currencyModel)
    {
        switch (currencyModel.currencyType)
        {
            case CurrencyType.Soft:
                string softCurr = GameManager.SaveData.SoftCurrency.ToString("N0");
                _softCurText.text = softCurr;
                _shopSoftCurText.text = softCurr;
                break;
            case CurrencyType.Key:
                UpdateKeys();
                break;
            default:
                break;
        }
    }

    #endregion

    #region ButtonHandlers

    public void OnResetButtonClicked()
    {
        _stage = 0;
        SetInGamePanelValues();
        ResetIndicators();

        _endPanel.SetActive(false);
        _losePanel.SetActive(false);
        _startPanel.SetActive(true);

        EventBroker.InvokeLevelReset();
    }
    
    public void OnContinueButtonClicked()
    {
        _stage = 0;
        SetInGamePanelValues();
        ResetIndicators();

        _endPanel.SetActive(false);
        _winPanel.SetActive(false);
        _startPanel.SetActive(true);

        EventBroker.InvokeNextLevel();
    }

    #endregion ButtonHandlers

}