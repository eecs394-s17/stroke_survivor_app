import React, { Component } from 'react';

import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  Navigator
} from 'react-native';

export default class App extends Component {

  render() {

    return (
      <View style={styles.container}>
        <Text style={styles.welcome}>
          Hello Stroke Survivor!
        </Text>
        <Text style={styles.instructions}>
          To get started, click on the games tab.
        </Text>
        <Text style={styles.instructions}>
         Good luck!
        </Text>
      </View>
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
