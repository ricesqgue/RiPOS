import { lazy } from 'react';

const HomePage = lazy(() => import('@pages/home/HomePage'));
const BrandsPage = lazy(() => import('@pages/brands/BrandsPage'));

export const layoutRoutes = [
  {
    path: '/',
    element: <HomePage />,
    breadcrumb: 'Inicio',
  },
  {
    path: '/marcas',
    element: <BrandsPage />,
    breadcrumb: 'Marcas',
  },
];
