import { post } from './core';

export function loginUser(username, password) {
    return post('/api/sessions', { username, password })
        .then(json => ({ session: json.session.sessionKey, expiresAt: json.session.expiresAt }));
};
