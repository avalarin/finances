import React, { Component } from 'react';
import { connect } from 'react-redux';
import { reduxForm, startSubmit, stopSubmit } from 'redux-form';
import strings from 'strings';
import { hideModal } from 'actions/modals';
import { createBook } from 'actions/books';
import Modal from 'components/controls/Modal';
import Button from 'components/controls/Button';
import TextInput from 'components/controls/TextInput';
import LoadingIndicator from 'components/controls/LoadingIndicator';

function submit(data, dispatch) {
    dispatch(startSubmit('createBook'));

    dispatch(createBook(data.name, err => {
        if (err) {
            return dispatch(stopSubmit('createBook', { _error: strings.errors.errorOccurred }));
        }
        dispatch(stopSubmit('createBook'));
        dispatch(hideModal('createBook'));
    }));
};

class LoginForm extends Component {
    render() {
        const { fields: { name }, error, handleSubmit, submitting, onClose } = this.props;

        return <Modal name="createBook" header={strings.books.createBookModal}>
            <form id="create-book-form" onSubmit={handleSubmit(submit)}>
                <TextInput label={strings.books.bookName} field={name} />
                {error && <div>{error}</div>}
            </form>

            <Button text={strings.actions.create} type="submit" form="create-book-form" disabled={submitting} />
            <Button onClick={onClose} text={strings.actions.cancel} disabled={submitting} />
        </Modal>;
    }
}

function validate(values) {
    const errors = {};

    if (!values.name || values.name.trim() === '') {
        errors.name = strings.errors.required;
    }

    return errors;
}

const ReduxForm = reduxForm({
    form: 'createBook',
    fields: [ 'name', ],
    validate
})(LoginForm);

export default connect((state, ownProps) => ({
}), (dispatch, ownProps) => ({
    onClose: () => dispatch(hideModal('createBook'))
}))(ReduxForm);

 
