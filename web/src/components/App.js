import React, { Component } from 'react';
import { connect } from 'react-redux';

class App extends Component {
    render() {
        return <div>{ this.props.children }</div>;
    }
}

export default connect(
    (state, ownProps) => ({}),
    (dispatch, ownProps) => ({}))(App);
