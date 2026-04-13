<template>
  <v-text-field
    :model-value="modelValue"
    label="Script"
    variant="outlined"
    density="compact"
    :prepend-inner-icon="modelValue ? 'mdi-server' : 'mdi-file-cog-outline'"
    placeholder="Sin selección"
    hint="Selecciona un script del servidor (ruta absoluta/UNC)"
    persistent-hint
    :error-messages="errorMessages"
    class="bg-white"
    @update:model-value="onUpdate"
  >
    <template #append-inner>
      <div class="d-flex align-center ga-1">
        <v-tooltip text="Pegar desde portapapeles" location="top">
          <template #activator="{ props }">
            <v-btn
              v-bind="props"
              icon
              size="small"
              variant="text"
              color="primary"
              :aria-label="'Pegar ruta'"
              @click="emit('paste')"
            >
              <v-icon>mdi-content-paste</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip v-if="modelValue" text="Copiar ruta" location="top">
          <template #activator="{ props }">
            <v-btn
              v-bind="props"
              icon
              size="small"
              variant="text"
              color="primary"
              :aria-label="'Copiar ruta'"
              @click="emit('copy')"
            >
              <v-icon>mdi-content-copy</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip v-if="modelValue" text="Limpiar" location="top">
          <template #activator="{ props }">
            <v-btn
              v-bind="props"
              icon
              size="small"
              variant="text"
              color="primary"
              :aria-label="'Limpiar selección'"
              @click="emit('clear')"
            >
              <v-icon>mdi-close-circle-outline</v-icon>
            </v-btn>
          </template>
        </v-tooltip>
      </div>
    </template>
  </v-text-field>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    modelValue: string;
    errorMessages?: string[] | string;
  }>(),
  {
    modelValue: '',
    errorMessages: () => [],
  },
);

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void;
  (e: 'paste'): void;
  (e: 'copy'): void;
  (e: 'clear'): void;
}>();

function onUpdate(value: string) {
  emit('update:modelValue', value);
}
</script>

