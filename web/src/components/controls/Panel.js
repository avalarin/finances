import React, { Component } from 'react';
import jss from 'react-jss'

const styles = {
    panel: {
        marginBottom: '1rem'
    },
    header: { }
};

class Panel extends Component {
    render() {
        const { header, children, sheet: {classes} } = this.props;
        return <div className={classes.panel}>
            <h3 className={classes.header}>{header}</h3>
            {children}
        </div>;
    }
};

export default jss(styles)(Panel);
