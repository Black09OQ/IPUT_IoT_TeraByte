const Obniz = require("obniz");
const mqtt = require("mqtt");

console.log("Start");

let client = mqtt.connect('mqtt://localhost:1883');

const obniz = new Obniz("9588-2575");
// obnizオンライン
obniz.onconnect = async function () {
  // 距離センサ「GP2Y0A21YK0F」の定義
    const sensor = obniz.wired("GP2Y0A21YK0F", { vcc:2, gnd:1, signal:0 });
  
    obniz.display.print("Initializing...");
  // 計測処理
    obniz.onloop = async function() {
        obniz.display.clear();
        let distance = await sensor.getWait();
        //document.getElementById("text-distance").innerHTML = distance;
        
        if(distance < 200){
            obniz.display.print("Detected!");
            obniz.display.print(distance.toString());

            topic = "/topic/through"
            metric = "NAMBA";
            client.publish(topic, metric);

        }else{
            obniz.display.print("Not Detected");
            obniz.display.print(distance.toString());
        }
        
        await obniz.wait(130);
    };
};