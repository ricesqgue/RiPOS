import { useDeleteApiColorsId, useGetApiColors } from '@api/generated/color/color';
import ColorFormDialog from '@components/colors/ColorFormDialog';
import { Button, Flex, Input, message, Modal, Space, Table } from 'antd';
import { useMemo, useState } from 'react';
import type { TableColumnsType } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPenToSquare, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { ColorResponse, StringMessageResponse } from '@api/generated/models';
import { AxiosResponse } from 'axios';
import { useQueryClient } from '@tanstack/react-query';
import useTableFilters from '@hooks/useTableFilters';
import { filterColors } from '@utils/filters/colorsFilters';
import TableToolbar from '@components/shared/tableToolbar/TableToolbar';

const ColorsPage = () => {
  const [isColorFormDialogOpen, setIsColorDialogOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>();
  const [selectedColor, setSelectedColor] = useState<ColorResponse>();
  const { mutateAsync: deleteColor } = useDeleteApiColorsId();
  const [messageApi, contextHolder] = message.useMessage();
  const [modal, modalContextHolder] = Modal.useModal();
  const queryClient = useQueryClient();

  const { serverFilters, clientFilters, setServerFilters, setClientFilters, resetAllFilters } =
    useTableFilters({ includeInactives: false }, { searchName: '' });

  const { data: colors, isLoading } = useGetApiColors(serverFilters, {
    query: {
      select: (response) => response.data,
    },
  });

  const filteredData = useMemo(
    () => filterColors(colors || [], clientFilters),
    [colors, clientFilters]
  );

  const clientFilterComponents = [
    <Input
      style={{ width: 250 }}
      allowClear
      addonBefore={<FontAwesomeIcon icon={faSearch} />}
      key={'search-colors-input'}
      placeholder="Buscar color"
      value={clientFilters.searchName}
      onChange={(v) => setClientFilters({ searchName: v.target.value })}
    />,
  ];

  const columns = useMemo<TableColumnsType<ColorResponse>>(
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
        title: 'Código RGB',
        dataIndex: 'rgbHex',
        key: 'rgbHex',
        sorter: (a, b) => a.name!.localeCompare(b.name!),
        sortDirections: ['ascend', 'descend'],
        showSorterTooltip: false,
        render: (rgbHex: string) => (
          <Flex align="center" gap={8}>
            <div
              style={{
                width: '30px',
                height: '30px',
                borderRadius: '100px',
                backgroundColor: rgbHex,
              }}
            ></div>
            {rgbHex}
          </Flex>
        ),
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
        render: (val: ColorResponse) => (
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

  const handleAddColorClick = () => {
    setDialogMode('add');
    setIsColorDialogOpen(true);
  };

  const handleEditClick = (color: ColorResponse) => {
    setSelectedColor(color);
    setDialogMode('edit');
    setIsColorDialogOpen(true);
  };

  const handleDeleteClick = (colorId: number) => {
    modal.confirm({
      title: 'Desactivar color',
      content: '¿Estás seguro de desactivar este color?',
      closable: true,
      okType: 'danger',
      onOk() {
        return deleteColor({ id: colorId })
          .then((response) => {
            messageApi.open({
              type: 'success',
              content: response.data.message,
            });
            queryClient.invalidateQueries({ queryKey: ['/api/colors'] });
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
            { text: 'Agregar', onClick: handleAddColorClick },
          ],
        }}
      />
      <Table<ColorResponse>
        columns={columns}
        loading={isLoading}
        dataSource={filteredData}
        rowKey="id"
        sticky
        size="middle"
        className="custom-ant-table"
      ></Table>
      {dialogMode && (
        <ColorFormDialog
          open={isColorFormDialogOpen}
          mode={dialogMode}
          onClose={() => setIsColorDialogOpen(false)}
          editColor={selectedColor}
        ></ColorFormDialog>
      )}
    </>
  );
};

export default ColorsPage;
