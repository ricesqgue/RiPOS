import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import tsConfigPaths from 'vite-tsconfig-paths';
import svgr from 'vite-plugin-svgr';

// https://vite.dev/config/
export default defineConfig({
  plugins: [tsConfigPaths(), react(), svgr()],
  css: {
    modules: {
      localsConvention: 'camelCaseOnly',
    },
  },
});
