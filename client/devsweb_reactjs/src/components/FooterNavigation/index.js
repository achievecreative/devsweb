import React from 'react';
import { Link, Text } from '@sitecore-jss/sitecore-jss-react';

import './FooterNavigation.scss';

const FooterNavigation = (props) => {
  const { items } = props?.fields ?? {};
  return (
    <div className={'footer-nav'}>
      {items.map((item) => {
        const { link, items } = item?.fields;
        return (
          <div key={item.id}>
            {link && <Link field={link} className="header" />}

            {items && (
              <div className="sublinks">
                {items.map((sublink) => {
                  return <Link key={sublink.id} field={sublink.fields.link} />;
                })}
              </div>
            )}
          </div>
        );
      })}
    </div>
  );
};

export default FooterNavigation;
