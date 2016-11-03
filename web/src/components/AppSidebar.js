import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router';
import { getSession } from 'selectors/session';
import { logoutUserAsync } from 'actions/session';
import jss from 'react-jss'

const styles = {
    layout: {
        width: '1170px',
        margin: '50px auto'
    },
    sidebar: {
        width: '25%',
        float: 'left',
        padding: '0 15px',
        boxSizing: 'border-box'
    },
    content: {
        width: '75%',
        float: 'right',
        padding: '0 15px',
        boxSizing: 'border-box'
    },

    brandName: {
        fontSize: '24px',
        width: '100%',
        display: 'block',
        marginBottom: '20px'
    },

    sidebarLink: {
        display: 'block',
        width: '100%',
        textDecoration: 'none',
        fontSize: '14px',
        padding: '0',
        borderRadius: '2px',
        lineHeight: '36px',
        verticalAlign: 'middle',
        fontWeight: '500',
        color: '$text-color-primary'
    },

    sidebarLinkActive: {
        color: '$text-color-accent',
        fontWeight: 'bold'
    }
};

class AppSidebar extends Component {
    constructor({sheet: {classes}, children}) {
        super();
        this.classes = classes;
    }

    render() {
        const classes = this.classes;
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

        return <div className={classes.layout}>
            <div>
                <div className={classes.sidebar}>
                    <nav>
                        <span className={classes.brandName}>
                            <Link to="/">Finances</Link>
                        </span>
                        <Link className={classes.sidebarLink} activeClassName={classes.sidebarLinkActive} to="/dashboard">Dashboard</Link>
                        <Link className={classes.sidebarLink} activeClassName={classes.sidebarLinkActive} to="/counter">Counter</Link>
                        <Link className={classes.sidebarLink} activeClassName={classes.sidebarLinkActive} to="/about">About</Link>
                        <a className={classes.sidebarLink} href="javascript:" onClick={onLogout}>Logout</a>
                    </nav>
                </div>
                <main className={classes.content}>
                    <div>{ children }</div>
                </main>
            </div>
        </div>;
    }
}

export default connect(
    (state, ownProps) => getSession(state),
    (dispatch, ownProps) => ({
        onLogout: () => dispatch(logoutUserAsync())
    }))(jss(styles)(AppSidebar));
