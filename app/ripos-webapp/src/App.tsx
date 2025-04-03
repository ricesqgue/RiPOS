import { createBrowserRouter, RouterProvider } from 'react-router';
import AppRoutes from './routes/AppRoutes';
import { ConfigProvider } from 'antd';
import { darkTheme, lightTheme } from '@styles/themes';
import esEs from 'antd/locale/es_ES';
import { useThemeStore } from '@stores/themeStore';
import { useEffect } from 'react';
import { QueryClientProvider } from '@tanstack/react-query';
import queryClient from '@api/queryClient';

const App = () => {
  const { darkMode } = useThemeStore();
  const router = createBrowserRouter([{ path: '*', Component: AppRoutes }]);

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', darkMode ? 'dark' : 'light');
  }, [darkMode]);

  return (
    <QueryClientProvider client={queryClient}>
      <ConfigProvider theme={darkMode ? darkTheme : lightTheme} locale={esEs}>
        <RouterProvider router={router} />
      </ConfigProvider>
    </QueryClientProvider>
  );
};

export default App;
