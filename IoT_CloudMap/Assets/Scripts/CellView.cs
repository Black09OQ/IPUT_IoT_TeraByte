using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using TMPro;

public delegate void CellClickedDelegate(string value);

public class CellView : EnhancedScrollerCellView
{
    [SerializeField] Sprite notCroudSprite;
    [SerializeField] Sprite croudSprite;
    [SerializeField] Sprite veryCroudSprite;


    private Station _data;

    public TextMeshProUGUI stationNameTMP;
    public Image croudIcon;
    public Image colorBar;
    public Image trainIcon;



    /// <summary>
    ///  These delegates will fire whenever one of the events occurs
    /// </summary>
    public CellClickedDelegate cellClicked;

    public void SetData(Station data)
    {
        _data = data;

        RefreshCellView();
    }

    public override void RefreshCellView()
    {
        stationNameTMP.SetText(_data.stationCharacter);
        Color color;
        if (ColorUtility.TryParseHtmlString(_data.stationColor, out color))
        {
            colorBar.color = color;
            trainIcon.color = color;
        }

        if (_data.co2 < 400)
        {
            croudIcon.sprite = notCroudSprite;
        }
        else if (_data.co2 >= 400 && _data.co2 <= 500)
        {
            croudIcon.sprite = croudSprite;
        }
        else
        {
            croudIcon.sprite = veryCroudSprite;
        }
    }

    // Handle the click of the fixed text button (this is hooked up in the Unity editor in the button's click event)
    public void Cell_OnClick()
    {
        // fire event if anyone has subscribed to it
        if (cellClicked != null) cellClicked(_data.stationName);
    }
}
