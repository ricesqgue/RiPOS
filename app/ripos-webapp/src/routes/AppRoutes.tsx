import { Navigate, Route, Routes } from 'react-router';
import ProtectedRoute from './ProtectedRoute';
import AppLayout from '@layout/AppLayout';
import LoginPage from '@pages/Login/LoginPage';

const AppRoutes = () => {
  return (
    <Routes>
      {/* Public routes */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/error" element={<h1>Error page</h1>} />

      {/* Protected routes */}
      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/" element={<h1>Home page</h1>} />
          <Route path="/dashboard" element={<h1>Dashboard page</h1>} />
        </Route>
      </Route>

      {/* Catch all invalid routes */}
      <Route path="*" element={<Navigate to={'/error'} replace />} />
    </Routes>
  );
};

export default AppRoutes;
