import styles from './appLogo.module.scss';
import RiposLogo from '@assets/logoWithName.svg?react';

interface LogoProps {
  collapsed: boolean;
}

const AppLogo = (props: LogoProps) => {
  return (
    <div className={styles.container}>
      <div
        className={styles.logoContainer}
        style={{
          width: props.collapsed ? '42px' : '120px',
          marginLeft: props.collapsed ? '10px' : '3px',
        }}
      >
        <RiposLogo className={styles.logo}></RiposLogo>
      </div>
    </div>
  );
};

export default AppLogo;
