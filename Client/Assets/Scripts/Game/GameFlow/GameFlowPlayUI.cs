using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowPlayUI : GameFlowUIBase
{
    public Text TimeText;
    public Text ScoreText;
    public Text HPValue;
    public Slider HPSlider;
    public Slider PressSlider;
    public RectTransform ValidArea;

    public GameObject smile;
    public GameObject cry;
    public GameObject boderEffect;

    public Text thumbText;

    enum Status
    {
        Play,
        Pause,
        Win,
        Lose
    }

    public GameObject WinUI;
    public GameObject LoseUI;
    public GameObject PauseUI;
    public GameObject PlayUI;

    private Status _curStatus;

    private Coroutine _coroutine;

    void Awake()
    {
        EventManager.RegisterEvent(EventConst.EventGameOver, Lose);
        EventManager.RegisterEvent(EventConst.EventLevelSuccess, Win);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = "成绩:" + (int) DataCenter.Instance.durationTime + "秒";
        TimeText.text = ((int) DataCenter.Instance.durationTime).ToString();
        HPSlider.value = DataCenter.Instance.HP * 1.0f / DataCenter.Instance.MaxHP;
        HPValue.text = DataCenter.Instance.HP + "/" + DataCenter.Instance.MaxHP;

        PressSlider.value = DataCenter.Instance.PressSlderValue;
        RectTransform pressSliderRt = PressSlider.GetComponent<RectTransform>();
        ValidArea.anchoredPosition = Vector3.right * pressSliderRt.sizeDelta.x * DataCenter.Instance.ValidAreaValue;
        // 获取当前的 sizeDelta
        Vector2 sizeDelta = ValidArea.sizeDelta;
        // 修改宽度
        sizeDelta.x = pressSliderRt.sizeDelta.x * DataCenter.Instance.ValidAreaRange;
        // 应用新的 sizeDelta
        ValidArea.sizeDelta = sizeDelta;

        thumbText.text = DataCenter.Instance.thumbUp.ToString();
        UpdateSmile();
    }

    private void UpdateSmile()
    {
        var isSmile = DataCenter.Instance.IsSmile;
        smile.SetActive(isSmile);
        cry.SetActive(!isSmile);
        boderEffect.SetActive(!isSmile);
    }

    public void BeginLevel()
    {
        this.SetStatus(Status.Play);
    }

    private void SetStatus(Status status)
    {
        switch (status)
        {
            case Status.Play:
                Time.timeScale = 1;
                break;
            case Status.Pause:
                Time.timeScale = 0;
                break;
            case Status.Win:
                Time.timeScale = 1;
                break;
            case Status.Lose:
                Time.timeScale = 1;
                break;
            default:
                break;
        }

        this._curStatus = status;

        WinUI.SetActive(status == Status.Win);
        LoseUI.SetActive(status == Status.Lose);
        PauseUI.SetActive(status == Status.Pause);
        PlayUI.SetActive(status == Status.Play);
    }

    public void Win()
    {
        this.SetStatus(Status.Win);
        // GameFlowController.Instance.WinCurrentLevel();
    }

    public void Lose()
    {
        GameFlowController.Instance.audioMgr.PlayFail();
        this.SetStatus(Status.Lose);
        if (gameObject.activeSelf)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(DelayedEndLevel());
        }
    }

    IEnumerator DelayedEndLevel()
    {
        yield return new WaitForSecondsRealtime(1); // 延时2秒
        GameFlowController.Instance.LoseCurrentLevel();
    }

    public void OnPauseBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        this.SetStatus(Status.Pause);
    }

    public void OnGoOnBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        this.SetStatus(Status.Play);
    }

    public void OnMenuBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        GameFlowController.Instance.EnterFlow(GameFlowEnum.GameFlowMenu);
    }

    public void OnQuitBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        GameFlowController.Instance.QuitGame();
    }

    public void OnRetryBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        GameFlowController.Instance.RetryCurLevel();
    }
}