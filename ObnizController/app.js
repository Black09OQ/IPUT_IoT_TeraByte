const Obniz = require("obniz");
const mqtt = require("mqtt");

console.log("Start");

let client = mqtt.connect('mqtt://localhost:1883');




var obniz = new Obniz("4496-5359");
obniz.onconnect = async function () {
    await obniz.connectWait();
    const RS_BTEVS1 = Obniz.getPartsClass('RS_BTEVS1');
    await obniz.ble.initWait();

    obniz.ble.scan.onfind = async (peripheral) => {
        if (RS_BTEVS1.isDevice(peripheral)) {
            console.log('find');

            console.log(peripheral.address);
          
            const device = new RS_BTEVS1(peripheral);
            device.ondisconnect = (reason) => {
            console.log(reason)
            };
          
            await device.connectWait();
            console.log('connected');

            obniz.onloop = async function() {
                

                const dataResult = await device.getDataWait();
                console.log(dataResult);
                var showData ="\nCO2 :" + dataResult.co2 + "\nTemp :"+dataResult.temp.toFixed(1) + "\nHumid :"+dataResult.humid;
                obniz.display.print(showData);

                topic = "/topic/tem"
                metric = dataResult.temp.toFixed(1).toString()
                client.publish(topic, metric);

                topic = "/topic/hum"
                metric = dataResult.humid.toString()
                client.publish(topic, metric);

                topic = "/topic/co2"
                metric = dataResult.co2.toString()
                client.publish(topic, metric);

                
                
                const datenow = new Date();
            };
        }
    };

    await obniz.ble.scan.startWait();
}