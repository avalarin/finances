import { createSelector } from 'reselect'

export function getSession(state) {
    return state.auth.toJS();
}

export function isAuthorized(state) {
    return state.auth.get('isAuthorized', false);
};
