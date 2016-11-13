import { get, post, getBookId } from './core';

export function load() {
    return post('/api/wallet', { bookId: getBookId() }).then(r => r.wallets);
}

export function create(name) {
    return post('/api/wallet/create', { bookId: getBookId(), walletName: name });
}