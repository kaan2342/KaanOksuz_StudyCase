using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform playerStartPoint;

    private int _currentStageId = 0,
        _collectedAmount;
    private LevelData _levelData;
    private List<Stage> _spawnedStages;

    public float LevelLength 
    {
        get 
        {
            float total = 0;
            for (int i = 0; i < _spawnedStages.Count; i++)
            {
                total += _spawnedStages[i].StageLength;
            }
            return total;
        }
    }

    public void Set(LevelData levelData)
    {
        Stage[] stages = levelData.Stages;
        _spawnedStages = new List<Stage>();
        _levelData = levelData;

        for (int i = 0; i < stages.Length; i++)
        {
            Stage stage = Instantiate(stages[i].gameObject, new Vector3(0, 0, LevelManager.ZPosition), Quaternion.identity, transform).GetComponent<Stage>();
            LevelManager.ZPosition += stage.StageLength;
            stage.Set();
            _spawnedStages.Add(stage);
        }
        playerStartPoint = _spawnedStages[_currentStageId].playerStartPoint;
    }

    public void StageFinished()
    {
        _spawnedStages[_currentStageId].Deactivate();
        _currentStageId++;
        //Stage Finished
        if (_currentStageId < _spawnedStages.Count)
        {
            _spawnedStages[_currentStageId].Activate();
            EventBroker.InvokePlay();
        }
        //Level Finished
        else
        {
            EventBroker.InvokeLevelFinished(LevelManager.Total);
            Destroy(gameObject, 1.5f);
        }
    }

    public void ActivateStage()
    {
        _spawnedStages[_currentStageId].Activate();
    }

    public void ResetLevel()
    {
        _currentStageId = 0;
        for (int i = 0; i < _spawnedStages.Count; i++)
        {
            Destroy(_spawnedStages[i].gameObject);
        }
        Set(_levelData);
    }

    public int GetPassThreshold()
    {
        return _spawnedStages[_currentStageId].PassThreshold;
    }
}
