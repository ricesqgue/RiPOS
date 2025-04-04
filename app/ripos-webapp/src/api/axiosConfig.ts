import axios from 'axios';
import { useAuthStore } from '@stores/authStore';
import { refreshToken } from './authService';
const api = axios.create({
  baseURL: 'https://localhost:7183',
  withCredentials: true,
});

api.interceptors.request.use(
  (config) => {
    const { storeId, accessToken } = useAuthStore.getState();
    const token = accessToken;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    if (storeId) {
      config.headers['X-Store-Id'] = storeId.toString();
    }
    return config;
  },
  (error) => Promise.reject(error)
);

let isRefreshing = false;
let failedRequestsQueue: Array<(token: string) => void> = [];

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const { accessToken, logout } = useAuthStore.getState();

      if (!accessToken) {
        logout(); // No access token available, log out
        return Promise.reject(error);
      }

      if (!isRefreshing) {
        isRefreshing = true;

        try {
          const newAccessToken = await refreshToken(accessToken);

          // Retry all failed requests with the new token
          failedRequestsQueue.forEach((callback) => callback(newAccessToken));
          failedRequestsQueue = [];
          isRefreshing = false;
          return api(originalRequest);
        } catch (refreshError) {
          useAuthStore.getState().logout(); // Clear auth state
          window.location.href = '/login'; // Redirect to login page
          return Promise.reject(refreshError);
        } finally {
          isRefreshing = false;
        }
      } else {
        return new Promise((resolve) => {
          failedRequestsQueue.push((newAccessToken) => {
            originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
            resolve(api(originalRequest));
          });
        });
      }
    }

    return Promise.reject(error);
  }
);

export default api;
