import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { lazy, ReactNode } from 'react';
import { Link } from 'react-router';

const POSPage = lazy(() => import('@pages/pos/POSPage'));
const BrandsPage = lazy(() => import('@pages/products/brands/BrandsPage'));
const CashRegistersPage = lazy(() => import('@pages/store/cashRegisters/CashRegistersPage'));
const CategoriesPage = lazy(() => import('@pages/products/categories/CategoriesPage'));
const ColorsPage = lazy(() => import('@pages/products/colors/ColorsPage'));
const CustomersPage = lazy(() => import('@pages/customers/CustomersPage'));
const GendersPage = lazy(() => import('@pages/products/genders/GendersPage'));
const VendorsPage = lazy(() => import('@pages/vendors/VendorsPage'));
const SizesPage = lazy(() => import('@pages/products/sizes/SizesPage'));

interface RouteConfig {
  path: string;
  element: ReactNode;
  breadcrumb: ItemType[];
  children?: RouteConfig[];
}

export const layoutRoutes: RouteConfig[] = [
  {
    path: '/',
    element: <POSPage />,
    breadcrumb: [{ title: 'Punto de venta' }],
  },
  {
    path: '/productos/categorias',
    element: <CategoriesPage />,
    breadcrumb: [{ title: 'Productos' }, { title: 'Categorías' }],
  },
  {
    path: '/productos/colores',
    element: <ColorsPage />,
    breadcrumb: [{ title: 'Productos' }, { title: 'Colores' }],
  },
  {
    path: '/productos/generos',
    element: <GendersPage />,
    breadcrumb: [{ title: 'Productos' }, { title: 'Géneros' }],
  },
  {
    path: '/productos/marcas',
    element: <BrandsPage />,
    breadcrumb: [{ title: 'Productos' }, { title: 'Marcas' }],
  },
  {
    path: '/productos/tallas',
    element: <SizesPage />,
    breadcrumb: [{ title: 'Productos' }, { title: 'Tallas' }],
  },
  {
    path: '/tienda/cajas-registradoras',
    element: <CashRegistersPage />,
    breadcrumb: [{ title: 'Tienda' }, { title: 'Cajas registradoras' }],
  },
  {
    path: '/clientes',
    element: <CustomersPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Clientes' }],
  },
  {
    path: '/proveedores',
    element: <VendorsPage />,
    breadcrumb: [{ title: <Link to={'/'}>Inicio</Link> }, { title: 'Proveedores' }],
  },
];
