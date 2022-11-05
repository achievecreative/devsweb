import React from 'react';
import { Placeholder, Text } from '@sitecore-jss/sitecore-jss-react';
import './footer.scss';
import clsx from 'clsx';

const Footer = (props) => {
  return (
    <div className="footer">
      <div className="row">
        <div className={clsx('col-md-8', 'col-sm-12', 'footer-nav-container')}>
          <Placeholder name="jss-footer-left" rendering={props.rendering} />
        </div>
        <div className="col-4">
          <Placeholder name="jss-footer-right" rendering={props.rendering} />
        </div>
      </div>
    </div>
  );
};

export default Footer;
