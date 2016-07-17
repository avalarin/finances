import { Map, List, fromJS } from 'immutable';
import { handleActions } from 'redux-actions';
import { LOGIN_USER, LOGOUT_USER } from 'actions/auth';

export default handleActions({
    LOGIN_USER: (state, action) => {
        var { username, password, session, expiresAt } = action.payload;
        return Map({
            isAuthorized: true,
            username, password, session, expiresAt
        });
    },
    LOGOUT_USER: (state, action) => Map({ isAuthorized: false })
}, fromJS({ isAuthorized: false }));
