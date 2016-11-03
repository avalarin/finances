import { Map, List, fromJS } from 'immutable';
import { handleAction, handleActions } from 'redux-actions';
import { INCREMENT_COUNTER, DECREMENT_COUNTER, ADD_COUNTER, REMOVE_COUNTER } from 'actions/counters';

export default handleActions({
    INCREMENT_COUNTER: (state, action) => state.updateIn([action.payload, 'value'], 0, i => i+1),
    DECREMENT_COUNTER: (state, action) => state.updateIn([action.payload, 'value'], 0, i => i-1),
    ADD_COUNTER: (state, action) => state.push(fromJS({value: 0})),
    DELETE_COUNTER: (state, action) => state.delete(action.payload)
}, fromJS([ { value: 0 } ]));
