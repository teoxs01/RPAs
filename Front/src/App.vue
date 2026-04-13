<template>
  <v-app>
    <DashboardLayout>
      <router-view />
    </DashboardLayout>
  </v-app>
</template>

<script setup lang="ts">
import DashboardLayout from '@/layouts/DashboardLayout.vue';
</script>
