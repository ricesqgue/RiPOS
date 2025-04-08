import { useBreadcrumbStore } from '@stores/breadcrumbStore';
import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { useEffect } from 'react';
import { Link } from 'react-router';

const pageBreadcrumb: ItemType[] = [
  {
    title: <Link to="/">Inicio</Link>,
  },
  {
    title: 'Marcas',
  },
];

const BrandsPage = () => {
  const { setBreadcrumbItems } = useBreadcrumbStore();

  useEffect(() => {
    setBreadcrumbItems(pageBreadcrumb);
  }, [setBreadcrumbItems]);

  return <h1>brands page</h1>;
};

export default BrandsPage;
