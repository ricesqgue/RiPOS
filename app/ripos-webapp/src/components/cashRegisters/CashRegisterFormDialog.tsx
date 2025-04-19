import {
  usePostApiCashregisters,
  usePutApiCashregistersId,
} from '@api/generated/cash-register/cash-register';
import { CashRegisterRequest, CashRegisterResponse, SimpleResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';

interface CashRegisterFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editCashRegister?: CashRegisterResponse;
}

type FormFields = {
  name: string;
};

const mapCashRegisterToFormFields = (cashRegister: CashRegisterResponse): FormFields => ({
  name: cashRegister.name!,
});

const CashRegisterFormDialog = (props: CashRegisterFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addCashRegister, isPending: isPendingAdd } = usePostApiCashregisters();
  const { mutateAsync: updateCashRegister, isPending: isPendingUpdate } =
    usePutApiCashregistersId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editCashRegister) {
        const formValues = mapCashRegisterToFormFields(props.editCashRegister);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editCashRegister, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
    };

    if (props.mode === 'add') {
      const addCashRegisterRequest: CashRegisterRequest = {
        name: normalizedValues.name,
      };

      addCashRegister({ data: addCashRegisterRequest })
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

      const updateCashRegisterRequest: CashRegisterRequest = {
        name: normalizedValues.name,
      };

      updateCashRegister({ id: props.editCashRegister!.id!, data: updateCashRegisterRequest })
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
    queryClient.invalidateQueries({ queryKey: ['/api/cashregisters'] });
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} caja registradora`}
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

export default CashRegisterFormDialog;
