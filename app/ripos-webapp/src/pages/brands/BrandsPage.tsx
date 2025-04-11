import { useGetApiBrands } from '@api/generated/brand/brand';
import { Table } from 'antd';
import { useEffect } from 'react';

const BrandsPage = () => {
  const { data: brands } = useGetApiBrands(
    { includeInactives: false },
    {
      query: {
        select: (response) => response.data,
      },
    }
  );

  useEffect(() => {
    if (brands) console.log(brands);
  }, [brands]);

  const columns = [
    {
      title: 'Nombre',
      key: 'name',
      dataIndex: 'name',
    },
    {
      title: 'Dirección',
      key: 'address',
      dataIndex: 'address',
    },
    {
      title: 'Tel',
      key: 'name',
      dataIndex: 'name',
    },
    {
      title: 'Nombre',
      key: 'name',
      dataIndex: 'name',
    },
  ];
  return (
    <>
      <Table columns={columns}></Table>
    </>
  );
};

export default BrandsPage;
