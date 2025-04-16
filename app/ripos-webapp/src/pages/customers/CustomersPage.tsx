import { useDeleteApiCustomersId, useGetApiCustomers } from '@api/generated/customer/customer';
import CustomerFormDialog from '@components/customers/CustomerFormDialog';
import { Button, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CustomerResponse, StringMessageResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterCustomers } from '@utils/filters/customersFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';
import { useGetApiMiscCountryStates } from '@api/generated/misc/misc';
import Select, { DefaultOptionType } from 'antd/es/select';

const CustomersPage = () => {
  const [isCustomerFormDialogOpen, setIsCustomerDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit' | 'view'>();
  const [selectedCustomer, setSelectedCustomer] = useState<CustomerResponse>();
  const { mutateAsync: deleteCustomer } = useDeleteApiCustomersId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();
  const { data: countryStates } = useGetApiMiscCountryStates({
    query: {
      select: (response) => response.data,
      staleTime: 10 * 60 * 1000,
      gcTime: 11 * 60 * 1000,
    },
  });

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '', countryStateIds: [] });

  const { data: customers, isLoading } = useGetApiCustomers(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterCustomers(customers || [], clientFilters),
    [customers, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-customers-input'}
      placeholder="Buscar cliente"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
    <Select
      style={{ minWidth: 200, maxWidth: 600 }}
      allowClear
      showSearch
      optionFilterProp="label"
      mode="multiple"
      placeholder="Filtrar por estado"
      key={'state-customer-select'}
      value={clientFilters.countryStateIds}
      onChange={(v) => setClientFilters({ countryStateIds: v })}
      options={
        countryStates
          ? countryStates.map<DefaultOptionType>((c) => ({
              value: c.id,
              label: c.name!,
              shortName: c.shortName!,
            }))
          : []
      }
    ></Select>,
  ];

  const columns = useMemo<TableColumnsType<CustomerResponse>>(
    () => [
      {
        title: 'Nombre',
        dataIndex: 'name',
        key: 'name',
        sorter: (a, b) => a.name!.localeCompare(b.name!),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
        render: (_, record) =>
          `${record.name} ${record.surname} ${record.secondSurname ?? ''}`.trim(),
      },
      {
        title: 'Email',
        dataIndex: 'email',
        key: 'email',
        sorter: (a, b) => (a.email ?? '').localeCompare(b.email ?? ''),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
      },
      {
        title: 'Estado',
        dataIndex: 'countryState',
        key: 'countryState',
        sorter: (a, b) => a.countryState!.name!.localeCompare(b.countryState!.name!),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
        render: (countryState) => countryState.name,
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
        render: (val: CustomerResponse) => (
          <Space.Compact>
            <Button
              type="text"
              icon={<FontAwesomeIcon icon={faEye} />}
              size="middle"
              shape="circle"
              disabled={!val.isActive}
              onClick={() => handleViewClick(val)}
            ></Button>
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
          </Space.Compact>
        ),
      },
    ],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [serverFilters.includeInactives]
  );

  const handleAddCustomerClick = () => {
    setDialogMode('add');
    setIsCustomerDialogOpen(true);
  };

  const handleEditClick = (customer: CustomerResponse) => {
    setSelectedCustomer(customer);
    setDialogMode('edit');
    setIsCustomerDialogOpen(true);
  };

  const handleViewClick = (customer: CustomerResponse) => {
    setSelectedCustomer(customer);
    setDialogMode('view');
    setIsCustomerDialogOpen(true);
  };

  const handleDeleteClick = (customerId: number) => {
    modal.confirm({
      title: 'Desactivar cliente',
      content: '¿Estás seguro de desactivar este cliente?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteCustomer({ id: customerId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/customers'] });
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
            { text: 'Agregar', onClick: handleAddCustomerClick },
          ],
        }}
      />
      <Table<CustomerResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <CustomerFormDialog
          open={isCustomerFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsCustomerDialogOpen(false)}
          editCustomer={selectedCustomer}
        ></CustomerFormDialog>
      )}
    </>
  );
};

export default CustomersPage;
