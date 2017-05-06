import React, { Component } from 'react';
import { Accelerometer, Gyroscope } from 'react-native-sensors';
import { Loop, Stage, World } from 'react-game-kit/native';

import {
  AppRegistry,
  StyleSheet,
  Text,
  View
} from 'react-native';

export default class App extends Component {

  constructor(props) {
    super(props);
    // const accelerationObservable = new Accelerometer({
    //   handle: 'Accelerometers',
    //   updateInterval: 100,
    // });
  }

  render() {

    const accelerationObservable = new Accelerometer({
       updateInterval: 100,
    });

    console.log('Initialized accelerometer!');
    accelerationObservable
      .map(({ x, y, z }) => x )
      .filter(speed => speed > 5)
      .subscribe(speed => console.log(`You moved your phone with ${speed}`));

    setTimeout(() => {
      accelerationObservable.stop();
    }, 100000);

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
