using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker
{
    public static event Action OnPlay;
    public static void InvokePlay()
    {
        OnPlay?.Invoke();
    }

    public static event Action<bool> OnStageFinished;
    public static void InvokeStageFinished(bool isSuccess)
    {
        OnStageFinished?.Invoke(isSuccess);
    }
    
    public static event Action OnLevelReset;
    public static void InvokeLevelReset()
    {
        OnLevelReset?.Invoke();
    }

    public static event Action<double> OnLevelFinished;
    public static void InvokeLevelFinished(double reward)
    {
        OnLevelFinished?.Invoke(reward);
        OnSave?.Invoke();
    }

    public static event Action OnNextLevel;
    public static void InvokeNextLevel()
    {
        OnNextLevel?.Invoke();
    }

    public static event Action<CurrencyModel> OnCurrencyChanged;
    public static void InvokeCurrencyChanged(CurrencyModel currencyModel)
    {
        OnCurrencyChanged?.Invoke(currencyModel);
        OnSave?.Invoke();
    }

    public static event Action OnSave;
    public static void InvokeSave()
    {
        OnSave?.Invoke();
    }

    public static event Action<int> OnChangeSkin;
    public static void InvokeChangeSkin(int index)
    {
        OnChangeSkin?.Invoke(index);
        OnSave?.Invoke();
    }
}
