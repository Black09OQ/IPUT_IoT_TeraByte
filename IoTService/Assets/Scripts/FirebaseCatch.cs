using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class Station
{
    public string name;
    public string nameJP;
    public int through;
    public int price;
}

public class FirebaseCatch : MonoBehaviour
{
    [SerializeField] GameObject purchasePanel;
    [SerializeField] GameObject warnPanel;
    [SerializeField] TextMeshProUGUI nameJPTMP;
    [SerializeField] TextMeshProUGUI priceTMP;
    public float timeOut;
    private float timeElapsed;

    FirebaseFirestore db;

    public Station[] stations = new Station[9];

    public ButtonColorChanger bcc;



    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        
        for(int i = 0; i < 9; i++){
            stations[i] = new Station();
        }
        stations[0].name = "UMEDA";
        stations[0].price = 0;
        stations[0].nameJP = "梅田";
        stations[1].name = "NAMBA";
        stations[1].price = 0;
        stations[1].nameJP = "難波";
        stations[2].name = "KOBE";
        stations[2].price = 0;
        stations[2].nameJP = "神戸";
        stations[3].name = "SHINIMAMIYA";
        stations[3].price = 0;
        stations[3].nameJP = "新今宮";
        stations[4].name = "KYOTO";
        stations[4].price = 0;
        stations[4].nameJP = "京都";
        stations[5].name = "NARA";
        stations[5].price = 0;
        stations[5].nameJP = "奈良";
        stations[6].name = "HONMACHI";
        stations[6].price = 0;
        stations[6].nameJP = "本町";
        stations[7].name = "TENNOUJI";
        stations[7].price = 0;
        stations[7].nameJP = "天王寺";
        stations[8].name = "AMAGASAKI";
        stations[8].price = 0;
        stations[8].nameJP = "尼崎";
        foreach(Station station in stations){
            station.through = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeOut) {
            foreach(Station station in stations){
                station.through = 0;
            }
            GetData();



            timeElapsed = 0.0f;
        }
    }

    void GetData(){
        DateTime dtNow = DateTime.Now;
        DateTime dt1HourBefore = dtNow - System.TimeSpan.FromHours(1);
        Timestamp ts = Timestamp.FromDateTime(dt1HourBefore);
        //Debug.Log(ts);
        Query capitalQuery = db.Collection("ThroughData").WhereGreaterThan("Date", ts);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents) {
                //Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> city = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city) {
                    //Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                    if ((string)pair.Key == "Name"){
                        foreach(Station station in stations){
                            if(station.name == (string)pair.Value){
                                station.through += 1;
                                
                            }

                        }
                    }
                    
                }

                // Newline to separate entries
            };

            /*
            foreach(Station station in stations){
            Debug.Log(station.name + " : " + station.through);
            }
            */
            bcc.ButtonColorChange(stations);
        });





    }

    public void OnStationButtonDown(int num){
        purchasePanel.SetActive(true);
        nameJPTMP.SetText(stations[num].nameJP);
        priceTMP.SetText("￥" + stations[num].price.ToString());

        if(stations[num].through >= (bcc.maxThrough * 0.7)){
            warnPanel.SetActive(true);
        }else{
            warnPanel.SetActive(false);
        }
    }
}
