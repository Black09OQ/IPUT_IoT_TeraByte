using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Unity.VisualScripting;
using System.Threading.Tasks;

public enum ListType
{
    LINES,
    OOSAKAKANJYOUSEN,
    KANSAIKUUKOUSEN,
    YAMATOJISEN,
    HANWASEN
}

public class DataLoder : MonoBehaviour
{
    public TextAsset LinesJson;
    public TextAsset OsakakaanjyousenJson;
    public TextAsset KansaikuukousenJson;
    public TextAsset YamatojisenJson;
    public TextAsset HanwasenJson;

    FirebaseFirestore db;

    [HideInInspector]
    public StationsList stationsList;

    void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }


    public async Task<List<Station>> AttachData(ListType listType)
    {
        switch (listType)
        {
            case ListType.LINES:
                LoadJson(LinesJson);
                break;
            case ListType.OOSAKAKANJYOUSEN:
                LoadJson(OsakakaanjyousenJson);
                break;
            case ListType.KANSAIKUUKOUSEN:
                LoadJson(KansaikuukousenJson);
                break;
            case ListType.YAMATOJISEN:
                LoadJson(YamatojisenJson);
                break;
            case ListType.HANWASEN:
                LoadJson(HanwasenJson);
                break;
            default:
                stationsList = null;
                break;
        }

        if (listType != ListType.LINES)
        {
            await GetDBData();
        }

        return stationsList.Stations;

    }

    public void LoadJson(TextAsset json)
    {
        stationsList = JsonUtility.FromJson<StationsList>(json.text);
    }

    public Task GetDBData()
    {
        Debug.Log("GetDBData");
        Query capitalQuery = db.Collection("CO2Data").OrderByDescending("Date").Limit(1);
        return capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
            {
                Dictionary<string, object> city = documentSnapshot.ToDictionary();

                Station priStation = new Station();

                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log("search...");
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));

                    if ((string)pair.Key == "Name")
                    {
                        priStation.stationName = (string)pair.Value;
                    }

                    if ((string)pair.Key == "CO2Value")
                    {
                        priStation.co2 = pair.Value.ConvertTo<int>();
                    }


                }

                foreach (Station station in stationsList.Stations)
                {
                    if (priStation.stationName == station.stationName)
                    {
                        station.co2 = priStation.co2;
                    }
                }

                Debug.Log(priStation.stationName);
                // Newline to separate entries
            };




        });

    }
}

[Serializable]
public class StationsList
{
    public List<Station> Stations = new List<Station>();
}
