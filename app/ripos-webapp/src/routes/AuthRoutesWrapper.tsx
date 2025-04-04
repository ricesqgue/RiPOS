import Loading from '@components/shared/Loading';
import useReauthenticate from '@hooks/useReauthenticate';
import { useAuthStore } from '@stores/authStore';
import { Navigate, Outlet, useLocation } from 'react-router';

const AuthRoutesWrapper = () => {
  const { isLoading } = useReauthenticate();
  const { isAuthenticated } = useAuthStore();
  const location = useLocation();

  const isLoginRoute = location.pathname === '/login';

  if (isLoading) {
    return <Loading />;
  }

  if (!isAuthenticated && !isLoginRoute) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default AuthRoutesWrapper;
