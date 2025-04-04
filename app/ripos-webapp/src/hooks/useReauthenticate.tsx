import { useEffect, useState } from 'react';
import { refreshToken } from '@api/authService';
import { useAuthStore } from '@stores/authStore';

const useReauthenticate = () => {
  const { accessToken, isAuthenticated, logout } = useAuthStore();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const tryReauthenticate = async () => {
      if (!accessToken) {
        setIsLoading(false);
        return;
      }

      try {
        await refreshToken(accessToken);
      } catch {
        logout();
      } finally {
        setIsLoading(false);
      }
    };

    if (!isAuthenticated) {
      tryReauthenticate();
    } else {
      setIsLoading(false);
    }
  }, [accessToken, isAuthenticated, logout]);

  return { isLoading };
};

export default useReauthenticate;
