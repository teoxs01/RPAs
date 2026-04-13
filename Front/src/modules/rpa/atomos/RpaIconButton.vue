<template>
  <v-tooltip :text="tooltip" location="top">
    <template #activator="{ props }">
      <v-btn
        v-bind="props"
        icon
        size="default"
        variant="text"
        :color="color"
        class="rpa-icon-btn"
        :disabled="disabled"
        :aria-label="ariaLabel"
        @click="emit('click')"
      >
        <v-icon size="20">{{ icon }}</v-icon>
      </v-btn>
    </template>
  </v-tooltip>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    icon: string;
    tooltip: string;
    ariaLabel: string;
    color?: string;
    disabled?: boolean;
  }>(),
  {
    color: 'primary',
    disabled: false,
  },
);

const emit = defineEmits<{
  (e: 'click'): void;
}>();
</script>

<style scoped>
.rpa-icon-btn {
  width: 36px;
  height: 36px;
}
</style>

