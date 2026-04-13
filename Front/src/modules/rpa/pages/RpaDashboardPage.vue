<template>
  <RpaPageHeader
    title="Monitoreo RPA"
    subtitle="Monitoreo centralizado de automatizaciones en Grafana"
    icon="mdi-robot-outline"
  />

  <v-card elevation="2" class="grafana-card mt-4">
    <v-card-text class="grafana-card__body">
      <v-alert v-if="!grafanaEmbedUrl" type="info" variant="tonal">
        Configura VITE_GRAFANA_EMBED_URL para visualizar el dashboard.
      </v-alert>

      <div v-else class="grafana-frame">
        <iframe class="grafana-frame__iframe" :src="grafanaEmbedUrl" frameborder="0" allowfullscreen />
      </div>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import RpaPageHeader from '@/modules/rpa/moleculas/RpaPageHeader.vue';

const defaultGrafanaEmbedUrl =
  'http://192.168.24.13:3000/';

const grafanaEmbedUrl = computed(() => import.meta.env.VITE_GRAFANA_EMBED_URL || defaultGrafanaEmbedUrl);
</script>

<style scoped>
.grafana-card {
  width: 100%;
  border-radius: 12px;
  overflow: hidden;
  border: 1px solid rgba(0, 0, 0, 0.08);
}

.grafana-card__body {
  padding: 18px;
  width: 100%;
}

.grafana-frame {
  width: 100%;
  height: 1000px;
  border-radius: 12px;
  overflow: hidden;
  border: 1px solid rgba(0, 0, 0, 0.08);
}

.grafana-frame__iframe {
  width: 100%;
  height: 100%;
  background: #ffffff;
}
</style>
