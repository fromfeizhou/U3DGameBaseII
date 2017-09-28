using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class MBaseBtnFormat : MonoBehaviour
{

    [HideInInspector]
    public string btnType = "1";
    [HideInInspector]
    public string mBtnSize = "1";
    private bool onDown = false;
    private Sprite comSp = null;
    private Sprite selSp = null;
    //动画类型
    private bool isAction = false;

    public int actionType = 1;


    public virtual void Start()
    {
        AssetManager.LoadAsset(GetBtnComResPath(), new UnityAction<Object, string>(ComCallBack), typeof(Sprite));
        AssetManager.LoadAsset(GetBtnSelResPath(), new UnityAction<Object, string>(SelCallBack), typeof(Sprite));

        EventTriggerListener.Get(gameObject).onDown = OnDown;
        EventTriggerListener.Get(gameObject).onUp = OnUp;
        EventTriggerListener.Get(gameObject).onExit = OnExit;
    }

    
    //按钮按下回调
    private void OnDown(GameObject go)
    {
        if (isAction) return;

        //在这里监听按钮的点击事件
        if (!onDown)
        {
            onDown = true;
            transform.GetComponent<RectTransform>().localScale = new Vector3(1.05f, 1.05f, 1.0f);
            if (null != selSp)
            {
                GetComponent<Image>().sprite = selSp;
            }
        }
    }

    // 当按钮抬起的时候自动调用此方法  
    private void OnUp(GameObject go)
    {
        if (onDown)
        {
            resetBtnState();
            OnClick();
        }
    }

    // 当按钮失去焦点
    private void OnExit(GameObject go)
    {
        if (onDown)
        {
            if (actionType == 1)
            {
                resetBtnState(false);
                doExitAction();
            }
            else
            {
                resetBtnState();
            }
        }
    }

    private void doExitAction(){
         isAction = true;
        Sequence sequence = DOTween.Sequence();
        Tween tScale1 = transform.DOScale(new Vector3(0.9f,0.9f,1f),0.1f);
        Tween tScale2 = transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.1f);
        Tween tScale3 = transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.1f);
        Tween tScale4 = transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        sequence.Append(tScale1);
        sequence.Append(tScale2);
        sequence.Append(tScale3);
        sequence.Append(tScale4);
        sequence.onComplete = delegate() { isAction = false; };
    }

    // 当按钮点击回调
    private void OnClick()
    {
        Debug.Log("OnClick");
    }

    //重置按钮状态
    private void resetBtnState(bool resetScale = true)
    {
        if (resetScale)
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (null != comSp)
        {
            GetComponent<Image>().sprite = comSp;
        }
        onDown = false;
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

    //按钮common资源加载回调
    private void ComCallBack(Object target, string path)
    {
        gameObject.GetComponent<Image>().sprite = target as Sprite;
        comSp = target as Sprite;
    }

    //按钮common资源加载回调
    private void SelCallBack(Object target, string path)
    {
        selSp = target as Sprite;
    }
}
