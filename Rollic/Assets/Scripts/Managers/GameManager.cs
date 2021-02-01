using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static SaveData SaveData;
    public static GameState GameState = GameState.Start;

    [SerializeField]
    private PlayerController playerController;

    private IManageable[] _manageables;

    private void Awake()
    {
        EventBroker.OnPlay += EventBroker_OnPlay;
        EventBroker.OnStageFinished += EventBroker_OnStageFinished;
        EventBroker.OnLevelFinished += EventBroker_OnLevelFinished;
        EventBroker.OnLevelReset += EventBroker_OnLevelReset;
        EventBroker.OnNextLevel += EventBroker_OnStartScreen;
        EventBroker.OnSave += EventBroker_OnSave;

        //TODO: SplashScreen
        SetManagerRefs();
        StartCoroutine(Initialize());
    }


    #region EventHandlers

    private void EventBroker_OnStageFinished(bool obj)
    {
        DataManager.SaveData(StringConstants.SaveData, SaveData);
        GameState = GameState.End;
    }

    private void EventBroker_OnPlay()
    {
        GameState = GameState.Play;
    }

    private void EventBroker_OnLevelFinished(double reward)
    {
        GameState = GameState.End;
    }
    private void EventBroker_OnLevelReset()
    {
        GameState = GameState.Start;
    }

    private void EventBroker_OnStartScreen()
    {
        GameState = GameState.Start;
    }
   
    private void EventBroker_OnSave()
    {
        DataManager.SaveData(StringConstants.SaveData, SaveData);
    }

    #endregion EventHandlers

    private IEnumerator Initialize()
    {
        LoadSaveData();

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < _manageables.Length; i++)
        {
            _manageables[i].Init();
        }

        yield return new WaitForSeconds(0.2f);

        playerController.Init();
        for (int i = 0; i < _manageables.Length; i++)
        {
            _manageables[i].Set();
        }
    }

    private void SetManagerRefs()
    {
        _manageables = new IManageable[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _manageables[i] = transform.GetChild(i).GetComponent<IManageable>();
        }
    }

    private void LoadSaveData()
    {
        var savedData = DataManager.LoadData<SaveData>(StringConstants.SaveData);

        if (savedData == null)
        {
            //new User
            List<int> defaultSkin = new List<int>();
            defaultSkin.Add(0);

            SaveData = new SaveData()
            {
                IsAudioOn = true,
                IsVibrationOn = true,
                Level = 1,
                EquippedSkinId = 0,
                SoftCurrency = 100,
                KeyCount = 0,
                UnlockedSkinIds = defaultSkin
            };

            DataManager.SaveData(StringConstants.SaveData, SaveData);
        }
        else
        {
            SaveData = savedData;
        }
    }
}