import { Map, List, fromJS } from 'immutable';
import { handleAction, handleActions } from 'redux-actions';
import { START_BOOKS_LOADING, STOP_BOOKS_LOADING } from 'actions/books';

export default handleActions({
    START_BOOKS_LOADING: (state, action) => state.update('loading', i => true),
    STOP_BOOKS_LOADING: (state, action) => fromJS({ loading: false, items: action.payload })
}, fromJS({ loading: false, selected: null, items: [] }));
