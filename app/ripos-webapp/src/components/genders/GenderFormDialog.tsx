import { usePostApiGenders, usePutApiGendersId } from '@api/generated/gender/gender';
import { GenderRequest, GenderResponse, SimpleResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useMemo, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';

interface GenderFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editGender?: GenderResponse;
}

type FormFields = {
  name: string;
};

const mapGenderToFormFields = (gender: GenderResponse): FormFields => ({
  name: gender.name!,
});

const GenderFormDialog = (props: GenderFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addGender, isPending: isPendingAdd } = usePostApiGenders();
  const { mutateAsync: updateGender, isPending: isPendingUpdate } = usePutApiGendersId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editGender) {
        const formValues = mapGenderToFormFields(props.editGender);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editGender, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
    };

    if (props.mode === 'add') {
      const addGenderRequest: GenderRequest = {
        name: normalizedValues.name,
      };

      addGender({ data: addGenderRequest })
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

      const updateGenderRequest: GenderRequest = {
        name: normalizedValues.name,
      };

      updateGender({ id: props.editGender!.id!, data: updateGenderRequest })
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
    queryClient.invalidateQueries({ queryKey: ['/api/genders'] });
  };

  const handleOnClose = () => {
    props.onClose();
    form.resetFields();
  };

  const modalTitle = useMemo(
    () => `${props.mode === 'add' ? 'Agregar' : 'Editar'} género`,
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

export default GenderFormDialog;
