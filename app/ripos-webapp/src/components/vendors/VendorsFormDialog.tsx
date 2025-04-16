import { usePostApiVendors, usePutApiVendorsId } from '@api/generated/vendor/vendor';
import {
  VendorRequest,
  VendorResponse,
  VendorResponseMessageResponse,
} from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { Button, Col, Form, Input, message, Modal, Row, Select } from 'antd';
import { useEffect, useMemo, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';
import { useGetApiMiscCountryStates } from '@api/generated/misc/misc';
import { DefaultOptionType } from 'antd/es/select';

interface VendorFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit' | 'view';
  onClose: () => void;
  editVendor?: VendorResponse;
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
  countryStateId: number;
};

const mapVendorToFormFields = (vendor: VendorResponse): FormFields => ({
  name: vendor.name!,
  surname: vendor.surname!,
  secondSurname: vendor.secondSurname!,
  phoneNumber: vendor.phoneNumber!,
  mobilePhone: vendor.mobilePhone!,
  email: vendor.email!,
  address: vendor.address!,
  city: vendor.city!,
  zipCode: vendor.zipCode!,
  countryStateId: vendor.countryState!.id!,
});

const mapFormFieldsToVendorRequest = (values: FormFields): VendorRequest => ({
  name: values.name,
  surname: values.surname,
  secondSurname: values.secondSurname,
  phoneNumber: values.phoneNumber,
  mobilePhone: values.mobilePhone,
  email: values.email,
  address: values.address,
  city: values.city,
  zipCode: values.zipCode,
  countryStateId: values.countryStateId,
});

const VendorFormDialog = (props: VendorFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addVendor, isPending: isPendingAdd } = usePostApiVendors();
  const { mutateAsync: updateVendor, isPending: isPendingUpdate } = usePutApiVendorsId();
  const [messageApi, contextHolder] = message.useMessage();
  const { data: countryStates } = useGetApiMiscCountryStates({
    query: {
      select: (response) => response.data,
    },
  });

  useEffect(() => {
    if (props.open && (props.mode === 'edit' || props.mode === 'view')) {
      if (props.editVendor) {
        const formValues = mapVendorToFormFields(props.editVendor);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editVendor, props.mode]);

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
    };

    if (props.mode === 'add') {
      const addVendorRequest = mapFormFieldsToVendorRequest(normalizedValues);

      addVendor({ data: addVendorRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<VendorResponseMessageResponse>) => {
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

      const updateVendorRequest = mapFormFieldsToVendorRequest(normalizedValues);

      updateVendor({ id: props.editVendor!.id!, data: updateVendorRequest })
        .then((response) => {
          invalidateQueries();
          handleOnClose();
          messageApi.open({
            type: 'success',
            content: response.data.message,
          });
        })
        .catch((err: AxiosError<VendorResponseMessageResponse>) => {
          console.log(err);
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    }
  };

  const invalidateQueries = () => {
    queryClient.invalidateQueries({ queryKey: ['/api/vendors'] });
  };

  const handleOnClose = () => {
    props.onClose();
    form.resetFields();
  };

  const modalTitle = useMemo(
    () =>
      `${props.mode === 'add' ? 'Agregar' : props.mode === 'edit' ? 'Editar' : 'Información del'} proveedor`,
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
                          label: c.name!,
                        }))
                      : []
                  }
                ></Select>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </>
  );
};

export default VendorFormDialog;
