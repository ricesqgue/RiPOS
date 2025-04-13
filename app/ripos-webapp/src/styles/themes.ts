import { theme, ThemeConfig } from 'antd';

const commonTheme: ThemeConfig = {
  token: {
    colorPrimary: '#1c9464',
  },
  components: {
    Layout: {
      headerPadding: '10px',
      triggerHeight: 40,
    },
    Menu: {
      subMenuItemSelectedColor: 'var(--rp-primary-a10)',
      itemBg: 'var(--rp-surface-tonal-a10)',
      itemBorderRadius: 0,
      itemMarginBlock: 0,
      itemMarginInline: 0,
    },
  },
};

const lightTheme: ThemeConfig = {
  algorithm: theme.defaultAlgorithm,
  token: {
    ...commonTheme.token,
  },
  components: {
    Layout: {
      ...commonTheme.components?.Layout,
      headerBg: 'var(--rp-surface-tonal-a40)',
      siderBg: 'var(--rp-surface-tonal-a40)',
      colorBgLayout: 'var(--rp-surface-a10)',
      triggerBg: 'var(--rp-surface-tonal-a10)',
      triggerColor: '#000',
    },
    Menu: {
      ...commonTheme.components?.Menu,
      itemSelectedBg: 'var(--rp-primary-a30)',
      itemSelectedColor: '#fff',
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
      headerBg: 'var(--rp-surface-tonal-a0)',
      siderBg: 'var(--rp-surface-tonal-a0)',
      colorBgLayout: 'var(--rp-surface-a0)',
      triggerBg: 'var(--rp-surface-tonal-a10)',
    },
    Menu: {
      ...commonTheme.components?.Menu,
      itemSelectedBg: 'var(--rp-primary-a0)',
      itemSelectedColor: '#FFF',
    },
    Table: {
      headerBg: 'var(--rp-surface-tonal-a10)',
      rowHoverBg: 'var(--rp-surface-a20)',
      bodySortBg: 'var(--rp-surface-a10)',
    },
  },
  cssVar: true,
};

export { lightTheme, darkTheme };
