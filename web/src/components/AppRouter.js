import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Router, Route, IndexRedirect, browserHistory } from 'react-router';
import { syncHistoryWithStore } from 'react-router-redux'
import App from './App';
import AppSidebar from './AppSidebar';
import AboutPage from './AboutPage';
import DashboardPage from './dashboard/DashboardPage';
import BooksIndexPage from './books/IndexPage';
import LoginPage from './login/LoginPage';
import CounterList from './CounterList';
import { isAuthorized, hasBook } from 'selectors/session';

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
            return false;
        }
        return true;
    }

    requireBook(nextState, replace) {
        if (!hasBook(this.props.store.getState())) {
            replace({
                pathname: '/books',
                state: { nextPathName: nextState.location.pathname }
            });
            return false;
        }
        return true;
    }

    requireAuthAndBook(nextState, replace) {
        return this.requireAuth(nextState, replace)
            && this.requireBook(nextState, replace);
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
                    <Route path="books" component={BooksIndexPage} onEnter={this.requireAuth} />
                    <Route path="dashboard" component={DashboardPage} onEnter={this.requireAuthAndBook.bind(this)} />
                    <Route path="counter" component={CounterList} onEnter={this.requireAuthAndBook.bind(this)} />
                    <Route path="about" component={AboutPage} onEnter={this.requireAuthAndBook.bind(this)} />
                </Route>
            </Route>
        </Router>
    }
}

export default AppRouter;
