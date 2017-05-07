import React, { Component } from 'react';
import { Accelerometer, Gyroscope } from 'react-native-sensors';
import { Loop, Stage, World } from 'react-game-kit/native';
import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  DeviceEventEmitter,
} from 'react-native';

var mSensorManager = require('NativeModules').SensorManager;

DeviceEventEmitter.addListener('Gyroscope', function (data) {
  /**
  * data.x
  * data.y
  * data.z
  **/
});
mSensorManager.startGyroscope(100);
mSensorManager.stopGyroscope();

mSensorManager.startOrientation(100);
DeviceEventEmitter.addListener('Orientation', function (data) {
  /**
  * data.azimuth
  * data.pitch
  * data.roll
  **/
});
mSensorManager.stopOrientation();


export default class App extends Component {

  constructor(props) {
    super(props);
    // const accelerationObservable = new Accelerometer({
    //   handle: 'Accelerometers',
    //   updateInterval: 100,
    // });
  }

  calculate_rp(x, y, z) {



    var rp = {
      pitch: Math.atan2(-1 * x, Math.sqrt(Math.pow(y, 2) + Math.pow(z, 2))) * 57.3,
      roll: Math.atan2(y, z) * 57.3,
    }

    return rp

  }

  render() {

    // int x, y, z;                        //three axis acceleration data
    // double roll = 0.00, pitch = 0.00;        //Roll & Pitch are the angles which rotate by the axis X and y
    //
    // void RP_calculate(){
    //   double x_Buff = float(x);
    //   double y_Buff = float(y);
    //   double z_Buff = float(z);
    //   roll = atan2(y_Buff , z_Buff) * 57.3;
      // pitch = atan2((- x_Buff) , sqrt(y_Buff * y_Buff + z_Buff * z_Buff)) * 57.3;
    // }

    const accelerationObservable = new Accelerometer({
       updateInterval: 100,
    });


    const gyroscopeObservable = new Gyroscope({
      updateInterval: 1000,
    })


    // .map(({ x, y, z }) => x + y + z )
    // .filter(speed => speed > 5)

    console.log('Initialized accelerometer!');
    accelerationObservable
      .subscribe(axis => console.log("axis: ", this.calculate_rp(axis.x, axis.y, axis.z)));

    setTimeout(() => {
      accelerationObservable.stop();
    }, 100000);

    // gyroscopeObservable
    //   .subscribe(gyro => console.log("gyro: ", gyro));
    //
    // setTimeout(() => {
    //   gyroscopeObservable.stop();
    // }, 100000);

    return (
      <Text>
        Hello, world!
      </Text>
      //   <Loop>
      //     <Stage>
        //
      //     </Stage>
      //   </Loop>
      );

  }

}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  instructions: {
    textAlign: 'center',
    color: '#333333',
    marginBottom: 5,
  },
});
