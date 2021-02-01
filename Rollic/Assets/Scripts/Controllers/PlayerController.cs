using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _forwardVelocity = 6,
        _horizontalVelocity = 3,
        _maxSidePos = 1.25f;
    [SerializeField]
    private FanController[] _fans;

    private bool _isActive;
    private Vector3 _clickPos;
    private Camera _mainCam;

    public void Init()
    {
        EventBroker.OnPlay += EventBroker_OnPlay;
        EventBroker.OnLevelFinished += EventBroker_OnLevelFinished;
        EventBroker.OnLevelReset += EventBroker_OnLevelReset;
        EventBroker.OnChangeSkin += EventBroker_OnChangeSkin;

        _mainCam = Camera.main;
    }

    #region EventHandlers

    private void EventBroker_OnPlay()
    {
        _rb.isKinematic = false;
        _isActive = true;
        transform.DORotate(new Vector3(270, 0, 270), 0.2f);
    }

    private void EventBroker_OnLevelFinished(double reward)
    {
        SetPlayer(0.5f);
    }

    private void EventBroker_OnLevelReset()
    {
        SetPlayer(0.1f);
    }

    private void EventBroker_OnChangeSkin(int index)
    {
        Material newMaterial = ContentManager.instance.skinDatas[index].Material;
        _meshRenderer.sharedMaterial = newMaterial;
    }

    #endregion EventHandlers

    private void SetPlayer(float time)
    {
        _rb.isKinematic = true;
        _isActive = false;
        Level currentLevel = LevelManager.CurrentLevel;
        transform.DOMove(currentLevel.playerStartPoint.position, time).SetDelay(0.2f);
        transform.DORotate(currentLevel.playerStartPoint.eulerAngles, time).SetDelay(0.2f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.GameState == GameState.Start && !EventSystem.current.IsPointerOverGameObject())
            {
                EventBroker.InvokePlay();
            }
            else if (GameManager.GameState == GameState.Play)
            {
                _clickPos = GetClickPos();
            }
        }
    }

    void FixedUpdate()
    {
        if (_isActive)
        {
            Vector3 newVelocity = new Vector3(0, 0, _forwardVelocity);
            if (Input.GetMouseButton(0))
            {
                Vector3 newClickPos = GetClickPos();
                if (newClickPos.x < _clickPos.x - 0.05f && transform.position.x > -_maxSidePos)
                {
                    newVelocity += new Vector3(-_horizontalVelocity, 0, 0);
                }
                else if (newClickPos.x > _clickPos.x + 0.05f && transform.position.x < _maxSidePos)
                {
                    newVelocity += new Vector3(_horizontalVelocity, 0, 0);
                }
                _clickPos = newClickPos;
            }

            _rb.velocity = newVelocity;
        }
    }

    private Vector3 GetClickPos()
    {
#if UNITY_IOS
        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = touch.position;
        touchPos.z = _mainCam.nearClipPlane;
        return _mainCam.ScreenToWorldPoint(touchPos);
#elif UNITY_ANDROID
        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = touch.position;
        touchPos.z = _mainCam.nearClipPlane;
        return _mainCam.ScreenToWorldPoint(touchPos);
#else
        Vector3 clickPos = Input.mousePosition;
        clickPos.z = _mainCam.nearClipPlane;
        return _mainCam.ScreenToWorldPoint(clickPos);
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.Barrier) && GameManager.GameState == GameState.Play)
        {
            other.enabled = false;
            _isActive = false;
            _rb.velocity = Vector3.zero;
            _rb.DOMoveZ(other.transform.position.z, 0.25f).SetEase(Ease.Linear);
            StartCoroutine(StageEndReached());
        }
        else if (other.CompareTag(StringConstants.Pickup))
        {
            other.enabled = false;
            Destroy(other.gameObject);
            for (int i = 0; i < _fans.Length; i++)
            {
                _fans[i].Activate();
            }
        }
        else if (other.CompareTag(StringConstants.Key))
        {
            other.enabled = false;
            Destroy(other.gameObject);

            if (GameManager.SaveData.KeyCount < 3)
            {
                LevelManager.CollectedKey = true;   
                GameManager.SaveData.KeyCount++;
                CurrencyModel currencyModel = new CurrencyModel()
                {
                    amount = 1,
                    currencyType = CurrencyType.Key
                };

                EventBroker.InvokeCurrencyChanged(currencyModel); 
            }
        }
    }

    private IEnumerator StageEndReached()
    {
        GameManager.GameState = GameState.Wait;

        yield return new WaitForSeconds(2.1f);
        
        if (GameManager.GameState == GameState.Wait && LevelManager.CollectedAmount < LevelManager.CurrentLevel.GetPassThreshold())
        {
            EventBroker.InvokeStageFinished(false); 
        }
    }
}