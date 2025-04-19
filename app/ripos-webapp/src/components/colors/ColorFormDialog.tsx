import { usePostApiColors, usePutApiColorsId } from '@api/generated/color/color';
import { ColorRequest, ColorResponse, SimpleResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { ColorPicker, Form, Input, message, Modal } from 'antd';
import { useEffect, useRef } from 'react';
import isEqual from 'lodash/isEqual';
import { AxiosError } from 'axios';
import { Color } from 'antd/es/color-picker';

interface ColorFormDialogProps {
  open: boolean;
  mode: 'add' | 'edit';
  onClose: () => void;
  editColor?: ColorResponse;
}

type FormFields = {
  name: string;
  rgbHex: string | Color;
};

const mapColorToFormFields = (color: ColorResponse): FormFields => ({
  name: color.name!,
  rgbHex: color.rgbHex!,
});

const ColorFormDialog = (props: ColorFormDialogProps) => {
  const [form] = Form.useForm<FormFields>();
  const initialValuesRef = useRef<FormFields | null>(null);
  const queryClient = useQueryClient();
  const { mutateAsync: addColor, isPending: isPendingAdd } = usePostApiColors();
  const { mutateAsync: updateColor, isPending: isPendingUpdate } = usePutApiColorsId();
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (props.open && props.mode === 'edit') {
      if (props.editColor) {
        const formValues = mapColorToFormFields(props.editColor);
        form.setFieldsValue(formValues);
        initialValuesRef.current = formValues;
      } else {
        handleOnClose();
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form, props.open, props.editColor, props.mode]);

  const handleSubmitForm = (values: FormFields) => {
    const normalizedValues: FormFields = {
      ...values,
      name: values.name.trim(),
      rgbHex:
        typeof values.rgbHex === 'string' ? values.rgbHex.trim() : values.rgbHex.toHexString(),
    };

    if (props.mode === 'add') {
      const addColorRequest: ColorRequest = {
        name: normalizedValues.name,
        rgbHex: normalizedValues.rgbHex as string,
      };

      addColor({ data: addColorRequest })
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
      const updateColorRequest: ColorRequest = {
        name: normalizedValues.name,
        rgbHex: normalizedValues.rgbHex as string,
      };

      updateColor({ id: props.editColor!.id!, data: updateColorRequest })
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
    queryClient.invalidateQueries({ queryKey: ['/api/colors'] });
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} color`}
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
            label="Código RGB"
            name="rgbHex"
            rules={[{ required: true, message: 'Campo requerido' }]}
          >
            <ColorPicker
              disabledAlpha
              disabledFormat
              format="hex"
              defaultFormat="hex"
            ></ColorPicker>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default ColorFormDialog;
