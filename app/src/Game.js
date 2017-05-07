import React, { Component } from 'react';
import { Image, Dimensions, PanResponder, View, TouchableHighlight, Text } from 'react-native';
import {
  Body,
  Loop,
  Stage,
  World,
} from 'react-game-kit/native';

import Matter from 'matter-js';

export default class Game extends Component {

  handleUpdate = () => {

    this.setState({
      ballPosition: this.body.body.position,
      ballAngle: this.body.body.angle,
    });

    Matter.Body.applyForce(this.body.body, {x:this.body.body.position.x,  y:this.body.body.position.y}, {x:this.state.direction*0.0075, y: 0});
    Matter.Body.setAngularVelocity(this.body.body, this.state.direction*0.1);

    console.log(this.body.body.velocity);
  }

  handleCollision = (events) => {

    // console.log(events.pairs);
    for (var i = 0; i < events.pairs.length; i++){
      // console.log(events.pairs[i].id);
      if (events.pairs[i].id === "1_5" || events.pairs[i].id === "1_4")
      { 
      // 1_5 and 1_4 refer to pairs of bodies with ids 1,5 and 1,4, respectively. The ball has an id of 1, the right wall has an id of 5, the left wall has an id of 4
        this.setState({direction: -1*this.state.direction});
        // console.log("changing direction");
      }

    }


  }

  physicsInit = (engine) => {

    const dimensions = Dimensions.get('window');
    console.log(dimensions);

    const ground = Matter.Bodies.rectangle(
      dimensions.width / 2, dimensions.height + 5,
      dimensions.width, 5,
      {
        isStatic: true,
      },
    );

    const ceiling = Matter.Bodies.rectangle(
      dimensions.width / 2, -75,
      dimensions.width, 1,
      {
        isStatic: true,
      },
    );

    const leftWall = Matter.Bodies.rectangle(
      -75, dimensions.height / 2,
      1, dimensions.height,
      {
        isStatic: true,
      },
    );

    const rightWall = Matter.Bodies.rectangle(
      dimensions.width, dimensions.height / 2,
       1, dimensions.height - 5,
      {
        isStatic: true,
      },
    );


    Matter.World.add(engine.world, [ground, leftWall, rightWall, ceiling]);


  }

  constructor(props) {
    super(props);

    this.state = {
      gravity: 1,
      ballPosition: {
        x: 0,
        y: 0,
      },
      ballAngle: 0,
      direction: 1,
    };
  }

  componentWillMount() {

    this._panResponder = PanResponder.create({
      onStartShouldSetPanResponder: (evt, gestureState) => true,
      onStartShouldSetPanResponderCapture: (evt, gestureState) => true,
      onMoveShouldSetPanResponder: (evt, gestureState) => true,
      onMoveShouldSetPanResponderCapture: (evt, gestureState) => true,
      onPanResponderGrant: (evt, gestureState) => {
        
        // Matter.Body.applyForce(this.body.body, {x: this.body.body.position.x, y: this.body.body.position.y}, {x:0, y:12.5});
        Matter.Body.setPosition(this.body.body, {x:this.body.body.position.x, y:this.body.body.position.y+50});
        // Matter.Body.setVelocity(this.body.body, {x: 0, y: -9.8});

        // this.startPosition = {
        //   x: this.body.body.position.x,
        //   y: this.body.body.position.y,
        // }
      },
      // onPanResponderMove: (evt, gestureState) => {
      //   Matter.Body.setPosition(this.body.body, {
      //     x: this.startPosition.x + gestureState.dx,
      //     y: this.startPosition.y + gestureState.dy,
      //   });
      // },
      onPanResponderRelease: (evt, gestureState) => {
        this.setState({
          gravity: 1,
        })
      }



      //   // Matter.Body.applyForce(this.body.body, {
      //   //   x: this.body.body.position.x,
      //   //   y: this.body.body.position.y,
      //   // }, {
      //   //   x: gestureState.vx,
      //   //   y: gestureState.vy,
      //   // });
      // },
    });

  }

  getBallStyles() {
    return {
      height: 75,
      width: 75,
      position: 'absolute',
      transform: [
        { translateX: this.state.ballPosition.x },
        { translateY: this.state.ballPosition.y },
        { rotate: (this.state.ballAngle * (180 / Math.PI)) + 'deg'}
      ],
    };
  }

  render() {
    
    
    const dimensions = Dimensions.get('window');

    return (
      <Loop>
        <Stage
          width={dimensions.width}
          height={dimensions.height}
          style={{ backgroundColor: '#3a9bdc' }}

        >
          <World
            onInit={this.physicsInit}
            onUpdate={this.handleUpdate}
            onCollision={this.handleCollision}
            gravity={{ x: 0, y: this.state.gravity, scale: 0.001 }}
            
          >
            <Body
              shape="circle"
              args={[0, dimensions.height - 75, 75]}
              density={0.003}
              friction={0}
              frictionStatic={0}
              restitution={0}
              ref={(b) => { this.body = b; }}
            >
              <View
                style={this.getBallStyles()} {...this._panResponder.panHandlers}
              >
                <Image onPress={this._onPressButton}
                  source={require('./assets/basketball.png')}
                  height={75}
                  width = {75}
                />
              </View>
            </Body>
          </World>
        </Stage>
      </Loop>
    );
  }
}


