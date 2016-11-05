import { createAction } from 'redux-actions';
import { startItemsLoading, finishItemsLoading } from 'actions/lists';
import api from 'api/index';

export function loadBooks() {
    return dispatch => {
        dispatch(startItemsLoading({ listName: 'books' }));
        api.books.load()
            .then(books => dispatch(finishItemsLoading({ listName: 'books', items: books })));
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