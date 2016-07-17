import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Router, Route, IndexRedirect, browserHistory } from 'react-router';
import { syncHistoryWithStore } from 'react-router-redux'
import App from './App';
import AppSidebar from './AppSidebar';
import AboutPage from './AboutPage';
import LoginPage from './login/LoginPage';
import CounterList from './CounterList';
import { isAuthorized } from 'selectors/auth';

class AppRouter extends Component {

    constructor() {
        super();

        this.requireAuth = this.requireAuth.bind(this);
        this.preventDoubleLogin = this.preventDoubleLogin.bind(this);
    }

    requireAuth(nextState, replace) {
        if (!isAuthorized(this.props.store.getState())) {
            replace({
              pathname: '/login',
              state: { nextPathName: nextState.location.pathname }
          });
        }
    }

    preventDoubleLogin(nextState, replace) {
        if (isAuthorized(this.props.store.getState())) {
            replace({
              pathname: '/'
          });
        }
    }

    render() {
        const history = syncHistoryWithStore(browserHistory, this.props.store);
        return <Router history={history}>
            <Route path="/" component={App}>
                <IndexRedirect to="/dashboard" />
                <Route path="login" component={LoginPage} onEnter={this.preventDoubleLogin} />
                <Route component={AppSidebar}>
                    <Route path="dashboard" component={AboutPage} onEnter={this.requireAuth} />
                    <Route path="counter" component={CounterList} onEnter={this.requireAuth} />
                    <Route path="about" component={AboutPage} onEnter={this.requireAuth} />
                </Route>
            </Route>
        </Router>
    }
}

export default AppRouter;
