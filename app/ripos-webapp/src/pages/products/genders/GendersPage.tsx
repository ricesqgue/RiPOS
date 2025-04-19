import { useDeleteApiGendersId, useGetApiGenders } from '@api/generated/gender/gender';
import GenderFormDialog from '@components/genders/GenderFormDialog';
import { Button, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { GenderResponse, StringMessageResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterGenders } from '@utils/filters/gendersFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';

const GendersPage = () => {
  const [isGenderFormDialogOpen, setIsGenderDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>();
  const [selectedGender, setSelectedGender] = useState<GenderResponse>();
  const { mutateAsync: deleteGender } = useDeleteApiGendersId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '' });

  const { data: genders, isLoading } = useGetApiGenders(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterGenders(genders || [], clientFilters),
    [genders, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-genders-input'}
      placeholder="Buscar género"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
  ];

  const columns = useMemo<TableColumnsType<GenderResponse>>(
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
        render: (val: GenderResponse) => (
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

  const handleAddGenderClick = () => {
    setDialogMode('add');
    setIsGenderDialogOpen(true);
  };

  const handleEditClick = (gender: GenderResponse) => {
    setSelectedGender(gender);
    setDialogMode('edit');
    setIsGenderDialogOpen(true);
  };

  const handleDeleteClick = (genderId: number) => {
    modal.confirm({
      title: 'Desactivar género',
      content: '¿Estás seguro de desactivar este género?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteGender({ id: genderId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/genders'] });
          })
          .catch((err: AxiosResponse<StringMessageResponse>) => {
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
            { text: 'Agregar', onClick: handleAddGenderClick },
          ],
        }}
      />
      <Table<GenderResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <GenderFormDialog
          open={isGenderFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsGenderDialogOpen(false)}
          editGender={selectedGender}
        ></GenderFormDialog>
      )}
    </>
  );
};

export default GendersPage;
