import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from 'react-router-dom';
import TestPage from './pages/TestPage';
import Layout from './components/Layout/Layout';

const AppRouter = () => {
  const router = createBrowserRouter(
    createRoutesFromElements(
      <>
        <Route element={<Layout />}>
          <Route path="/" element={<TestPage />}></Route>
        </Route>
      </>,
    ),
  );

  return <RouterProvider router={router}></RouterProvider>;
};

export default AppRouter;
