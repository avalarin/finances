import React from 'react';
import {Route} from 'react-router';
import App from 'components/App';
import Login from 'pages/Login';

module.exports = <Route component={App}>
  <Route path="/login" component={Login} />
</Route>;
