import { Divider, Layout } from 'antd';
import { Outlet } from 'react-router';
import UserInfo from './header/userInfo/UserInfo';
import styles from './appLayout.module.scss';
import StoreInfo from './header/storeInfo/StoreInfo';
import HeaderOptions from './header/options/HeaderOptions';
import PageBreadcrumb from './breadcrumbs/PageBreadcrumb';
import { Suspense } from 'react';
import Loading from '@components/shared/Loading';
import AppMenu from './sider/AppMenu';

const AppLayout = () => {
  const { Header, Content, Sider } = Layout;
  return (
    <Layout className={styles.appContainer}>
      <Sider>
        <div style={{ height: '64px' }}>Logo</div>
        <AppMenu></AppMenu>
      </Sider>
      <Layout>
        <Header className={styles.header}>
          <UserInfo />
          <Divider type="vertical" />
          <StoreInfo />
          <Divider type="vertical" />
          <HeaderOptions />
        </Header>
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
