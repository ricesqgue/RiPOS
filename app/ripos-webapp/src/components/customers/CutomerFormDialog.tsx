import { usePostApiCustomers, usePutApiCustomersId } from '@api/generated/customer/customer';
import {
  CustomerRequest,
  CustomerResponse,
  CustomerResponseMessageResponse,
} from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Button, Col, Form, Input, message, Modal, Row, Select } from 'antd';
import { useEffect, useMemo, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';
import { useGetApiMiscCountryStates } from '@api/generated/misc/misc';
import { DefaultOptionType } from 'antd/es/select';

interface CustomerFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit' | 'view';
  onClose: () => void;
  editCustomer?: CustomerResponse;
}

type FormFields = {
  name: string;
  surname: string;
  secondSurname?: string;
  phoneNumber?: string;
  mobilePhone?: string;
  email?: string;
  address?: string;
  city?: string;
  zipCode?: string;
  rfc?: string;
  countryStateId: number;
};

const mapCustomerToFormFields = (customer: CustomerResponse): FormFields => ({
  name: customer.name!,
  surname: customer.surname!,
  secondSurname: customer.secondSurname!,
  phoneNumber: customer.phoneNumber!,
  mobilePhone: customer.mobilePhone!,
  email: customer.email!,
  address: customer.address!,
  city: customer.city!,
  zipCode: customer.zipCode!,
  rfc: customer.rfc!,
  countryStateId: customer.countryState!.id!,
});

const mapFormFieldsToCustomerRequest = (values: FormFields): CustomerRequest => ({
  name: values.name,
  surname: values.surname,
  secondSurname: values.secondSurname,
  phoneNumber: values.phoneNumber,
  mobilePhone: values.mobilePhone,
  email: values.email,
  address: values.address,
  city: values.city,
  zipCode: values.zipCode,
  rfc: values.rfc,
  countryStateId: values.countryStateId,
});

const CustomerFormDialog = (props: CustomerFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addCustomer, isPending: isPendingAdd } = usePostApiCustomers();
  const { mutateAsync: updateCustomer, isPending: isPendingUpdate } = usePutApiCustomersId();
  const [messageApi, contextHolder] = message.useMessage();
  const { data: countryStates } = useGetApiMiscCountryStates({
    query: {
      select: (response) => response.data,
      staleTime: 10 * 60 * 1000,
    },
  });

  useEffect(() => {
    if (props.open && (props.mode === 'edit' || props.mode === 'view')) {
      if (props.editCustomer) {
        const formValues = mapCustomerToFormFields(props.editCustomer);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editCustomer, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
      surname: values.surname.trim(),
      secondSurname: values.secondSurname?.trim(),
      phoneNumber: values.phoneNumber?.trim(),
      mobilePhone: values.mobilePhone?.trim(),
      email: values.email?.trim(),
      address: values.address?.trim(),
      city: values.city?.trim(),
      zipCode: values.zipCode?.trim(),
      rfc: values.rfc?.trim(),
    };

    if (props.mode === 'add') {
      const addCustomerRequest = mapFormFieldsToCustomerRequest(normalizedValues);

      addCustomer({ data: addCustomerRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<CustomerResponseMessageResponse>) => {
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

      const updateCustomerRequest = mapFormFieldsToCustomerRequest(normalizedValues);

      updateCustomer({ id: props.editCustomer!.id!, data: updateCustomerRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<CustomerResponseMessageResponse>) => {
          console.log(err);
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    }
  };

  const invalidateQueries = () => {
    queryClient.invalidateQueries({ queryKey: ['/api/customers'] });
  };

  const handleOnClose = () => {
    props.onClose();
    form.resetFields();
  };

  const modalTitle = useMemo(
    () =>
      `${props.mode === 'add' ? 'Agregar' : props.mode === 'edit' ? 'Editar' : 'Información del'} cliente`,
    [props.mode]
  );

  const modalFooter = useMemo(() => {
    switch (props.mode) {
      case 'add':
      case 'edit':
        return [
          <Button onClick={handleOnClose}>Cancelar</Button>,
          <Button htmlType="submit" type="primary" onClick={form.submit}>
            Guardar
          </Button>,
        ];
      case 'view':
        return [<Button onClick={handleOnClose}>Cerrar</Button>];
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.mode]);

  return (
    <>
      {contextHolder}
      <Modal
        open={props.open}
        title={modalTitle}
        confirmLoading={isPendingAdd || isPendingUpdate}
        onClose={handleOnClose}
        maskClosable={false}
        footer={modalFooter}
      >
        <Form
          form={form}
          onFinish={handleSubmitForm}
          layout="vertical"
          autoComplete="off"
          size="middle"
          variant="filled"
          validateTrigger={'onBlur'}
          disabled={props.mode === 'view'}
          className={props.mode === 'view' ? 'visibleDisabledForm' : ''}
        >
          <Row gutter={10}>
            <Col xs={12}>
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
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Apellido paterno"
                name="surname"
                rules={[
                  { required: true, message: 'Campo requerido' },
                  { max: 50, message: 'Máximo 50 caracteres' },
                ]}
              >
                <Input maxLength={50}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Apellido materno"
                name="secondSurname"
                rules={[{ max: 50, message: 'Máximo 50 caracteres' }]}
              >
                <Input maxLength={50}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Correo electrónico"
                name="email"
                rules={[{ type: 'email', message: 'Email no válido' }]}
              >
                <Input maxLength={100}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Número telefónico"
                name="phoneNumber"
                rules={[{ pattern: /^(?:\d{10}|\d{3}-\d{3}-\d{4})$/, message: 'Formato inválido' }]}
              >
                <Input maxLength={12}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Número celular"
                name="mobilePhone"
                rules={[{ pattern: /^(?:\d{10}|\d{3}-\d{3}-\d{4})$/, message: 'Formato inválido' }]}
              >
                <Input maxLength={12}></Input>
              </Form.Item>
            </Col>

            <Col xs={24}>
              <Form.Item<FormFields>
                label="Dirección"
                name="address"
                rules={[{ max: 400, message: 'Máximo 400 caracteres' }]}
              >
                <Input maxLength={400}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="City"
                name="city"
                rules={[{ max: 100, message: 'Máximo 100 caracteres' }]}
              >
                <Input maxLength={100}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Código postal"
                name="zipCode"
                rules={[{ pattern: /^[0-9]{5}$/, message: 'Formato inválido' }]}
              >
                <Input maxLength={10}></Input>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="Estado"
                name="countryStateId"
                rules={[{ required: true, message: 'Campo requerido' }]}
              >
                <Select
                  showSearch
                  optionFilterProp="label"
                  options={
                    countryStates
                      ? countryStates.map<DefaultOptionType>((c) => ({
                          value: c.id!,
                          label: c.shortName!,
                        }))
                      : []
                  }
                ></Select>
              </Form.Item>
            </Col>

            <Col xs={12}>
              <Form.Item<FormFields>
                label="RFC"
                name="rfc"
                rules={[
                  { pattern: /^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{2}[0-9A]$/, message: 'Formato inválido' },
                ]}
              >
                <Input
                  maxLength={13}
                  style={{ textTransform: 'uppercase' }}
                  onInput={(e) => (e.currentTarget.value = e.currentTarget.value.toUpperCase())}
                ></Input>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </>
  );
};

export default CustomerFormDialog;
