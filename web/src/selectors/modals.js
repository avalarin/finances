export function isVisible(state, modalName) {
    return state.modals.getIn([modalName, 'visible']);
}