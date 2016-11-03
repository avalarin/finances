import { createAction } from 'redux-actions';
import { startSubmit, stopSubmit } from 'redux-form';
import { push as pushRoute } from 'react-router-redux'
import * as login from 'api/login';

export const LOGIN_USER = 'LOGIN_USER';
export const LOGOUT_USER = 'LOGOUT_USER';
export const SELECT_BOOK = 'SELECT_BOOK';

export const loginUser = createAction(LOGIN_USER);
export const logoutUser = createAction(LOGOUT_USER);
export const selectBook = createAction(SELECT_BOOK);

export function loginUserAsync(username, password, callback) {
    return function(dispatch) {
        login.loginUser(username, password)
            .then(s => {
                var session = { username, session: s.session, expiresAt: s.expiresAt };
                console.log(session);
                localStorage.setItem('session', JSON.stringify(session));

                dispatch(loginUser(session));
                dispatch(pushRoute('/'));
                callback();
            })
            .catch(err => {
                console.error(err);
                callback(err);
            });
    };
};

export function logoutUserAsync() {
    return function(dispatch) {
        localStorage.removeItem('session');

        dispatch(logoutUser());
        dispatch(pushRoute('/login'));
    };
};

export function restoreSession() {
    return function(dispatch) {
        var localStorageSessionStr = localStorage.getItem('session');
        var localStorageSession;
        if (localStorageSessionStr) {
            localStorageSession = JSON.parse(localStorageSessionStr);
            dispatch(loginUser(localStorageSession));
        }
    }
}
