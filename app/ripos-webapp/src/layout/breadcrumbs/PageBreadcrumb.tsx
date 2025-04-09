import { Breadcrumb } from 'antd';
import styles from './pageBreadcrumb.module.scss';
import { useBreadcrumbStore } from '@stores/breadcrumbStore';
import { Link, matchPath, useLocation } from 'react-router';
import { useEffect } from 'react';
import { layoutRoutes } from '@routes/layoutRoutes';

const PageBreadcrumb = () => {
  const { breadcrumbItems, setBreadcrumbItems } = useBreadcrumbStore();
  const location = useLocation();

  useEffect(() => {
    const matchingRoute = layoutRoutes.find((layoutRoutes) => {
      return matchPath(layoutRoutes.path, location.pathname);
    });

    if (matchingRoute) {
      setBreadcrumbItems(matchingRoute.breadcrumb);
    } else {
      setBreadcrumbItems([{ title: <Link to="/">Inicio</Link> }]);
    }
  }, [location.pathname, setBreadcrumbItems]);

  return <Breadcrumb className={styles.breadcrumb} items={breadcrumbItems} />;
};

export default PageBreadcrumb;
