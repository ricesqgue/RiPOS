import { createBrowserRouter, RouterProvider } from 'react-router';
import AppRoutes from './routes/AppRoutes';

const App = () => {
  const router = createBrowserRouter([{ path: '*', Component: AppRoutes }]);
  return <RouterProvider router={router} />;
};

export default App;
