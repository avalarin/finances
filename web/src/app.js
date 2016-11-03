import 'material-design-lite';

import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import AppRouter from './components/AppRouter';
import { restoreSession } from './actions/session';

import configureStore from './configureStore';

if (!window.settings) throw Error('window.settings is required');
if (!window.settings.apiAddress) throw Error('window.settings.apiAddress is required');

const store = configureStore();
store.dispatch(restoreSession());

ReactDOM.render(
    <Provider store={store}>
        <AppRouter store={store} />
    </Provider>,
    document.getElementById('app')
);

if (module.hot) {
    module.hot.accept('./components/AppRouter', () => {
        var NextAppRouter = require('./components/AppRouter').default;
        ReactDOM.render(
            <Provider store={store}>
                <NextAppRouter store={store} />
            </Provider>,
            document.getElementById('app')
        );
    });
}