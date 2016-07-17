import React from 'react';
import MDLComponent from './MDLComponent';
import classNames from 'classNames';

export default class Button extends MDLComponent {
    render() {
        const {text, form, type = 'button', primary = false, disabled = false } = this.props;
        var className = classNames([ 'mdl-button', 'mdl-js-button', 'mdl-js-ripple-effect' ], {
            'mdl-button--raised': primary,
            'mdl-button--primary': primary
        });
        return <button className={className} type={type} disabled={disabled} form={form}>
            { text }
        </button>;
    }
};
