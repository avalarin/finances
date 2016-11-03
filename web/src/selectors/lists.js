export function getSelected(state, listName) {
    return state.lists.getIn([listName, 'selected']);
}

export function hasSelection(state, listName) {
    return typeof state.lists.getIn([listName, 'selected']) !== 'undefined';
}
