import React from 'react';
import strings from 'strings';
import PageHeader from 'components/controls/PageHeader';

export default () => (
    <div>
        <PageHeader text={strings.dashboard.title} />
        <span>Simple application</span>
    </div>
);
