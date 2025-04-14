import {
  useDeleteApiCashregistersId,
  useGetApiCashregisters,
} from '@api/generated/cash-register/cash-register';
import CashRegisterFormDialog from '@components/cashRegisters/CashRegisterFormDialog';
import { Button, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CashRegisterResponse, StringMessageResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterCashRegisters } from '@utils/filters/cashRegistersFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';

const CashRegistersPage = () => {
  const [isCashRegisterFormDialogOpen, setIsCashRegisterDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>();
  const [selectedCashRegister, setSelectedCashRegister] = useState<CashRegisterResponse>();
  const { mutateAsync: deleteCashRegister } = useDeleteApiCashregistersId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '' });

  const { data: cashRegisters, isLoading } = useGetApiCashregisters(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterCashRegisters(cashRegisters || [], clientFilters),
    [cashRegisters, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-cashRegisters-input'}
      placeholder="Buscar caja registradora"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
  ];

  const columns = useMemo<TableColumnsType<CashRegisterResponse>>(
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
        render: (val: CashRegisterResponse) => (
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

  const handleAddCashRegisterClick = () => {
    setDialogMode('add');
    setIsCashRegisterDialogOpen(true);
  };

  const handleEditClick = (cashRegister: CashRegisterResponse) => {
    setSelectedCashRegister(cashRegister);
    setDialogMode('edit');
    setIsCashRegisterDialogOpen(true);
  };

  const handleDeleteClick = (cashRegisterId: number) => {
    modal.confirm({
      title: 'Desactivar caja registradora',
      content: '¿Estás seguro de desactivar esta caja registradora?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteCashRegister({ id: cashRegisterId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/cashregisters'] });
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
            { text: 'Agregar', onClick: handleAddCashRegisterClick },
          ],
        }}
      />
      <Table<CashRegisterResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <CashRegisterFormDialog
          open={isCashRegisterFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsCashRegisterDialogOpen(false)}
          editCashRegister={selectedCashRegister}
        ></CashRegisterFormDialog>
      )}
    </>
  );
};

export default CashRegistersPage;
