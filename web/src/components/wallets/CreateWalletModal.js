import React, { Component } from 'react';
import { connect } from 'react-redux';
import { reduxForm, startSubmit, stopSubmit } from 'redux-form';
import strings from 'strings';
import { hideModal } from 'actions/modals';
import { createWallet } from 'actions/wallets';
import Modal from 'components/controls/Modal';
import Button from 'components/controls/Button';
import TextInput from 'components/controls/TextInput';
import LoadingIndicator from 'components/controls/LoadingIndicator';

function submit(data, dispatch) {
    dispatch(startSubmit('createWallet'));

    dispatch(createWallet(data.name, err => {
        if (err) {
            return dispatch(stopSubmit('createWallet', { _error: strings.errors.errorOccurred }));
        }
        dispatch(stopSubmit('createWallet'));
        dispatch(hideModal('createWallet'));
    }));
};

class CreateWalletForm extends Component {
    render() {
        const { fields: { name }, error, handleSubmit, submitting, onClose } = this.props;

        return <Modal name="createWallet" header={strings.wallets.createWalletModal}>
            <form id="create-wallet-form" onSubmit={handleSubmit(submit)}>
                <TextInput label={strings.wallets.walletName} field={name} />
                {error && <div>{error}</div>}
            </form>

            <Button text={strings.actions.create} type="submit" form="create-wallet-form" disabled={submitting} />
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
    form: 'createWallet',
    fields: [ 'name', ],
    validate
})(CreateWalletForm);

export default connect((state, ownProps) => ({
}), (dispatch, ownProps) => ({
    onClose: () => dispatch(hideModal('createWallet'))
}))(ReduxForm);

 
