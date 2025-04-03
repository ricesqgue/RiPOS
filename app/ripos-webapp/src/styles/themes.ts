import { theme, ThemeConfig } from 'antd';

const lightTheme: ThemeConfig = {
  algorithm: theme.defaultAlgorithm,
  token: {
    // TODO: Definir colores personalizados
  },
  cssVar: true,
};

const darkTheme: ThemeConfig = {
  algorithm: theme.darkAlgorithm,
  token: {
    // TODO: Definir colores personalizados
  },
  cssVar: true,
};

export { lightTheme, darkTheme };
