import { List } from 'immutable';

export function getSelected(state, listName) {
    return state.lists.getIn([listName, 'selected']);
}

export function hasSelection(state, listName) {
    return state.lists.getIn([listName, 'selected'], -1) != -1;
}

export function isLoading(state, listName) {
    return state.lists.getIn([listName, 'loading'], false);
}

export function getItems(state, listName) {
    return state.lists.getIn([listName, 'items'], []);
}