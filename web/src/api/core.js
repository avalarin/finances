import 'whatwg-fetch';
import { Promise } from 'es6-promise';

function getSessionId() {
    var str = localStorage.getItem('session');
    if (!str) return null;
    var obj = JSON.parse(str);
    return obj.session;
}

function getBookId() {

}

function headers() {
    return {
        'Accept': 'application/json',
        'Content-Type': 'application/json;charset=UTF-8',
        'X-Auth': getSessionId()
    };
}

export function get(url) {
    return new Promise((resolve, revert) => {
        fetch(window.settings.apiAddress + url, {
            method: 'GET',
            mode: 'cors',
            headers: headers()
        }).then(resp => resp.json()).then(resolve).catch(revert);
    });
};


export function post(url, data) {
    return new Promise((resolve, revert) => {
        fetch(window.settings.apiAddress + url, {
            method: 'POST',
            mode: 'cors',
            headers: headers(),
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
