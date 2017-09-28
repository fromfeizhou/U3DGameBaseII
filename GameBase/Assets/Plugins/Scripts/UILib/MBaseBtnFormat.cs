﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
public enum MButtonState
{
    NORMAL,
    SELECTED,
    ENABLE
}

public class MBaseBtnFormat : MonoBehaviour
{

    [HideInInspector]
    public string btnType = "1";
    [HideInInspector]
    public string mBtnSize = "1";
    private bool _onDown = false;
    private Sprite _comSp = null;
    private Sprite _selSp = null;
    private MButtonState _btnState = MButtonState.NORMAL;
    //动画类型
    private bool _isAction = false;
    public int actionType = 1;

    public bool isSelectedBtn = false;
    private bool _selected = false;

    public virtual void Start()
    {
        //AssetManager.LoadAsset(GetBtnComResPath(), new UnityAction<Object, string>(ComCallBack), typeof(Sprite));
        _comSp = GetComponent<Image>().sprite;
        AssetManager.LoadAsset(GetBtnSelResPath(), new UnityAction<Object, string>(SelCallBack), typeof(Sprite));

        EventTriggerListener.Get(gameObject).onDown = OnDown;
        EventTriggerListener.Get(gameObject).onUp = OnUp;
        EventTriggerListener.Get(gameObject).onExit = OnExit;
    }

    //设置按钮选中状态
    public bool Selected
    {
        get { return _selected; }
        set
        {

            if (value != _selected)
            {
                _selected = value;
                updateSelectedState();
            }
        }
    }

    //按钮按下回调
    private void OnDown(GameObject go)
    {
        if (_isAction) return;

        //在这里监听按钮的点击事件
        if (!_onDown)
        {
            _onDown = true;
            transform.GetComponent<RectTransform>().localScale = new Vector3(1.05f, 1.05f, 1.0f);
            ChangeBtnState(MButtonState.SELECTED);
        }
    }

    // 当按钮抬起的时候自动调用此方法  
    private void OnUp(GameObject go)
    {
        if (_onDown)
        {
            resetBtnState();
            OnClick();
        }
    }

    // 当按钮失去焦点
    private void OnExit(GameObject go)
    {
        if (_onDown)
        {
            resetBtnState();
            if (actionType == 1)
            {
                doExitAction();
            }
        }
    }

    private void doExitAction()
    {
        _isAction = true;
        Sequence sequence = DOTween.Sequence();
        Tween tScale1 = transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.1f);
        Tween tScale2 = transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.1f);
        Tween tScale3 = transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.1f);
        Tween tScale4 = transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        sequence.Append(tScale1);
        sequence.Append(tScale2);
        sequence.Append(tScale3);
        sequence.Append(tScale4);
        sequence.onComplete = delegate() { _isAction = false; };
    }

    // 当按钮点击回调
    private void OnClick()
    {
        Debug.Log("OnClick");
        Selected = !Selected;
    }

    //重置按钮状态
    private void resetBtnState()
    {
        //带缓动按钮 不需要恢复缩放
        if (actionType != 1)
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        //选中按钮特殊处理
        if (isSelectedBtn)
        {
            updateSelectedState();
        }
        else
        {
            ChangeBtnState(MButtonState.NORMAL);
        }
        _onDown = false;
    }

    //选中按钮状态刷新
    private void updateSelectedState()
    {
        if (!isSelectedBtn) return;
        if (_selected)
        {
            ChangeBtnState(MButtonState.SELECTED);
        }
        else
        {
            ChangeBtnState(MButtonState.NORMAL);
        }
    }

    //返回按钮资源 普通状态
    public string GetBtnComResPath()
    {
        string path = System.IO.Path.Combine(PathManager.GetResLibPath("ImgBtnSpritePath"), "btnStyle_" + btnType + ".png");
        return path;
    }

    //返回按钮资源 选中状态
    public string GetBtnSelResPath()
    {
        string path = System.IO.Path.Combine(PathManager.GetResLibPath("ImgBtnSpritePath"), "btnStyle_" + btnType + "s.png");
        return path;
    }


    //按钮状态切换
    private void ChangeBtnState(MButtonState state)
    {
        if (state != _btnState)
        {
            _btnState = state;
            updateBtnState();
        }
    }

    //按钮common资源加载回调
    private void ComCallBack(Object target, string path)
    {
        gameObject.GetComponent<Image>().sprite = target as Sprite;
        _comSp = target as Sprite;
    }

    //按钮common资源加载回调
    private void SelCallBack(Object target, string path)
    {
        _selSp = target as Sprite;
    }

    //更新状态
    private void updateBtnState()
    {
        switch (_btnState)
        {
            case MButtonState.NORMAL:
                if (null != _selSp)
                {
                    GetComponent<Image>().sprite = _comSp;
                }
                break;
            case MButtonState.SELECTED:
                if (null != _selSp)
                {
                    GetComponent<Image>().sprite = _selSp;
                }
                break;
            case MButtonState.ENABLE:
                break;
        }

    }
}
