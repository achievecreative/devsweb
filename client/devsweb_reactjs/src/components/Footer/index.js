import React from 'react';
import { Placeholder, Text } from '@sitecore-jss/sitecore-jss-react';

const Footer = (props) => {
  return (
    <div className="container-fluid">
      <div className="row">
        <div className="col-8">
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
