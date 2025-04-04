import { StoreResponse } from '@api/generated/models';
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface AuthState {
  accessToken: string | null;
  storeId: number | null;
  isAuthenticated: boolean;
  availableStores: StoreResponse[];
  isRefreshingToken: boolean;
  setAccessToken: (accessToken: string) => void;
  setStoreId: (storeId: number) => void;
  setAvailableStores: (stores: StoreResponse[]) => void;
  setIsRefreshingToken: (isRefreshing: boolean) => void;
  logout: () => void;
}

const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      storeId: null,
      isAuthenticated: false,
      availableStores: [],
      isRefreshingToken: false,
      setAccessToken: (accessToken) => {
        set({ accessToken, isAuthenticated: !!accessToken });
      },
      setStoreId: (storeId) => {
        set({ storeId: storeId });
      },
      setAvailableStores: (stores: StoreResponse[]) => {
        set({ availableStores: stores });
      },
      setIsRefreshingToken: (isRefreshing) => {
        set({ isRefreshingToken: isRefreshing });
      },
      logout: () => {
        set({ accessToken: null, isAuthenticated: false });
      },
    }),
    {
      name: 'ripos-auth-storage',
      partialize: (state) => ({ accessToken: state.accessToken, storeId: state.storeId }),
      onRehydrateStorage: (state) => {
        if (state) {
          state.isAuthenticated = !!state.accessToken;
        }
      },
    }
  )
);

export { useAuthStore };
