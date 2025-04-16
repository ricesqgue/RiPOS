import { usePostApiSizes, usePutApiSizesId } from '@api/generated/size/size';
import { SizeRequest, SizeResponse, SizeResponseMessageResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';

interface SizeFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editSize?: SizeResponse;
}

type FormFields = {
  name: string;
  shortName: string;
};

const mapSizeToFormFields = (size: SizeResponse): FormFields => ({
  name: size.name!,
  shortName: size.shortName!,
});

const SizeFormDialog = (props: SizeFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addSize, isPending: isPendingAdd } = usePostApiSizes();
  const { mutateAsync: updateSize, isPending: isPendingUpdate } = usePutApiSizesId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editSize) {
        const formValues = mapSizeToFormFields(props.editSize);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editSize, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
      shortName: values.shortName.trim(),
    };

    if (props.mode === 'add') {
      const addSizeRequest: SizeRequest = {
        name: normalizedValues.name,
        shortName: normalizedValues.shortName,
      };

      addSize({ data: addSizeRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<SizeResponseMessageResponse>) => {
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    } else if (props.mode === 'edit') {
      const modified = !isEqual(normalizedValues, initialValuesRef.current);
      if (!modified) {
        messageApi.open({
          type: 'info',
          content: 'No hay cambios que guardar',
        });
        return;
      }

      const updateSizeRequest: SizeRequest = {
        name: normalizedValues.name,
        shortName: normalizedValues.shortName,
      };

      updateSize({ id: props.editSize!.id!, data: updateSizeRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<SizeResponseMessageResponse>) => {
          console.log(err);
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    }
  };

  const invalidateQueries = () => {
    queryClient.invalidateQueries({ queryKey: ['/api/sizes'] });
  };

  const handleOnClose = () => {
    props.onClose();
    form.resetFields();
  };

  return (
    <>
      {contextHolder}
      <Modal
        open={props.open}
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} talla`}
        confirmLoading={isPendingAdd || isPendingUpdate}
        onCancel={handleOnClose}
        onClose={handleOnClose}
        maskClosable={false}
        onOk={form.submit}
        okText="Guardar"
      >
        <Form
          form={form}
          onFinish={handleSubmitForm}
          layout="vertical"
          autoComplete="off"
          size="middle"
          variant="filled"
        >
          <Form.Item<FormFields>
            label="Nombre"
            name="name"
            rules={[
              { required: true, message: 'Campo requerido' },
              { max: 50, message: 'Máximo 50 caracteres' },
            ]}
          >
            <Input maxLength={50}></Input>
          </Form.Item>
          <Form.Item<FormFields>
            label="Nombre corto"
            name="shortName"
            rules={[
              { required: true, message: 'Campo requerido' },
              { max: 10, message: 'Máximo 10 caracteres' },
            ]}
          >
            <Input maxLength={10}></Input>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default SizeFormDialog;
