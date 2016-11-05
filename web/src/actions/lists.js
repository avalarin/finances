import { createAction } from 'redux-actions';

export const SELECT_ITEM = 'SELECT_ITEM';
export const START_ITEMS_LOADING = 'START_ITEMS_LOADING';
export const FINISH_ITEMS_LOADING = 'FINISH_ITEMS_LOADING';

export const selectItem = createAction(SELECT_ITEM);
export const startItemsLoading = createAction(START_ITEMS_LOADING);
export const finishItemsLoading = createAction(FINISH_ITEMS_LOADING);