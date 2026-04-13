<template>
  <v-dialog :model-value="modelValue" :max-width="maxWidth" @update:model-value="onUpdate">
    <v-card>
      <v-card-title class="dialog-title">
        <v-icon v-if="icon" class="mr-2" :color="iconColor">{{ icon }}</v-icon>
        {{ title }}
      </v-card-title>
      <v-card-text>
        <slot>
          {{ message }}
        </slot>
      </v-card-text>
      <v-card-actions class="pa-4 pt-0">
        <v-spacer />
        <v-btn variant="text" :disabled="loading" @click="cancel">Cancelar</v-btn>
        <v-btn :color="confirmColor" variant="elevated" :disabled="loading" @click="confirm">
          {{ confirmText }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    modelValue: boolean;
    title: string;
    message?: string;
    confirmText?: string;
    confirmColor?: string;
    maxWidth?: number | string;
    loading?: boolean;
    icon?: string;
    iconColor?: string;
  }>(),
  {
    message: '',
    confirmText: 'Confirmar',
    confirmColor: 'primary',
    maxWidth: 520,
    loading: false,
    icon: '',
    iconColor: 'primary',
  },
);

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void;
  (e: 'confirm'): void;
  (e: 'cancel'): void;
}>();

function onUpdate(value: boolean) {
  emit('update:modelValue', value);
}

function cancel() {
  emit('cancel');
  emit('update:modelValue', false);
}

function confirm() {
  emit('confirm');
}
</script>

<style scoped>
.dialog-title {
  font-weight: 900;
}
</style>

