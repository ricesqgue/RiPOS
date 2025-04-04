import { Navigate, Route, Routes } from 'react-router';
import ProtectedRoute from './ProtectedRoute';
import AppLayout from '@layout/AppLayout';
import LoginPage from '@pages/login/LoginPage';
import HomePage from '@pages/home/HomePage';
import SelectStorePage from '@pages/login/SelectStorePage';
import AuthRoutesWrapper from './AuthRoutesWrapper';

const AppRoutes = () => {
  return (
    <Routes>
      <Route element={<AuthRoutesWrapper />}>
        {/* Public routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/error" element={<h1>Error page</h1>} />

        {/* Protected routes */}
        <Route element={<ProtectedRoute />}>
          <Route path="/login/store" element={<SelectStorePage />} />
          <Route element={<AppLayout />}>
            <Route path="/" element={<HomePage />} />
          </Route>
        </Route>

        {/* Catch all invalid routes */}
        <Route path="*" element={<Navigate to={'/error'} replace />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
