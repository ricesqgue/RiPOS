import { Button, Card, Col, Flex, Form, Input, Row } from 'antd';
import styles from './loginPage.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faKey, faUser } from '@fortawesome/free-solid-svg-icons';

type FormFields = {
  username?: string;
  password?: string;
};

const LoginPage = () => {
  const [form] = Form.useForm();

  return (
    <Row className={styles.container} justify="center" align="middle">
      <Col xs={{ span: 20 }} sm={{ span: 18 }} md={{ span: 10 }} lg={{ span: 8 }}>
        <Card title={'Iniciar sesión'} className={styles.card}>
          <Form form={form} requiredMark={false} layout="vertical" autoComplete="off">
            <Form.Item<FormFields>
              label="Nombre de usuario"
              name="username"
              rules={[{ required: true, message: '' }]}
            >
              <Input
                variant="filled"
                size="large"
                addonBefore={<FontAwesomeIcon icon={faUser} />}
              ></Input>
            </Form.Item>

            <Form.Item<FormFields>
              label="Contraseña"
              name="password"
              rules={[{ required: true, message: '' }]}
            >
              <Input.Password
                variant="filled"
                size="large"
                addonBefore={<FontAwesomeIcon icon={faKey} />}
              ></Input.Password>
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
                    Entrar
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

export default LoginPage;
