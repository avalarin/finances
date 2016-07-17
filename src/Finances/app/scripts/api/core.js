import 'whatwg-fetch';
import { Promise } from 'es6-promise';

export function post(url, data) {
    return new Promise((resolve, revert) => {
        fetch(window.apiAddress + url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
            .then(resp => resp.json())
            .then(json => {
                if (json.status !== 'Success') {
                    revert(json.status);
                }
                resolve(json);
            })
            .catch(error => revert(error));
    });
};

