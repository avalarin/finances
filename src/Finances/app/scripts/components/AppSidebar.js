import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router';
import { getSession } from 'selectors/auth';
import { logoutUserAsync } from 'actions/auth';
import { SearchInput } from 'components/controls/index';

class AppSidebar extends Component {
    render() {
        const { children, isAuthorized, username, onLogout } = this.props;

        var navigationMenu;
        if (isAuthorized) {
            navigationMenu = <nav className="mdl-navigation mdl-layout--large-screen-only">
                <Link className="mdl-navigation__link" to="/counter">Counter</Link>
                <Link className="mdl-navigation__link" to="/about">About</Link>
                <a className="mdl-navigation__link" href="javascript:">{ username }</a>
            </nav>;
        } else {
            navigationMenu = <nav className="mdl-navigation mdl-layout--large-screen-only">
                <Link className="mdl-navigation__link" to="/login">Login</Link>
            </nav>;
        }

        return <div className="app-layout">
            <div className="app-layout__container">
                <div className="app-layout__sidebar">
                    <nav className="app-sidebar">
                        <span className="app-sidebar__brandname">
                            <Link to="/">Finances</Link>
                        </span>
                        <Link className="app-sidebar__link" activeClassName="app-sidebar__link--active" to="/dashboard">Dashboard</Link>
                        <Link className="app-sidebar__link" activeClassName="app-sidebar__link--active" to="/counter">Counter</Link>
                        <Link className="app-sidebar__link" activeClassName="app-sidebar__link--active" to="/about">About</Link>
                        <a className="app-sidebar__link" href="javascript:" onClick={onLogout}>Logout</a>
                    </nav>
                </div>
                <main className="app-layout__content">
                    <div className="app-content">{ children }</div>
                </main>
            </div>
        </div>;
    }
}

export default connect(
    (state, ownProps) => getSession(state),
    (dispatch, ownProps) => ({
        onLogout: () => dispatch(logoutUserAsync())
    }))(AppSidebar);
