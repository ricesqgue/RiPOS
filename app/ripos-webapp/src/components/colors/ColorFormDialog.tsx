import { usePostApiColors, usePutApiColorsId } from '@api/generated/color/color';
import { ColorRequest, ColorResponse, ColorResponseMessageResponse } from '@api/generated/models';
import { useQueryClient } from '@tanstack/react-query';
import { ColorPicker, Form, Input, message, Modal } from 'antd';
import { useEffect, useRef, useState } from 'react';
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
  const [isFormValid, setIsFormValid] = useState(false);
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
    if (props.mode === 'add') {
      const addColorRequest: ColorRequest = {
        name: values.name!,
        rgbHex: typeof values.rgbHex! === 'string' ? values.rgbHex : values.rgbHex.toHexString(),
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
        .catch((err: AxiosError<ColorResponseMessageResponse>) => {
          messageApi.error({
            type: 'error',
            content: err.response?.data.message ?? 'Error al guardar los cambios',
          });
        });
    } else if (props.mode === 'edit') {
      const rgbHexFormValue = form.getFieldValue('rgbHex');
      const updateColorRequest: ColorRequest = {
        name: form.getFieldValue('name'),
        rgbHex:
          typeof rgbHexFormValue === 'string' ? rgbHexFormValue : rgbHexFormValue.toHexString(),
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
        .catch((err: AxiosError<ColorResponseMessageResponse>) => {
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
    setIsFormValid(false);
  };

  const handleOnFieldsChange = () => {
    const hasErrors = form.getFieldsError().some((field) => field.errors.length > 0);
    const currentRgbHex = form.getFieldValue('rgbHex');

    if (hasErrors || currentRgbHex === undefined) {
      setIsFormValid(false);
      return;
    }

    if (props.mode === 'edit') {
      const currentValues = form.getFieldsValue();
      const normalizedValues: FormFields = {
        name: currentValues.name.trim(),
        rgbHex:
          typeof currentValues.rgbHex === 'string'
            ? currentValues.rgbHex.trim()
            : currentValues.rgbHex.toHexString(),
      };
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
        title={`${props.mode === 'add' ? 'Agregar' : 'Editar'} color`}
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
          <Form.Item<FormFields> label="Código RGB" name="rgbHex" rules={[{ required: true }]}>
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
