import { combineReducers } from 'redux'
import { reducer as formReducer } from 'redux-form';
import { routerReducer } from 'react-router-redux'
import counters from './counters';
import session from './session';
import books from './books';
import lists from './lists';
import modals from './modals';

export default combineReducers({
    counters, session, books, lists,
    modals,
    form: formReducer,
    routing: routerReducer
});
