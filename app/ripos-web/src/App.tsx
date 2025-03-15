import { QueryClientProvider } from '@tanstack/react-query';
import AppRouter from './AppRouter';
import queryClient from './services/queryClient';
import AppContextProvider from './state/AppContextProvider';
import { ThemeProvider } from '@emotion/react';
import theme from '@styles/MuiTheme';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <AppContextProvider>
          <AppRouter></AppRouter>
        </AppContextProvider>
      </ThemeProvider>
      <ReactQueryDevtools
        position={'bottom'}
        buttonPosition="bottom-left"
        initialIsOpen={false}
      ></ReactQueryDevtools>
    </QueryClientProvider>
  );
};

export default App;
