import { useDeleteApiBrandsId, useGetApiBrands } from '@api/generated/brand/brand';
import BrandFormDialog from '@components/brands/BrandFormDialog';
import { Button, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { BrandResponse, SimpleResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterBrands } from '@utils/filters/brandsFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';

const BrandsPage = () => {
  const [isBrandFormDialogOpen, setIsBrandDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>();
  const [selectedBrand, setSelectedBrand] = useState<BrandResponse>();
  const { mutateAsync: deleteBrand } = useDeleteApiBrandsId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '' });

  const { data: brands, isLoading } = useGetApiBrands(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterBrands(brands || [], clientFilters),
    [brands, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-brands-input'}
      placeholder="Buscar marca"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
  ];

  const columns = useMemo<TableColumnsType<BrandResponse>>(
    () => [
      {
        title: 'Nombre',
        dataIndex: 'name',
        key: 'name',
        sorter: (a, b) => a.name!.localeCompare(b.name!),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
      },
      {
        title: 'Estatus',
        dataIndex: 'isActive',
        key: 'isActive',
        hidden: !serverFilters.includeInactives,
        render: (isActive: boolean) => (isActive ? 'Activo' : 'Desactivado'),
        sorter: (a, b) => (a.isActive === b.isActive ? 0 : a.isActive ? -1 : 1),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
      },
      {
        title: '',
        dataIndex: '',
        key: 'actions',
        align: 'end',
        render: (val: BrandResponse) => (
          <Space>
            <Button
              type="text"
              icon={<FontAwesomeIcon icon={faPenToSquare} />}
              size="middle"
              shape="circle"
              disabled={!val.isActive}
              onClick={() => handleEditClick(val)}
            ></Button>
            <Button
              type="text"
              icon={<FontAwesomeIcon icon={faTrashAlt} />}
              size="middle"
              shape="circle"
              disabled={!val.isActive}
              onClick={() => handleDeleteClick(val.id!)}
            ></Button>
          </Space>
        ),
      },
    ],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [serverFilters.includeInactives]
  );

  const handleAddBrandClick = () => {
    setDialogMode('add');
    setIsBrandDialogOpen(true);
  };

  const handleEditClick = (brand: BrandResponse) => {
    setSelectedBrand(brand);
    setDialogMode('edit');
    setIsBrandDialogOpen(true);
  };

  const handleDeleteClick = (brandId: number) => {
    modal.confirm({
      title: 'Desactivar marca',
      content: '¿Estás seguro de desactivar esta marca?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteBrand({ id: brandId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/brands'] });
          })
          .catch((err: AxiosResponse<SimpleResponse>) => {
            messageApi.open({
              type: 'error',
              content: err.data.message,
            });
          });
      },
    });
  };

  return (
    <>
      {contextHolder}
      {modalContextHolder}
      <TableToolbar
        filters={[{ components: clientFilterComponents }]}
        options={{
          includeInactivesSwitch: {
            value: serverFilters.includeInactives,
            onChange: (v) => setServerFilters({ includeInactives: v }),
          },
          buttons: [
            { text: 'Reiniciar filtros', onClick: resetAllFilters },
            { text: 'Agregar', onClick: handleAddBrandClick },
          ],
        }}
      />
      <Table<BrandResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <BrandFormDialog
          open={isBrandFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsBrandDialogOpen(false)}
          editBrand={selectedBrand}
        ></BrandFormDialog>
      )}
    </>
  );
};

export default BrandsPage;
