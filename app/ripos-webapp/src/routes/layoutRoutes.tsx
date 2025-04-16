import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { lazy, ReactNode } from 'react';
import { Link } from 'react-router';

const HomePage = lazy(() => import('@pages/home/HomePage'));
const BrandsPage = lazy(() => import('@pages/brands/BrandsPage'));
const CashRegistersPage = lazy(() => import('@pages/cashRegisters/CashRegistersPage'));
const CategoriesPage = lazy(() => import('@pages/categories/CategoriesPage'));
const ColorsPage = lazy(() => import('@pages/colors/ColorsPage'));
const CustomersPage = lazy(() => import('@pages/customers/CustomersPage'));
const GendersPage = lazy(() => import('@pages/genders/GendersPage'));
const VendorsPage = lazy(() => import('@pages/vendors/VendorsPage'));
const SizesPage = lazy(() => import('@pages/sizes/SizesPage'));

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
    path: '/categorias',
    element: <CategoriesPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Categorías' }],
  },
  {
    path: '/colores',
    element: <ColorsPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Colores' }],
  },
  {
    path: '/clientes',
    element: <CustomersPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Clientes' }],
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
    path: '/proveedores',
    element: <VendorsPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Proveedores' }],
  },
  {
    path: '/tallas',
    element: <SizesPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Tallas' }],
  },
];
