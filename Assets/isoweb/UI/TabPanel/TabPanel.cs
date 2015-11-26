using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TabPanel : MonoBehaviour
{
    public enum ScreenSide
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM
    }

    [SerializeField]
    private RectTransform _tabTransform;

    [SerializeField]
    private RectTransform _tabButtonTransform;

    [SerializeField]
    private RectTransform _contentTransform;


    [SerializeField] private ScreenSide _screenSide;

    private bool _collapsed;
    private ScreenSide _lastScreenSide;

    protected RectTransform _rect
    {
        get { return (RectTransform) transform; }
    }

    void Start()
    {
        _collapsed = true;
        UpdateLayout(true);
    }

    private void UpdateLayout(bool force=false)
    {
        if (!force && _lastScreenSide == _screenSide)
            return;

        switch (_screenSide)
        {
            case ScreenSide.LEFT:
                _rect.anchorMin = _rect.anchorMax = _rect.pivot = new Vector2(0f, 0.5f);
                _contentTransform.anchorMin = _contentTransform.anchorMax = _contentTransform.pivot = new Vector2(1f, 0.5f);
                _tabTransform.anchorMin = _tabTransform.anchorMax = _tabTransform.pivot = new Vector2(0f, 0f);
                _tabTransform.anchoredPosition = new Vector2(12f, _contentTransform.sizeDelta.y/4f); // half of half
                _tabButtonTransform.rotation = Quaternion.Euler(0, 0, -90f);
                break;
            case ScreenSide.RIGHT:
                _rect.anchorMin = _rect.anchorMax = _rect.pivot = new Vector2(1f, 0.5f);
                _contentTransform.anchorMin = _contentTransform.anchorMax = _contentTransform.pivot = new Vector2(0f, 0.5f);
                _tabTransform.anchorMin = _tabTransform.anchorMax = _tabTransform.pivot = new Vector2(1f, 0f);
                _tabTransform.anchoredPosition = new Vector2(12f, _contentTransform.sizeDelta.y / -4f); // half of half
                _tabButtonTransform.rotation = Quaternion.Euler(0, 0, 90f);
                break;
            case ScreenSide.TOP:
                _rect.anchorMin = _rect.anchorMax = _rect.pivot = new Vector2(0.5f, 1f);
                _contentTransform.anchorMin = _contentTransform.anchorMax = _contentTransform.pivot = new Vector2(0.5f, 0f);
                _tabTransform.anchorMin = _tabTransform.anchorMax = _tabTransform.pivot = new Vector2(0.5f, 1f);
                _tabTransform.anchoredPosition = new Vector2(_tabTransform.anchoredPosition.x / 2f, -12f);
                _tabButtonTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ScreenSide.BOTTOM:
                _rect.anchorMin = _rect.anchorMax = _rect.pivot = new Vector2(0.5f, 0f);
                _contentTransform.anchorMin = _contentTransform.anchorMax = _contentTransform.pivot = new Vector2(0.5f, 1f);
                _tabTransform.anchorMin = _tabTransform.anchorMax = _tabTransform.pivot = new Vector2(0.5f, 0f);
                _tabTransform.anchoredPosition = new Vector2(_tabTransform.anchoredPosition.x / 2f, 12f);
                _tabButtonTransform.rotation = Quaternion.Euler(0, 0, 0);

                break;
        }

        _lastScreenSide = _screenSide;
        _rect.anchoredPosition = Vector2.zero;

        if (_collapsed)
            CollapseTab(true);
        else
            ExpandTab(true);
    }

    void Update()
    {
        UpdateLayout();
    }

    void CollapseTab(bool instant=false)
    {
        var dur = 0.5f;

        if (instant)
            dur = 0f;
            
        switch (_screenSide)
        {
            case ScreenSide.LEFT:
                _rect.DOMoveX(0, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.RIGHT:
                _rect.DOMoveX(Screen.width, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.TOP:
                _rect.DOMoveY(Screen.height, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.BOTTOM:
                _rect.DOMoveY(0, dur).SetEase(Ease.OutQuint);
                break;
        }
        _collapsed = true;
    }

    void ExpandTab(bool instant=false)
    {
        var dur = 0.5f;

        if (instant)
            dur = 0f;

        switch (_screenSide)
        {
            case ScreenSide.LEFT:
                _rect.DOMoveX(_contentTransform.sizeDelta.x, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.RIGHT:
                _rect.DOMoveX(Screen.width - _contentTransform.sizeDelta.x, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.TOP:
                _rect.DOMoveY(Screen.height - _contentTransform.sizeDelta.y, dur).SetEase(Ease.OutQuint);
                break;
            case ScreenSide.BOTTOM:
                _rect.DOMoveY(_contentTransform.sizeDelta.y, dur).SetEase(Ease.OutQuint);
                break;
        }
        _collapsed = false;
    }

    public void ToggleTab()
    {
        if(_collapsed)
            ExpandTab();
        else
            CollapseTab();
    }
}
