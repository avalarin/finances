import { get } from './core';

export function load() {
    return get('/api/book')
          .then(json => json);
};
