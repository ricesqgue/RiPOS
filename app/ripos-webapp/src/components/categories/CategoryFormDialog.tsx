import { usePostApiCategories, usePutApiCategoriesId } from '@api/generated/category/category';
import {
  CategoryRequest,
  CategoryResponse,
  CategoryResponseMessageResponse,
} from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef, useState } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';

interface CategoryFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editCategory?: CategoryResponse;
}

type FormFields = {
  name: string;
};

const mapCategoryToFormFields = (category: CategoryResponse): FormFields => ({
  name: category.name!,
});

const CategoryFormDialog = (props: CategoryFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const [isFormValid, setIsFormValid] = useState(false);
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addCategory, isPending: isPendingAdd } = usePostApiCategories();
  const { mutateAsync: updateCategory, isPending: isPendingUpdate } = usePutApiCategoriesId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editCategory) {
        const formValues = mapCategoryToFormFields(props.editCategory);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editCategory, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    if (props.mode === 'add') {
      const addCategoryRequest: CategoryRequest = {
        name: values.name!,
      };

      addCategory({ data: addCategoryRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<CategoryResponseMessageResponse>) => {
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    } else if (props.mode === 'edit') {
      const updateCategoryRequest: CategoryRequest = {
        name: form.getFieldValue('name'),
      };

      updateCategory({ id: props.editCategory!.id!, data: updateCategoryRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<CategoryResponseMessageResponse>) => {
          console.log(err);
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    }
  };

  const invalidateQueries = () => {
    queryClient.invalidateQueries({ queryKey: ['/api/categories'] });
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} categoría`}
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

export default CategoryFormDialog;
