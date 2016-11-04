import 'whatwg-fetch';
import { Promise } from 'es6-promise';

function getSession() {
    var str = localStorage.getItem('session');
    if (!str) return null;
    return JSON.parse(str);
}

function getSessionId() {
    var session = getSession();
    if (!session) return null;
    return session.session;
}

function headers() {
    return {
        'Accept': 'application/json',
        'Content-Type': 'application/json;charset=UTF-8',
        'X-Auth': getSessionId()
    };
}

function checkResponse(resp) {
    if (resp.status && resp.status !== 'Success') {
        throw Error('Request executed with error ' + resp.status);
    }
    return resp;
}

export function getBookId() {
    var session = getSession();
    if (!session || !session.book) {
        throw Error("book is required");
    }
    return session.book;
}

export function get(url) {
    return fetch(window.settings.apiAddress + url, {
        method: 'GET',
        mode: 'cors',
        headers: headers()
    }).then(resp => resp.json())
       .then(resp => checkResponse(resp));
};

export function post(url, data) {
    return fetch(window.settings.apiAddress + url, {
        method: 'POST',
        mode: 'cors',
        headers: headers(),
        body: JSON.stringify(data)
    }).then(resp => resp.json())
        .then(checkResponse);
};
