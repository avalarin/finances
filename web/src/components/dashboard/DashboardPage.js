import React, { Component } from 'react';
import { connect } from 'react-redux';
import strings from 'strings';
import { loadWallets } from 'actions/wallets';
import { showModal } from 'actions/modals';
import { getItems } from 'selectors/lists';
import List from 'components/controls/List';
import Panel from 'components/controls/Panel';
import Button from 'components/controls/Button';
import PageHeader from 'components/controls/PageHeader';
import CreateWalletModal from 'components/wallets/CreateWalletModal';

class WalletItem extends Component {
    render() {
        const { item } = this.props;
        return <span>{item.name}</span>;
    }
}

class DashboardPage extends Component {
    componentDidMount() {
        this.props.onRefresh();
    }

    render() {
        const { onShowModal, onRefresh } = this.props;

        return <div>
        <PageHeader text={strings.dashboard.title} />
        <Panel header={strings.dashboard.wallets}>
            <Button onClick={() => onShowModal('createWallet')} text={strings.actions.create} />
            <Button onClick={onRefresh} text={strings.actions.refresh} />
            <Button onClick={onRefresh} text="Тест" />

            <List component={WalletItem} listName="wallets" emptyMessage={strings.wallets.noWallets} />
        </Panel>

        <CreateWalletModal />
    </div>;
    }
}

export default connect((state, ownProps) => ({
    wallets: getItems(state, 'books')
}), (dispatch, ownProps) => ({
    onRefresh: () => dispatch(loadWallets()),
    onShowModal: (modalName) => dispatch(showModal(modalName))
}))(DashboardPage);
