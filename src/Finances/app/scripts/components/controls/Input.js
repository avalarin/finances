import React from 'react';
import MDLComponent from './MDLComponent';

export default class Input extends MDLComponent {
    render() {
        const {header, field, type = 'text'} = this.props;
        var className = 'mdl-textfield mdl-js-textfield mdl-textfield--floating-label';
        var isInvalid = field.touched && field.error;
        if (isInvalid) className += ' is-invalid';
        return <div className={className}>
            <input className="mdl-textfield__input" type="{type}" {...field}/>
            <label className="mdl-textfield__label">{header}</label>
            {isInvalid && <span className="mdl-textfield__error">{field.error}</span>}
        </div>;
    }
};
