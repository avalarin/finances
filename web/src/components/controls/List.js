import React, { Component } from 'react';
import { connect } from 'react-redux';
import { selectItem } from 'actions/lists';
import { getSelected, getItems, isLoading } from 'selectors/lists';
import jss from 'react-jss';
import classNames from 'classnames';
import LoadingIndicator from 'components/controls/LoadingIndicator';

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
        cursor: 'pointer'
    },
    itemLast: {
        borderBottom: '1px #e0e0e0 solid'
    },
    itemSelected: {
        background: '#eeeeee'
    }
};

class List extends Component {
    render() {
        const { listName, selected, items, loading, component, emptyMessage, sheet: {classes},
                onSelect } = this.props;
        if (loading) return <LoadingIndicator />;
        if (!items.length) return <div>{emptyMessage}</div>;
        return <div className={classes.list}>
            { items.map((item, i) => {
                var className = classNames({
                    [classes.item]: true,
                    [classes.itemLast]: i==items.length-1,
                    [classes.itemSelected]: selected==i
                });
                return <div className={className} key={i} onClick={() => onSelect(i)}>
                    { React.createElement(component, { item: item, key: i, index: i })}
                </div>;
            }) }
        </div>;
    }
};

export default connect((state, ownProps) => ({
    selected: getSelected(state, ownProps.listName),
    items: getItems(state, ownProps.listName),
    loading: isLoading(state, ownProps.listName)
}), (dispatch, ownProps) => ({
    onSelect: (newIndex) => dispatch(selectItem({ listName: ownProps.listName, newIndex: newIndex }))
}))(jss(styles)(List));
