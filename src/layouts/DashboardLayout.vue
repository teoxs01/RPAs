<template>
  <div class="layout-root">
    <v-navigation-drawer
      v-model="drawer"
      class="sidebar"
      width="280"
      :rail="rail"
      permanent
    >
      <div class="sidebar-header" :class="{ 'sidebar-header--rail': rail }">
        <v-btn icon variant="text" class="sidebar-toggle" @click="toggleRail">
          <v-icon>{{ rail ? 'mdi-menu' : 'mdi-chevron-left' }}</v-icon>
        </v-btn>
      </div>

      <v-divider />

      <v-list nav density="compact" class="sidebar-list">
        <div v-if="!rail" class="sidebar-section">RPA</div>

        <template v-for="item in items" :key="item.to">
          <v-tooltip v-if="rail" :text="item.title" location="end">
            <template #activator="{ props }">
              <v-list-item v-bind="props" :to="item.to" :prepend-icon="item.icon" class="nav-item" />
            </template>
          </v-tooltip>

          <v-list-item v-else :to="item.to" :prepend-icon="item.icon" class="nav-item">
            <v-list-item-title class="nav-title">{{ item.title }}</v-list-item-title>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>

    <v-main class="main">
      <div class="page-shell">
        <slot />
      </div>
    </v-main>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

const drawer = ref(true);
const rail = ref(false);

const toggleRail = () => {
  rail.value = !rail.value;
};

const items = computed(() => [
  { title: 'RPA', icon: 'mdi-robot-outline', to: '/rpa' },
  { title: 'Gestión de Automatizaciones', icon: 'mdi-cog-outline', to: '/rpa/automatizaciones' },
]);
</script>

<style scoped>
.layout-root {
  min-height: 100vh;
}

.sidebar {
  border-right: 1px solid rgba(0, 0, 0, 0.08);
  background: #ffffff;
}

.sidebar-header {
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0 8px;
  background: #252550;
}

.sidebar-header--rail {
  justify-content: center;
  padding: 0;
}

.sidebar-toggle {
  color: #ffffff;
}

.sidebar-section {
  padding: 10px 16px 6px;
  font-size: 11px;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(31, 41, 55, 0.65);
}

.sidebar-list :deep(.v-list-item) {
  border-radius: 10px;
  margin: 4px 8px;
}

.sidebar-list :deep(.v-list-item--active) {
  background: #1c8dce;
  color: #ffffff;
}

.sidebar-list :deep(.v-list-item--active .v-icon) {
  color: #ffffff;
}

.sidebar-list :deep(.v-list-item:hover) {
  background: rgba(28, 141, 206, 0.08);
}

.nav-item :deep(.v-list-item__prepend) {
  margin-inline-end: 10px;
}

.nav-title {
  font-weight: 800;
  font-size: 13px;
  color: var(--text-primary);
}

.sidebar :deep(.v-navigation-drawer__content) {
  overflow-x: hidden;
  scrollbar-width: thin;
}

.sidebar :deep(.v-navigation-drawer__content::-webkit-scrollbar) {
  width: 8px;
}

.sidebar :deep(.v-navigation-drawer__content::-webkit-scrollbar-thumb) {
  background: rgba(0, 0, 0, 0.18);
  border-radius: 8px;
}

.sidebar :deep(.v-navigation-drawer--rail .v-list-item) {
  margin: 6px 6px;
  border-radius: 12px;
  justify-content: center;
}

.sidebar :deep(.v-navigation-drawer--rail .v-list-item__prepend) {
  margin-inline-end: 0;
}

.sidebar :deep(.v-navigation-drawer--rail .v-list-item-title) {
  display: none;
}

.sidebar :deep(.v-navigation-drawer--rail .v-list-item__content) {
  display: none;
}

.main {
  background: #f5f7fb;
}

.page-shell {
  padding: 20px;
  max-width: 1400px;
  width: 100%;
  margin: 0 auto;
}
</style>
