import { useAuthStore } from '@stores/authStore';
import { Navigate, Outlet, useLocation } from 'react-router';

const ProtectedRoute = () => {
  const { isAuthenticated, availableStores, storeId, setStoreId } = useAuthStore();
  const location = useLocation();
  const isSelectStorePage = location.pathname === '/login/store';

  if (!isAuthenticated) {
    return <Navigate to={'/login'} replace />;
  }

  // Verificar si hay una tienda seleccionada
  const isStoreSelected = () => {
    // Si hay una tienda seleccionada, verificar si es válida
    if (storeId && availableStores.some((store) => store.id === storeId)) {
      return true;
    }

    if (availableStores.length === 1) {
      // Solo hay una tienda disponible, seleccionarla automáticamente
      setStoreId(availableStores[0].id!);
      return true;
    }

    return false;
  };

  const storeSelected = isStoreSelected();

  // Redirigir a la selección de tienda si no hay tienda seleccionada
  if (!storeSelected && !isSelectStorePage) {
    return <Navigate to={'/login/store'} replace />;
  }

  // Redirigir a la página principal si ya hay una tienda seleccionada
  if (isSelectStorePage && storeSelected) {
    return <Navigate to={'/'} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
