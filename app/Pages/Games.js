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
        <Rectangle></Rectangle>
        <Text style={styles.welcome}>
          Welcome!
          {"\n"}
          Get ready to play Lucky Red
        </Text>
        <Text style={styles.instructions}>
          To get started, click on the red ball.
          {"\n"}
          {"\n"}
        </Text>
        <Text style={styles.instructions}>
          If it hits the top rectangle, you win!
          {'\n'}
          If it hits the bottom, you lose!
          {"\n"}
          {"\n"}
          {"\n"}
        </Text>
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
