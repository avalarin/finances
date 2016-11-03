import React, { Component } from 'react';
import jss from 'react-jss';
import classNames from 'classnames';

const styles = {
    container: {
        marginBottom: '1rem'
    },
    input: {

    },
    invalid: {
        
    },
    label: {
        fontSize: '0.8rem',
        display: 'block',
        color: 'black'
    },
    error: {
        
    }
};

class Input extends Component {
    render() {
        const { label, field, type = 'text', sheet: {classes} } = this.props;

        var cx = classNames.bind(classes);
        var isInvalid = field.touched && field.error;
        var inputClass = cx('input', {
            'invalid': isInvalid
        });

        return <div className={classes.container}>
            <label className={classes.label}>{label}</label>
            <input type={type} {...field} />

            { isInvalid && <span className={classes.error}>{field.error}</span> }
        </div>;
    }
};

export default jss(styles)(Input);
