import { usePostApiCategories, usePutApiCategoriesId } from '@api/generated/category/category';
import { CategoryRequest, CategoryResponse, SimpleResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef } from 'react';
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
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
    };

    if (props.mode === 'add') {
      const addCategoryRequest: CategoryRequest = {
        name: normalizedValues.name,
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

      const updateCategoryRequest: CategoryRequest = {
        name: normalizedValues.name,
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
    queryClient.invalidateQueries({ queryKey: ['/api/categories'] });
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} categoría`}
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

export default CategoryFormDialog;
