import { createSelector } from 'reselect'

export function getSession(state) {
    return state.session.toJS();
}

export function isAuthorized(state) {
    return state.session.get('isAuthorized', false);
};

export function hasBook(state) {
    return !!state.session.get('book', null);
};
