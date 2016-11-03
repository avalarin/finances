import React, { Component } from 'react';
import { connect } from 'react-redux';
import { reduxForm, startSubmit, stopSubmit } from 'redux-form';
import strings from 'strings';
import { hideModal } from 'actions/modals';
import Modal from 'components/controls/Modal';
import Button from 'components/controls/Button';
import Input from 'components/controls/Input';
import LoadingIndicator from 'components/controls/LoadingIndicator';

function submit(data, dispatch) {
    dispatch(startSubmit('createBook'));
    setTimeout(() => dispatch(stopSubmit('createBook')), 2000);
};

class LoginForm extends Component {
    render() {
        const { fields: { name }, error, handleSubmit, submitting, onClose } = this.props;

        return <Modal name="createBook" header={strings.books.createBookModal}>
            <form id="create-book-form" onSubmit={handleSubmit(submit)}>
                <Input label={strings.books.bookName} field={name} />
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
        errors.username = strings.errors.required;
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
    onClose: () => dispatch(hideModal({ modalName: 'createBook' }))
}))(ReduxForm);

 
