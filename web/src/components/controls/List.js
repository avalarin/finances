import React, { Component } from 'react';
import { connect } from 'react-redux';
import { selectItem } from 'actions/lists';
import { getSelected } from 'selectors/lists';
import jss from 'react-jss';

const styles = {
    list: {
        margin: '15px 0'
    },
    item: {
        padding: '10px 10px',
        background: '#fafafa',
        borderWidth: '1px 1px 0 1px',
        borderColor: '#e0e0e0',
        borderStyle: 'solid',
        cursor: 'pointer',
        '&:last-child': {
            borderBottom: '1px #e0e0e0 solid'
        },
        '&.selected': {
            background: '#eeeeee'
        }
    }
};

class List extends Component {
    render() {
        const { listName, selected, onSelect, items, component, emptyMessage, sheet: {classes} } = this.props;
        if (!items.length) {
            return <div>{emptyMessage}</div>;
        }
        return <div className={classes.list}>
            { items.map((item, i) => {
                var className = classes.item;
                if (selected==i) className += ' selected';
                return <div className={className} key={i} onClick={() => onSelect(i)}>
                    { React.createElement(component, { item: item, key: i, index: i })}
                </div>;
            }) }
        </div>;
    }
};

export default connect((state, ownProps) => ({
    selected: getSelected(state, ownProps.listName)
}), (dispatch, ownProps) => ({
    onSelect: (newIndex) => dispatch(selectItem({ listName: ownProps.listName, newIndex: newIndex }))
}))(jss(styles)(List));
