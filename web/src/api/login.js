import { post } from './core';

export function loginUser(username, password) {
    return post('/api/sessions', { username, password })
        .then(json => ({ session: json.sessionKey, expiresAt: json.expiresAt }));
};
