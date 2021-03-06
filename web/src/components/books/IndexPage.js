import React, { Component } from 'react';
import { connect } from 'react-redux';
import strings from 'strings';
import { selectBook } from 'actions/session';
import { loadBooks } from 'actions/books';
import { showModal } from 'actions/modals';
import { getSelected, hasSelection, isLoading, getItems } from 'selectors/lists';
import BookItem from './BookItem';
import CreateBookModal from './CreateBookModal';
import Button from 'components/controls/Button';
import List from 'components/controls/List';
import Panel from 'components/controls/Panel';
import PageHeader from 'components/controls/PageHeader';

class IndexPage extends Component {
    componentDidMount() {
        this.props.onRefresh();
    }

    render() {
        var { selected, isSelected, currentBook, books,
              onRefresh, onSelectBook, onShowModal } = this.props;
        var book = books.filter(b => b.id == currentBook)[0];

        var currentBookContent;
        if (!currentBook || !book) {
            currentBookContent = <div>{strings.books.notSelected}</div>;
        } else {
            currentBookContent = <div>{`Book #${book.id}`}</div>;
        }

        return <div>
            <PageHeader text={strings.books.books} />
            <Panel header={strings.books.currentBook}>
                {currentBookContent}
            </Panel>
            <Panel header={strings.books.allBooks}>
                <Button onClick={() => onShowModal('createBook')} text={strings.actions.create} />
                <Button onClick={onRefresh} text={strings.actions.refresh} />
                { isSelected && <Button text={strings.books.useBook} onClick={() => onSelectBook(books[selected].id)} /> }
                <List component={BookItem} listName="books" emptyMessage={strings.books.noBooks} />
            </Panel>

            <CreateBookModal />
        </div>;
    }
}

export default connect((state, ownProps) => ({
    currentBook: state.session.get('book'),
    books: getItems(state, 'books'),
    selected: getSelected(state, 'books'),
    isSelected: hasSelection(state, 'books')
}), (dispatch, ownProps) => ({
    onRefresh: () => dispatch(loadBooks()),
    onSelectBook: (id) => dispatch(selectBook(id)),
    onShowModal: (modalName) => dispatch(showModal(modalName)) 
}))(IndexPage);
