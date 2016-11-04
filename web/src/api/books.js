import { get, post } from './core';

export function load() {
    return get('/api/book');
}

export function create(name) {
    return post('/api/book/create', { name: name });
}