import { useGetApiAuthUserInfo } from '@api/generated/auth/auth';
import Loading from '@components/shared/Loading';
import useReauthenticate from '@hooks/useReauthenticate';
import { useAuthStore } from '@stores/authStore';
import { useEffect } from 'react';
import { Navigate, Outlet, useLocation } from 'react-router';

const AuthRoutesWrapper = () => {
  const { isLoading } = useReauthenticate();
  const { isAuthenticated, setUserInfo } = useAuthStore();
  const location = useLocation();
  const { data: userInfoResponse, isLoading: isLoadingUserInfo } = useGetApiAuthUserInfo({
    query: { enabled: isAuthenticated },
  });

  useEffect(() => {
    if (userInfoResponse && userInfoResponse.data) {
      setUserInfo(userInfoResponse.data);
    }
  }, [userInfoResponse, setUserInfo]);

  const isLoginRoute = location.pathname === '/login';

  if (isLoading || isLoadingUserInfo) {
    return <Loading fullscreen size="large" />;
  }

  if (!isAuthenticated && !isLoginRoute) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default AuthRoutesWrapper;
