import { usePostApiBrands, usePutApiBrandsId } from '@api/generated/brand/brand';
import { BrandRequest, BrandResponse, BrandResponseMessageResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef, useState } from 'react';
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
  const [isFormValid, setIsFormValid] = useState(false);
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
    if (props.mode === 'add') {
      const addBrandRequest: BrandRequest = {
        name: values.name!,
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
        .catch((err: AxiosError<BrandResponseMessageResponse>) => {
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    } else if (props.mode === 'edit') {
      const updateBrandRequest: BrandRequest = {
        name: form.getFieldValue('name'),
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
        .catch((err: AxiosError<BrandResponseMessageResponse>) => {
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
    setIsFormValid(false);
  };

  const handleOnFieldsChange = () => {
    const hasErrors = form.getFieldsError().some((field) => field.errors.length > 0);

    if (hasErrors) {
      setIsFormValid(false);
      return;
    }

    if (props.mode === 'edit') {
      const currentValues = form.getFieldsValue();
      const normalizedValues: FormFields = { name: currentValues.name.trim() };
      const modified = !isEqual(normalizedValues, initialValuesRef.current);
      setIsFormValid(modified);
      return;
    }

    setIsFormValid(true);
  };

  return (
    <>
      {contextHolder}
      <Modal
        open={props.open}
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} marca`}
        confirmLoading={isPendingAdd || isPendingUpdate}
        onCancel={handleOnClose}
        onClose={handleOnClose}
        maskClosable={false}
        onOk={form.submit}
        okButtonProps={{ disabled: !isFormValid }}
        okText="Guardar"
      >
        <Form
          form={form}
          onFinish={handleSubmitForm}
          layout="vertical"
          autoComplete="off"
          size="middle"
          variant="filled"
          onFieldsChange={handleOnFieldsChange}
        >
          <Form.Item<FormFields> label="Nombre" name="name" rules={[{ required: true }]}>
            <Input></Input>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default BrandFormDialog;
