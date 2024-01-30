using UnityEngine;
using System.Collections.Generic;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;

public class Controller : MonoBehaviour, IEnhancedScrollerDelegate
{
    private List<Station> _data;

    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;
    public float cellSize;

    public DataLoder dataLoder;

    void Start()
    {
        // set the application frame rate.
        // this improves smoothness on some devices
        Application.targetFrameRate = 60;

        scroller.Delegate = this;

        dataLoder = GameObject.Find("DataLoder").GetComponent<DataLoder>();

        LoadData(ListType.LINES);
    }

    private async void LoadData(ListType listType)
    {
        _data = await dataLoder.AttachData(listType);

        scroller.ReloadData();
    }

    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;

        // Set handlers for the cell views delegates.
        // Each handler will respond to a different type of event
        cellView.cellClicked = CellClicked;

        cellView.SetData(_data[dataIndex]);


        return cellView;
    }

    #endregion

    /// <summary>
    /// Handler for when the cell view fires a fixed text button click event
    /// </summary>
    /// <param name="value">value of the text</param>
    private void CellClicked(string stationName)
    {
        switch (stationName)
        {
            case "OOSAKAKANJYOUSEN":
                LoadData(ListType.OOSAKAKANJYOUSEN);
                break;
            case "KANSAIKUUKOUSEN":
                LoadData(ListType.KANSAIKUUKOUSEN);
                break;
            case "YAMATOJISEN":
                LoadData(ListType.YAMATOJISEN);
                break;
            case "HANWASEN":
                LoadData(ListType.HANWASEN);
                break;
            default:
                Debug.Log(stationName);
                break;
        }
    }

    public void OnBackButtonClicked()
    {
        LoadData(ListType.LINES);
    }
}

