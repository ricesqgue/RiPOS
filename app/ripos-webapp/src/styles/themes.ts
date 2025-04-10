import { theme, ThemeConfig } from 'antd';

const commonTheme: ThemeConfig = {
  token: {
    colorPrimary: '#1c9464',
  },
  components: {
    Layout: {
      headerPadding: '10px',
    },
    Menu: {
      subMenuItemSelectedColor: 'var(--rp-primary-a10)',
      itemBg: 'var(--rp-surface-tonal-a10)',
    },
  },
};

const lightTheme: ThemeConfig = {
  algorithm: theme.defaultAlgorithm,
  token: {
    ...commonTheme.token,
    // colorBgContainer: 'var(--rp-surface-a0)',
  },
  components: {
    Layout: {
      ...commonTheme.components?.Layout,
      headerBg: 'var(--rp-surface-a30)',
      siderBg: 'var(--rp-surface-a20)',
      colorBgLayout: 'var(--rp-surface-a10)',
    },
    Menu: {
      ...commonTheme.components?.Menu,
      itemSelectedBg: 'var(--rp-primary-a30)',
      itemSelectedColor: '#000',
    },
  },
  cssVar: true,
};

const darkTheme: ThemeConfig = {
  algorithm: theme.darkAlgorithm,
  token: {
    ...commonTheme.token,
    colorBgContainer: 'var(--rp-surface-a10)',
  },
  components: {
    Layout: {
      ...commonTheme.components?.Layout,
      headerBg: 'var(--rp-surface-a0)',
      siderBg: 'var(--rp-surface-a10)',
      colorBgLayout: 'var(--rp-surface-a0)',
    },
    Menu: {
      ...commonTheme.components?.Menu,
      itemSelectedBg: 'var(--rp-primary-a10)',
      itemSelectedColor: '#FFF',
    },
  },
  cssVar: true,
};

export { lightTheme, darkTheme };
