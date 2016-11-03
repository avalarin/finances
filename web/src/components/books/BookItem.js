import React from 'react';
import { connect } from 'react-redux';

const BookItem = ({ item }) => (
    <div>
        <span>Book #{item.id}</span>
    </div>
);

export default connect((state, ownProps) => ({
    item: ownProps.item
}), (dispatch, ownProps) => ({
}))(BookItem);
