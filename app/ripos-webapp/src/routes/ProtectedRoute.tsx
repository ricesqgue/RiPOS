import { Navigate, Outlet } from 'react-router';

const ProtectedRoute = () => {
  const isValid = false;

  if (!isValid) {
    return <Navigate to={'/login'} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
