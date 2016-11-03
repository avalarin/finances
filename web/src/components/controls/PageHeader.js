import React, { Component } from 'react';
import jss from 'react-jss';

const styles = {
    header: {
        fontSize: '24px',
        width: '100%',
        display: 'block',
        marginBottom: '20px'
    }
};

class PageHeader extends Component {
    render() {
        const { sheet: {classes}, text } = this.props;
        return <span className={classes.header}>
            {text}
        </span>;
    }
};

export default jss(styles)(PageHeader);
