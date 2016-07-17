import 'material-design-lite';

import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import AppRouter from './components/AppRouter'
import { restoreUser } from './actions/auth';

import configureStore from './configureStore';

window.apiAddress = 'http://localhost:5000';

const store = configureStore();
store.dispatch(restoreUser());

ReactDOM.render(
    <Provider store={store}>
        <AppRouter store={store} />
    </Provider>,
    document.getElementById('app')
);
