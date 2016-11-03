import React, { Component } from 'react';
import MDLComponent from './MDLComponent';
import jss from 'react-jss';

const styles = {
    indicator: {
        marginTop: '15px',
        width: '70px',
        textAlign: 'center'
    },
    bounce: {
        width: '8px',
        height: '8px',
        backgroundColor: '#373A3C',
        borderRadius: '100%',
        display: 'inline-block',
        marginRight: '5px',
        animation: 'sk-bouncedelay 1.4s infinite ease-in-out both'
    },
    bounce1: { animationDelay: '-0.32s' },
    bounce2: { animationDelay: '-0.16s' },
    bounce3: { },
    '@keyframes sk-bouncedelay': {
        '0%, 80%, 100%': { transform: 'scale(0)' },
        '40%': { transform: 'scale(1.0)' }
    }
};

class LoadingIndicator extends Component {
    render() {
        const { visible = true, sheet: {classes} } = this.props;
        if (!visible) return null;
        return <div className={classes.indicator}>
            <div className={`${classes.bounce} ${classes.bounce1}`}></div>
            <div className={`${classes.bounce} ${classes.bounce2}`}></div>
            <div className={`${classes.bounce} ${classes.bounce3}`}></div>
        </div>;
    }
};

export default jss(styles)(LoadingIndicator);
