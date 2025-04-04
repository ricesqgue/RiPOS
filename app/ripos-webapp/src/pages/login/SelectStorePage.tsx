import { Button, Card, Col, Flex, Form, Row, Select } from 'antd';
import type { FormProps } from 'antd';
import styles from './loginPage.module.scss';
import { useAuthStore } from '@stores/authStore';
import { useNavigate } from 'react-router';

type FormFields = {
  storeId: number;
};

const SelectStorePage = () => {
  const [form] = Form.useForm();
  const { availableStores, setStoreId } = useAuthStore();
  const navigate = useNavigate();

  const storeOptions = availableStores.map((store) => ({
    label: store.name,
    value: store.id,
  }));

  const onFinish: FormProps<FormFields>['onFinish'] = (values) => {
    setStoreId(values.storeId);
    navigate('/', { replace: true });
  };

  return (
    <Row className={styles.container} justify="center" align="middle">
      <Col xs={{ span: 20 }} sm={{ span: 18 }} md={{ span: 10 }} lg={{ span: 8 }}>
        <Card title="Iniciar sesión" className={styles.card}>
          <Form
            form={form}
            requiredMark={false}
            layout="vertical"
            autoComplete="off"
            onFinish={onFinish}
            size="large"
            variant="filled"
          >
            <Form.Item<FormFields>
              label="Selecciona sucursal"
              name="storeId"
              rules={[{ required: true, message: '' }]}
            >
              <Select options={storeOptions} placeholder="Seleccionar..." />
            </Form.Item>

            <Flex justify="end">
              <Form.Item shouldUpdate>
                {() => (
                  <Button
                    htmlType="submit"
                    type="primary"
                    disabled={
                      !form.isFieldsTouched(true) ||
                      !!form.getFieldsError().some(({ errors }) => errors.length > 0)
                    }
                  >
                    Continuar
                  </Button>
                )}
              </Form.Item>
            </Flex>
          </Form>
        </Card>
      </Col>
    </Row>
  );
};

export default SelectStorePage;
