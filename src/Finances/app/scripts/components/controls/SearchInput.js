import React from 'react';
import MDLComponent from './MDLComponent';

export default class SearchInput extends MDLComponent {
    render() {
        var className = 'mdl-textfield mdl-js-textfield mdl-textfield--floating-label';
        return <div className={className}>
            <input className="mdl-textfield__input" type="text" />
            <label className="mdl-textfield__label">Search...</label>
        </div>;
    }
};
