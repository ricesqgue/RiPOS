import CashRegistersPage from '@pages/cashRegisters/CashRegistersPage';
import GendersPage from '@pages/genders/GendersPage';
import SizesPage from '@pages/sizes/SizesPage';
import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { lazy, ReactNode } from 'react';
import { Link } from 'react-router';

const HomePage = lazy(() => import('@pages/home/HomePage'));
const BrandsPage = lazy(() => import('@pages/brands/BrandsPage'));

interface RouteConfig {
  path: string;
  element: ReactNode;
  breadcrumb: ItemType[];
  children?: RouteConfig[];
}

export const layoutRoutes: RouteConfig[] = [
  {
    path: '/',
    element: <HomePage />,
    breadcrumb: [{ title: 'Inicio' }],
  },
  {
    path: '/cajas-registradoras',
    element: <CashRegistersPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Cajas registradoras' }],
  },
  {
    path: '/generos',
    element: <GendersPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Géneros' }],
  },
  {
    path: '/marcas',
    element: <BrandsPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Marcas' }],
  },
  {
    path: '/tallas',
    element: <SizesPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Tallas' }],
  },
];
