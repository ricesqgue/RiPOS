import { usePostApiBrands, usePutApiBrandsId } from '@api/generated/brand/brand';
import { BrandRequest, BrandResponse, SimpleResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useMemo, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';

interface BrandFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editBrand?: BrandResponse;
}

type FormFields = {
  name: string;
};

const mapBrandToFormFields = (brand: BrandResponse): FormFields => ({
  name: brand.name!,
});

const BrandFormDialog = (props: BrandFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addBrand, isPending: isPendingAdd } = usePostApiBrands();
  const { mutateAsync: updateBrand, isPending: isPendingUpdate } = usePutApiBrandsId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editBrand) {
        const formValues = mapBrandToFormFields(props.editBrand);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editBrand, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
    };

    if (props.mode === 'add') {
      const addBrandRequest: BrandRequest = {
        name: normalizedValues.name,
      };

      addBrand({ data: addBrandRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<SimpleResponse>) => {
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

      const updateBrandRequest: BrandRequest = {
        name: normalizedValues.name,
      };

      updateBrand({ id: props.editBrand!.id!, data: updateBrandRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<SimpleResponse>) => {
          console.log(err);
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    }
  };

  const invalidateQueries = () => {
    queryClient.invalidateQueries({ queryKey: ['/api/brands'] });
  };

  const handleOnClose = () => {
    props.onClose();
    form.resetFields();
  };

  const modalTitle = useMemo(
    () => `${props.mode === 'add' ? 'Agregar' : 'Editar'} marca`,
    [props.mode]
  );

  return (
    <>
      {contextHolder}
      <Modal
        open={props.open}
        title={modalTitle}
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
        </Form>
      </Modal>
    </>
  );
};

export default BrandFormDialog;
