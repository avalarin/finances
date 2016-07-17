import { combineReducers } from 'redux'
import { reducer as formReducer } from 'redux-form';
import { routerReducer } from 'react-router-redux'
import counters from './counters';
import auth from './auth';

export default combineReducers({
    counters, auth,
    form: formReducer,
    routing: routerReducer
});
