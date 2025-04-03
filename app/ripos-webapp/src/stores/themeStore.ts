import { create } from 'zustand';

interface ThemeState {
  darkMode: boolean;
  toggleTheme: () => void;
  setDarkMode: () => void;
  setLightMode: () => void;
}

const useThemeStore = create<ThemeState>((set) => ({
  darkMode: false,
  toggleTheme: () => set((state) => ({ darkMode: !state.darkMode })),
  setDarkMode: () => set(() => ({ darkMode: true })),
  setLightMode: () => set(() => ({ darkMode: false })),
}));

export { useThemeStore };
