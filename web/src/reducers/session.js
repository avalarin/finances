import { Map, List, fromJS } from 'immutable';
import { handleActions } from 'redux-actions';
import { LOGIN_USER, LOGOUT_USER, SELECT_BOOK } from 'actions/session';

const emptySession = { isAuthorized: false, book: null };

function save(session) {
    var sessionStr = JSON.stringify(session);
    localStorage.setItem('session', sessionStr);
    return Map(session);
}

function loadSession() {
    var localStorageSessionStr = localStorage.getItem('session');
    var localStorageSession;
    if (!localStorageSessionStr) {
        return fromJS(emptySession);
    }
    return fromJS(JSON.parse(localStorageSessionStr));
}

export default handleActions({
    LOGIN_USER: (state, action) => {
        var { username, session, expiresAt } = action.payload;
        return save({isAuthorized: true, book: null, username, session, expiresAt});
    },
    LOGOUT_USER: (state, action) => save(emptySession),
    SELECT_BOOK: (state, action) => {
        var session = state.set('book', action.payload).toJS();
        return save(session);
    }
}, loadSession());
