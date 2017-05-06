import React, { Component } from 'react';
//import { Image, Dimensions, PanResponder, View } from 'react-native';
import {
  Body,
  Loop,
  Stage,
  World,
} from 'react-game-kit';

import Matter from 'matter-js';

import {
  AppRegistry,
  StyleSheet,
  Text,
  View
} from 'react-native';



export default class Game extends Component {
  render() {
    return (
      <View style={styles.container}>
        <Text style={styles.welcome}>
          Welcome to React Native! Chankyu Oh!!
        </Text>
        <Text style={styles.instructions}>
          To get started, edit index.ios.js
        </Text>
        <Text style={styles.instructions}>
          Press Cmd+R to reload,{'\n'}
          Cmd+D or shake for dev menu
        </Text>
        <Rectangle></Rectangle>
        <Ball></Ball>
      </View>
    );
  }
}

export class Rectangle extends Component {
  render() {
    return (
      <View style={styles.rectangle} />
    )
  }
};

export class Ball extends Component {
  render() {
    return (
      <View style={styles.ball} />
    )
  }
};

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
  ball: {
    width: 100,
    height: 100,
    borderRadius: 100/2,
    backgroundColor: 'red'
  },
  rectangle: {
    width: 320,
    height: 50,
    backgroundColor: 'red'
  }
})

