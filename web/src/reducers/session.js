import { Map, List, fromJS } from 'immutable';
import { handleActions } from 'redux-actions';
import { LOGIN_USER, LOGOUT_USER, SELECT_BOOK } from 'actions/session';

export default handleActions({
    LOGIN_USER: (state, action) => {
        var { username, password, session, expiresAt } = action.payload;
        return Map({
            isAuthorized: true, book: null,
            username, password, session, expiresAt
        });
    },
    LOGOUT_USER: (state, action) => Map({ isAuthorized: false }),
    SELECT_BOOK: (state, action) => state.set('book', action.payload)
}, fromJS({ isAuthorized: false }));
