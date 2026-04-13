<template>
  <v-row class="mt-4" dense>
    <v-col cols="12" sm="6" lg="3">
      <RpaMetricCard
        label="Total automatizaciones"
        :value="stats.total"
        icon="mdi-format-list-bulleted"
        variant="primary"
      />
    </v-col>
    <v-col cols="12" sm="6" lg="3">
      <RpaMetricCard
        label="Activas"
        :value="stats.activas"
        icon="mdi-robot-happy-outline"
        variant="green"
      />
    </v-col>
    <v-col cols="12" sm="6" lg="3">
      <RpaMetricCard
        label="En error"
        :value="stats.errores"
        icon="mdi-alert-circle-outline"
        variant="red"
      />
    </v-col>
    <v-col cols="12" sm="6" lg="3">
      <RpaMetricCard
        label="Pendientes"
        :value="stats.pendientes"
        icon="mdi-timer-outline"
        variant="teal"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import RpaMetricCard from '@/modules/rpa/atomos/RpaMetricCard.vue';

defineProps<{
  stats: {
    total: number;
    activas: number;
    errores: number;
    pendientes: number;
  };
}>();
</script>
