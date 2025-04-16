import { Divider, Grid, Layout } from 'antd';
import { Outlet } from 'react-router';
import UserInfo from './header/userInfo/UserInfo';
import styles from './appLayout.module.scss';
import StoreInfo from './header/storeInfo/StoreInfo';
import HeaderOptions from './header/options/HeaderOptions';
import PageBreadcrumb from './breadcrumbs/PageBreadcrumb';
import { Suspense, useEffect, useRef, useState } from 'react';
import Loading from '@components/shared/loading/Loading';
import AppMenu from './sider/AppMenu';
import Logo from './header/appLogo/AppLogo';

const { useBreakpoint } = Grid;

const AppLayout = () => {
  const { Header, Content, Sider } = Layout;
  const screens = useBreakpoint();
  const [collapsed, setCollapsed] = useState(false);
  const isAutoCollapsed = useRef(true);

  useEffect(() => {
    if (isAutoCollapsed.current) {
      setCollapsed(!screens.md);
    }
  }, [screens.md]);

  const handleManualCollapse = (value: boolean) => {
    isAutoCollapsed.current = false;
    setCollapsed(value);
  };

  return (
    <Layout className={styles.appContainer}>
      <Sider collapsible collapsed={collapsed} onCollapse={handleManualCollapse}>
        <Logo collapsed={collapsed} />
        <AppMenu collapsed={collapsed}></AppMenu>
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
