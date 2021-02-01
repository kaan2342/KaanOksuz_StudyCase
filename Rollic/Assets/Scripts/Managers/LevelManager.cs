using UnityEngine;

public class LevelManager : MonoBehaviour, IManageable
{
    public static bool CollectedKey;
    public static int CollectedAmount, Total = 0;
    public static float ZPosition;
    public static Level CurrentLevel, NextLevel;

    [SerializeField]
    private Transform _levelContainer;


    public void Init()
    {
        EventBroker.OnStageFinished += EventBroker_OnStageFinished;
        EventBroker.OnLevelFinished += EventBroker_OnLevelFinished;
        EventBroker.OnLevelReset += EventBroker_OnLevelReset;
        EventBroker.OnPlay += EventBroker_OnPlay;
        EventBroker.OnNextLevel += EventBroker_OnNextLevel;

        ZPosition = 0;
    }


    public void Set()
    {
        int currentLevel = GameManager.SaveData.Level;
        LevelData currentLevelData = GetLevelData(currentLevel);
        LevelData nextLevelData = GetLevelData(currentLevel + 1);

        CurrentLevel = SpawnLevel(currentLevelData);
        NextLevel = SpawnLevel(nextLevelData);
    }

    #region EventHandlers

    private void EventBroker_OnStageFinished(bool isSuccess)
    {
        Total += CollectedAmount;
        CollectedAmount = 0;
        //Stage Completed
        if (isSuccess)
        {
            CurrentLevel.StageFinished(); 
        }
    }

    private void EventBroker_OnLevelFinished(double reward)
    {
        CollectedKey = false;
        CollectedAmount = 0;
        CurrentLevel = NextLevel;
        int currentLevel = ++GameManager.SaveData.Level;
        LevelData nextLevelData = GetLevelData(currentLevel + 1);
        NextLevel = SpawnLevel(nextLevelData);
    }

    private void EventBroker_OnLevelReset()
    {
        Debug.Log(CollectedKey);
        if (CollectedKey)
        {
            GameManager.SaveData.KeyCount--;
            CurrencyModel currencyModel = new CurrencyModel()
            {
                amount = -1,
                currencyType = CurrencyType.Key
            };
            EventBroker.InvokeCurrencyChanged(currencyModel);
            CollectedKey = false;
        }
        ZPosition -= CurrentLevel.LevelLength + NextLevel.LevelLength;
        CurrentLevel.ResetLevel();
        NextLevel.ResetLevel();
    }
    
    private void EventBroker_OnPlay()
    {
        Total = 0;
        CurrentLevel.ActivateStage();
    }
    
    private void EventBroker_OnNextLevel()
    {
        CurrencyModel currencyModel = new CurrencyModel
        {
            amount = Total,
            currencyType = CurrencyType.Soft
        };
        GameManager.SaveData.SoftCurrency += Total;
        EventBroker.InvokeCurrencyChanged(currencyModel);
    }

    #endregion EventHandlers

    public Level SpawnLevel(LevelData levelData)
    {
        Level newLevel = Instantiate(new GameObject(), _levelContainer).AddComponent<Level>();
        newLevel.name = levelData.name;
        newLevel.Set(levelData);
        return newLevel;
    }

    public LevelData GetLevelData(int level)
    {
        int randomizer = level > 10 ? Random.Range(0, 10) : 0;

        return ContentManager.instance.levelDatas[((level - 1) % 10) + randomizer];
    }
}