import { Map, List, fromJS } from 'immutable';
import { handleAction, handleActions } from 'redux-actions';
import { SHOW_MODAL, HIDE_MODAL } from 'actions/modals';

export default handleActions({
    SHOW_MODAL: (state, action) => state.setIn([action.payload.modalName, 'visible'], true),
    HIDE_MODAL: (state, action) => state.setIn([action.payload.modalName, 'visible'], false)
}, fromJS({ }));
