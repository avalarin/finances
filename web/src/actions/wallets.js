import { startItemsLoading, finishItemsLoading } from 'actions/lists';
import api from 'api/index';

export function loadWallets() {
    return dispatch => {
        dispatch(startItemsLoading({ listName: 'wallets' }));
        api.wallets.load()
            .then(wallets => dispatch(finishItemsLoading({ listName: 'wallets', items: wallets })));
    };
};

export function createWallet(name, callback) {
    return dispatch => {
        api.wallets.create(name)
            .then(resp => {
                dispatch(loadWallets());
                callback();
            })
            .catch(callback);
    };
};