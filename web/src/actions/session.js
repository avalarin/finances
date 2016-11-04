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
                dispatch(loginUser(session));
                dispatch(pushRoute('/'));
                callback();
            })
            .catch(callback);
    };
};

export function logoutUserAsync() {
    return function(dispatch) {
        dispatch(logoutUser());
        dispatch(pushRoute('/login'));
    };
};
