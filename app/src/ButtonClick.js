ButtonClick.js

import React,{
	Component,
	Text,
	View,
	StyleSheet
} from 'react-native';

class ButtonClick extends Component {
  _onPressButton() {
    console.log("You tapped the button!");
  }

  render() {
    return (
      <TouchableHighlight onPress={this._onPressButton}>
        <Text>Button</Text>
      </TouchableHighlight>
    );
  }
}

AppRegistry.registerComponent('ButtonClick', () => ButtonClick)