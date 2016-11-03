import { Map, List, fromJS } from 'immutable';
import { handleAction, handleActions } from 'redux-actions';
import { SELECT_ITEM } from 'actions/lists';

export default handleActions({
    SELECT_ITEM: (state, action) => state.setIn([action.payload.listName, 'selected'], action.payload.newIndex)
}, fromJS({ }));
