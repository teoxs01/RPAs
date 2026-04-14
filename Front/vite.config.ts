import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import vuetify from 'vite-plugin-vuetify';
import { fileURLToPath, URL } from 'node:url';

export default defineConfig({
  plugins: [vue(), vuetify({ autoImport: true })],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  define: { 'process.env': {} },
  server: {
    proxy: {
      '/grafana': {
        target: 'http://192.168.24.13:3000',
        changeOrigin: true,
        cookieDomainRewrite: 'localhost',
        rewrite: path => path.replace(/^\/grafana/, ''),
      },
    },
  },
});
