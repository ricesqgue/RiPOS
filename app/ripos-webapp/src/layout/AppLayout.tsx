import { Divider, Layout } from 'antd';
import { Outlet } from 'react-router';
import UserInfo from './header/userInfo/UserInfo';
import styles from './appLayout.module.scss';
import StoreInfo from './header/storeInfo/StoreInfo';
import HeaderOptions from './header/options/HeaderOptions';
import PageBreadcrumb from './breadcrumbs/PageBreadcrumb';
import { Suspense } from 'react';
import Loading from '@components/shared/Loading';

const AppLayout = () => {
  const { Header, Content, Sider } = Layout;

  return (
    <Layout className={styles.appContainer}>
      <Header className={styles.header}>
        <UserInfo />
        <Divider type="vertical" />
        <StoreInfo />
        <Divider type="vertical" />
        <HeaderOptions />
      </Header>
      <Layout>
        <Sider></Sider>
        <Layout className={styles.appContentContainer}>
          <Suspense fallback={<Loading fullscreen size="large" />}>
            <PageBreadcrumb />
            <Content className={styles.appContent}>
              <Outlet />
            </Content>
          </Suspense>
        </Layout>
      </Layout>
    </Layout>
  );
};

export default AppLayout;
