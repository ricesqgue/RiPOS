import {
  usePostApiCashregisters,
  usePutApiCashregistersId,
} from '@api/generated/cash-register/cash-register';
import {
  CashRegisterRequest,
  CashRegisterResponse,
  CashRegisterResponseMessageResponse,
} from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Form, Input, message, Modal } from 'antd';
import { useEffect, useRef, useState } from 'react';
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
  const [isFormValid, setIsFormValid] = useState(false);
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
    if (props.mode === 'add') {
      const addCashRegisterRequest: CashRegisterRequest = {
        name: values.name!,
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
        .catch((err: AxiosError<CashRegisterResponseMessageResponse>) => {
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    } else if (props.mode === 'edit') {
      const updateCashRegisterRequest: CashRegisterRequest = {
        name: form.getFieldValue('name'),
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
        .catch((err: AxiosError<CashRegisterResponseMessageResponse>) => {
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} caja registradora`}
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

export default CashRegisterFormDialog;
