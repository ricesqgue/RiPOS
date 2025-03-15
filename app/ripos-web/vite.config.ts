import { defineConfig } from 'vite';
import { fileURLToPath } from 'url';
import react from '@vitejs/plugin-react-swc';
import tsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: [
      {
        find: '@styles',
        replacement: fileURLToPath(new URL('./src/styles', import.meta.url)),
      },
    ],
  },
  plugins: [tsconfigPaths(), react()],
  css: {
    modules: {
      localsConvention: 'camelCase',
    },
    preprocessorOptions: {
      scss: {
        additionalData: '@use "./src/styles/_variables.scss" as *;',
      },
    },
  },
});
