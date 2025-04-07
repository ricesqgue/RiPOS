import { theme, ThemeConfig } from 'antd';

const commonTheme: ThemeConfig = {
  token: {
    colorPrimary: '#1c9464',
  },
  components: {
    Layout: {
      headerBg: '#1c9464',
      headerColor: '#ffffff',
      headerPadding: '10px',
      siderBg: '#242424',
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
    },
  },
  cssVar: true,
};

const darkTheme: ThemeConfig = {
  algorithm: theme.darkAlgorithm,
  token: {
    ...commonTheme.token,
  },
  components: {
    Layout: {
      ...commonTheme.components?.Layout,
    },
  },
  cssVar: true,
};

export { lightTheme, darkTheme };
