import { useDeleteApiCategoriesId, useGetApiCategories } from '@api/generated/category/category';
import CategoryFormDialog from '@components/categories/CategoryFormDialog';
import { Button, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CategoryResponse, SimpleResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterCategories } from '@utils/filters/categoriesFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';

const CategoriesPage = () => {
  const [isCategoryFormDialogOpen, setIsCategoryDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>();
  const [selectedCategory, setSelectedCategory] = useState<CategoryResponse>();
  const { mutateAsync: deleteCategory } = useDeleteApiCategoriesId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '' });

  const { data: categories, isLoading } = useGetApiCategories(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterCategories(categories || [], clientFilters),
    [categories, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-categories-input'}
      placeholder="Buscar categoría"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
  ];

  const columns = useMemo<TableColumnsType<CategoryResponse>>(
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
        render: (val: CategoryResponse) => (
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

  const handleAddCategoryClick = () => {
    setDialogMode('add');
    setIsCategoryDialogOpen(true);
  };

  const handleEditClick = (category: CategoryResponse) => {
    setSelectedCategory(category);
    setDialogMode('edit');
    setIsCategoryDialogOpen(true);
  };

  const handleDeleteClick = (categoryId: number) => {
    modal.confirm({
      title: 'Desactivar categoría',
      content: '¿Estás seguro de desactivar esta categoría?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteCategory({ id: categoryId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/categories'] });
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
            { text: 'Agregar', onClick: handleAddCategoryClick },
          ],
        }}
      />
      <Table<CategoryResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <CategoryFormDialog
          open={isCategoryFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsCategoryDialogOpen(false)}
          editCategory={selectedCategory}
        ></CategoryFormDialog>
      )}
    </>
  );
};

export default CategoriesPage;
