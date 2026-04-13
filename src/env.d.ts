/// <reference types="vite/client" />

declare module '*.vue' {
  import type { DefineComponent } from 'vue';
  const component: DefineComponent<{}, {}, any>;
  export default component;
}

declare module 'vuetify/styles';

interface ImportMetaEnv {
  readonly VITE_GRAFANA_EMBED_URL?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
