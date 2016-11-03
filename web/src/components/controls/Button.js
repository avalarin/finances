import React, { Component } from 'react';
import jss from 'react-jss'

const styles = {
    button: {
        background: '#2196f3',
        color: 'white',
        padding: '.5rem',
        border: 0,
        cursor: 'pointer',
        'marginBottom': '.5rem',
        '&:hover:not(:disabled)': {
            background: '#1e88e5'
        },
        '&:active:not(:disabled)': {
            background: '#1976d2'
        },
        '&:focus:not(:disabled)': {
            outline: 'none'
        },
        '&:disabled': {
            opacity: '0.6'
        }
    }
};

class Button extends Component {
    render() {
        const { text, form, type = 'button', disabled = false,
                sheet: {classes}, onClick } = this.props;
        return <button className={classes.button} type={type} disabled={disabled} form={form} onClick={onClick}>
            { text }
        </button>;
    }
};

export default jss(styles)(Button);
