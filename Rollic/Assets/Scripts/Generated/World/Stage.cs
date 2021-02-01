using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform playerStartPoint;
    
    [SerializeField]
    private GameObject _collectablesPrefab;

    [SerializeField]
    private Transform _sideWall,
        _collectableContainer,
        _leftGate,
        _rightGate,
        _platform;

    [SerializeField]
    private TextMeshProUGUI _amountText;

    private CollectableContainer _collectables;
    private bool _stageEndInitiated;

    public int PassThreshold
    { 
        get 
        {
            return _collectables.passThreshold;
        } 
    }
    
    public float StageLength 
    { 
        get 
        {
            return _sideWall.lossyScale.z + _leftGate.lossyScale.z;
        } 
    }

    public void Set()
    {
        StopCoroutine(StageEndCoroutine());
        _collectables = Instantiate(_collectablesPrefab, _collectableContainer).GetComponent<CollectableContainer>();
        _amountText.text =  "0 / " + PassThreshold;
    }

    public void Activate()
    {
        _collectables.ActivateCollectables(this);
    }
    
    public void Deactivate()
    {
        Destroy(_collectables);
    }

    public void Collected()
    {
        if (!_stageEndInitiated)
        {
            _stageEndInitiated = true;
            StartCoroutine(StageEndCoroutine());
        }
        LevelManager.CollectedAmount++;

        _amountText.text = LevelManager.CollectedAmount + " / " + PassThreshold;
    }

    private IEnumerator StageEndCoroutine()
    {
        yield return new WaitForSeconds(2f);

        bool isSuccess = LevelManager.CollectedAmount >= PassThreshold ? true : false;
        if (isSuccess)
        {
            _platform.DOMoveY(0, 0.5f);
            _leftGate.DOLocalRotate(new Vector3(180, -90,-90), 0.5f);
            _rightGate.DOLocalRotate(new Vector3(180, -90,-90), 0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        _collectables.DeactivateCollectables();
        EventBroker.InvokeStageFinished(isSuccess);
    }
}