import React from 'react';
import { connect } from 'react-redux';
import { incrementCounter, decrementCounter, deleteCounter } from 'actions/counters';

const Counter = ({value, onIncrement, onDecrement, onDelete}) => (
    <div>
        <h6>{value}</h6>
        <button onClick={onIncrement}>+</button>
        <button onClick={onDecrement}>-</button>
        <span> </span>
        <button onClick={onDelete}>Delete</button>
    </div>
);

export default connect((state, ownProps) => ({
    value: state.counters.getIn([ownProps.index, 'value'], 0)
}), (dispatch, ownProps) => ({
    onIncrement: () => dispatch(incrementCounter(ownProps.index)),
    onDecrement: () => dispatch(decrementCounter(ownProps.index)),
    onDelete: () => dispatch(deleteCounter(ownProps.index))
}))(Counter);
