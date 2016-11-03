import React, { Component } from 'react';
import { connect } from 'react-redux';
import { reduxForm, startSubmit, stopSubmit } from 'redux-form';
import { loginUserAsync } from 'actions/session';
import Button from 'components/controls/Button';
import TextInput from 'components/controls/TextInput';

function submit(data, dispatch) {
    dispatch(startSubmit('login'));
    dispatch(loginUserAsync(data.username, data.password, function(err) {
        if (err) {
            dispatch(stopSubmit('login', { _error: 'Login failed' }));
            return;
        }
        dispatch(stopSubmit('login'));
    }));
};

class LoginForm extends Component {
    render() {
        const { fields: { username, password }, error, handleSubmit, submitting } = this.props;

        return <div className="app-card-login mdl-card mdl-shadow--6dp">
            <div className="mdl-card__title mdl-color--primary mdl-color-text--white">
                <h2 className="mdl-card__title-text">Вход</h2>
           </div>
            <div className="mdl-card__supporting-text">
                <form id="login-form" onSubmit={handleSubmit(submit)}>
                    <TextInput header="Имя пользователя" field={username} />
                    <TextInput header="Пароль" type="password" field={password} />
                    {error && <div>{error}</div>}
                </form>
            </div>
            <div className="mdl-card__actions mdl-card--border">
                <Button text="Войти" type="submit" form="login-form" disabled={submitting} />
            </div>
        </div>;
    }
}

function validate(values) {
    const errors = {};
    if (!values.username || values.username.trim() === '') {
        errors.username = 'Enter a user name';
    }

    if (!values.password || values.password.trim() === '') {
        errors.password = 'Enter a password';
    }

    return errors;
}

function mapStateToProps(state, ownProps) { }

function mapDispatchToProps(dispatch, ownProps) {
    return {
        onSubmit: () => alert(1)
    }
}

export default reduxForm({
    form: 'login',
    fields: [ 'username', 'password' ],
    validate
})(LoginForm);
