import { createAction } from 'redux-actions';
import api from 'api/index';

export const START_BOOKS_LOADING = 'START_BOOKS_LOADING';
export const STOP_BOOKS_LOADING = 'STOP_BOOKS_LOADING';

export const startBooksLoading = createAction(START_BOOKS_LOADING);
export const stopBooksLoading = createAction(STOP_BOOKS_LOADING);

export function loadBooks() {
    return dispatch => {
        dispatch(startBooksLoading());
        api.books.load()
            .then(books => dispatch(stopBooksLoading(books)));
    };
};

export function createBook(name, callback) {
    return dispatch => {
        api.books.create(name)
            .then(resp => {
                dispatch(loadBooks());
                callback();
            })
            .catch(callback);
    };
};