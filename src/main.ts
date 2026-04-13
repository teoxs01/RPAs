import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';
import './style.css';

import { createVuetify } from 'vuetify';
import { aliases, mdi } from 'vuetify/iconsets/mdi';

const vuetify = createVuetify({
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: { mdi },
  },
  theme: {
    defaultTheme: 'corporate',
    themes: {
      corporate: {
        dark: false,
        colors: {
          primary: '#0078d4',
          secondary: '#005a9e',
          surface: '#ffffff',
          background: '#f2f2f2',
          error: '#d32f2f',
          warning: '#f9a825',
          success: '#2e7d32',
        },
      },
    },
  },
});

createApp(App).use(router).use(vuetify).mount('#app');
