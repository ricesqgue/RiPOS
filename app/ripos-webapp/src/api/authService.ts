import axios from 'axios';
import { useAuthStore } from '@stores/authStore';

const refreshToken = async (accessToken: string) => {
  try {
    const response = await axios.post(
      `https://localhost:7183/api/auth/refreshToken`,
      { accessToken },
      { withCredentials: true }
    );

    const { accessToken: newAccessToken, availableStores } = response.data;

    // Actualizar el estado global con el nuevo token
    useAuthStore.getState().setAccessToken(newAccessToken);
    useAuthStore.getState().setAvailableStores(availableStores);

    return newAccessToken;
  } catch (error) {
    useAuthStore.getState().logout();
    throw error;
  }
};

export { refreshToken };
