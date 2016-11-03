import React, { Component } from 'react';
import jss from 'react-jss';
import classNames from 'classnames';

const styles = {
    container: {
        marginBottom: '1rem'
    },
    input: {
        padding: '0.5rem',
        border: '1px solid #E5E5E5',
        
        '&:active': {

        },
        '&:focus': {
            outline: 'none'
        }
    },
    invalidInput: {
        border: '1px solid #DD2C00'
    },
    label: {
        fontSize: '0.8rem',
        display: 'block'
    },
    invalidLabel: {
        color: '#DD2C00'
    },
    error: {
       textTransform: 'lowercase'
    }
};

class TextInput extends Component {
    render() {
        const { label, field, type = 'text', sheet: {classes} } = this.props;
        var isInvalid = field.touched && field.error;
        var inputClass = classNames(classes.input, {
            [classes.invalidInput]: isInvalid
        });
        var labelClass = classNames(classes.label, {
            [classes.invalidLabel]: isInvalid
        });
        return <div className={classes.container}>
            <label className={labelClass}>
                {label}
                { isInvalid && <span className={classes.error}> - {field.error}</span> }
            </label>
            <input className={inputClass} type={type} {...field} />
        </div>;
    }
};

export default jss(styles)(TextInput);
