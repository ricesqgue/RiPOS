import { Breadcrumb } from 'antd';
import styles from './pageBreadcrumb.module.scss';
import { useBreadcrumbStore } from '@stores/breadcrumbStore';

const PageBreadcrumb = () => {
  const { breadcrumbItems } = useBreadcrumbStore();

  return <Breadcrumb className={styles.breadcrumb} items={breadcrumbItems} />;
};

export default PageBreadcrumb;
