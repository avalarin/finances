import { createAction } from 'redux-actions';

export const INCREMENT_COUNTER = 'INCREMENT_COUNTER';
export const DECREMENT_COUNTER = 'DECREMENT_COUNTER';
export const ADD_COUNTER = 'ADD_COUNTER';
export const DELETE_COUNTER = 'DELETE_COUNTER';

export const incrementCounter = createAction(INCREMENT_COUNTER);
export const decrementCounter = createAction(DECREMENT_COUNTER);
export const addCounter = createAction(ADD_COUNTER);
export const deleteCounter = createAction(DELETE_COUNTER);
