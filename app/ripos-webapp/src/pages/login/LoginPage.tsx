import { Button, Card, Col, Flex, Form, Input, Row } from 'antd';
import type { FormProps } from 'antd';
import styles from './loginPage.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faKey, faUser } from '@fortawesome/free-solid-svg-icons';
import { useAuthStore } from '@stores/authStore';
import { Navigate, useNavigate } from 'react-router';
import { AuthRequest } from '@api/generated/models';
import { usePostApiAuth } from '@api/generated/auth/auth';

type FormFields = {
  username?: string;
  password?: string;
};

const LoginPage = () => {
  const [form] = Form.useForm();
  const { mutateAsync: login, isPending } = usePostApiAuth();
  const navigate = useNavigate();

  const onFinish: FormProps<FormFields>['onFinish'] = (values) => {
    const loginRequest: AuthRequest = {
      username: values.username ?? '',
      password: values.password ?? '',
    };

    login({ data: loginRequest })
      .then((response) => {
        const { accessToken, availableStores } = response.data;
        useAuthStore.getState().setAccessToken(accessToken);
        useAuthStore.getState().setAvailableStores(availableStores);
        navigate('/login/store', { replace: true });
      })
      .catch((error) => {
        console.error('Login failed:', error);
      });
  };

  if (useAuthStore.getState().isAuthenticated) {
    return <Navigate to="/login/store" replace />;
  }

  return (
    <Row className={styles.container} justify="center" align="middle">
      <Col xs={{ span: 20 }} sm={{ span: 18 }} md={{ span: 10 }} lg={{ span: 8 }}>
        <Card title={'Iniciar sesión'} className={styles.card}>
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
              label="Nombre de usuario"
              name="username"
              rules={[{ required: true, message: '' }]}
            >
              <Input addonBefore={<FontAwesomeIcon icon={faUser} />}></Input>
            </Form.Item>

            <Form.Item<FormFields>
              label="Contraseña"
              name="password"
              rules={[{ required: true, message: '' }]}
            >
              <Input.Password addonBefore={<FontAwesomeIcon icon={faKey} />}></Input.Password>
            </Form.Item>

            <Flex justify="end">
              <Form.Item shouldUpdate>
                {() => (
                  <Button
                    htmlType="submit"
                    type="primary"
                    loading={isPending}
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
