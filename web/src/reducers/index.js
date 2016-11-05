import { combineReducers } from 'redux'
import { reducer as formReducer } from 'redux-form';
import { routerReducer } from 'react-router-redux'
import counters from './counters';
import session from './session';
import lists from './lists';
import modals from './modals';

export default combineReducers({
    counters, session, lists,
    modals,
    form: formReducer,
    routing: routerReducer
});
