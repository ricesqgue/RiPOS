import { Outlet } from 'react-router-dom';
import styles from './Layout.module.scss';
import ApplicationBar from './AppBar/ApplicationBar';
import DrawerMenu from './DrawerMenu/DrawerMenu';

const Layout = () => {
  return (
    <>
      <div className={styles.appContainer}>
        <div>
          <ApplicationBar></ApplicationBar>
          <DrawerMenu></DrawerMenu>
        </div>
        <div className={styles.appMain}>
          <div className={styles.appContent}>
            <Outlet></Outlet>
          </div>
        </div>
      </div>
    </>
  );
};

export default Layout;
