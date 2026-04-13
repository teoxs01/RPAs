<template>
  <div class="header">
    <div class="title-row">
      <div>
        <div class="page-title">
          <v-icon v-if="icon" class="mr-2" color="primary">{{ icon }}</v-icon>
          {{ title }}
        </div>
        <div v-if="subtitle" class="page-subtitle">{{ subtitle }}</div>
      </div>

      <div v-if="$slots.actions" class="header-actions">
        <slot name="actions" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  title: string;
  subtitle?: string;
  icon?: string;
}>();
</script>

<style scoped>
.header {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.title-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
}

.header-actions {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
  align-items: center;
  justify-content: flex-end;
}
</style>

