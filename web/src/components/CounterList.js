import React from 'react';
import { connect } from 'react-redux';
import { addCounter } from 'actions/counters';
import Counter from './Counter';

const CounterList = ({ counters, onAdd }) => (
    <div>
        <div className="app-page-title">
            <h4>Counters</h4>
        </div>
        { counters.map((counter, i) => <Counter key={i} index={i} />) }
        <br/>
        <button onClick={onAdd}>Add counter</button>
    </div>
);

export default connect((state, ownProps) => ({
    counters: state.counters.toJS()
}), (dispatch, ownProps) => ({
    onAdd: () => dispatch(addCounter())
}))(CounterList);
