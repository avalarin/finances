import { Map, List, fromJS } from 'immutable';
import { handleAction, handleActions } from 'redux-actions';
import { SELECT_ITEM, START_ITEMS_LOADING, FINISH_ITEMS_LOADING } from 'actions/lists';

export default handleActions({
    SELECT_ITEM: (state, action) => state.setIn([action.payload.listName, 'selected'], action.payload.newIndex),
    START_ITEMS_LOADING: (state, action) => state.setIn([action.payload.listName, 'loading'], true),
    FINISH_ITEMS_LOADING: (state, action) => state.setIn([action.payload.listName, 'loading'], false)
                                                  .setIn([action.payload.listName, 'items'], action.payload.items)
}, fromJS({ }));
