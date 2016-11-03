import { createStore, applyMiddleware } from 'redux'
import { browserHistory } from 'react-router';
import { routerMiddleware as createRouterMiddleware } from 'react-router-redux'
import thunkMiddleware from 'redux-thunk'
import createLogger from 'redux-logger'
import reducers from './reducers/index';

const loggerMiddleware = createLogger();
const routerMiddleware = createRouterMiddleware(browserHistory);
const middlewares = applyMiddleware(
    thunkMiddleware, routerMiddleware, loggerMiddleware);

export default function configureStore(initialState) {
    return createStore(
        reducers,
        initialState,
        middlewares
    );
};
