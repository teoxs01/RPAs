<template>
  <v-dialog :model-value="open" @update:model-value="$emit('update:open', $event)" max-width="560">
    <v-card>
      <v-card-title class="dialog-title">
        <v-icon class="mr-2" color="primary">mdi-filter-variant</v-icon>
        Filtros
      </v-card-title>
      <v-card-text>
        <v-row dense>
          <v-col cols="12">
            <v-text-field
              :model-value="draft.search"
              @update:model-value="$emit('update:field', { field: 'search', value: $event })"
              label="Buscar por nombre o ruta"
              variant="outlined"
              density="compact"
              prepend-inner-icon="mdi-magnify"
              hide-details
              class="bg-white"
            />
          </v-col>

          <v-col cols="12" sm="6">
            <v-select
              :model-value="draft.activo"
              @update:model-value="$emit('update:field', { field: 'activo', value: $event })"
              :items="activoOptions"
              label="Activo"
              variant="outlined"
              density="compact"
              hide-details
              class="bg-white"
            />
          </v-col>

          <v-col cols="12" sm="6">
            <v-select
              :model-value="draft.ultimoEstado"
              @update:model-value="$emit('update:field', { field: 'ultimoEstado', value: $event })"
              :items="estadoOptions"
              label="Último estado"
              variant="outlined"
              density="compact"
              hide-details
              class="bg-white"
            />
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions class="pa-4 pt-0">
        <v-btn variant="text" @click="$emit('clear')">Limpiar</v-btn>
        <v-spacer />
        <v-btn variant="text" @click="$emit('close')">Cancelar</v-btn>
        <v-btn color="primary" variant="elevated" @click="$emit('apply')">Aplicar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import type { UltimoEstado } from '@/modules/rpa/types';

defineProps<{
  open: boolean;
  draft: {
    search: string;
    activo: 'Todos' | 'Sí' | 'No';
    ultimoEstado: 'Todos' | UltimoEstado;
  };
  activoOptions: string[];
  estadoOptions: string[];
}>();

defineEmits<{
  (e: 'update:open', value: boolean): void;
  (e: 'update:field', payload: { field: 'search' | 'activo' | 'ultimoEstado'; value: any }): void;
  (e: 'clear'): void;
  (e: 'close'): void;
  (e: 'apply'): void;
}>();
</script>

<style scoped>
.dialog-title {
  font-weight: 900;
  font-size: 1.2rem;
  padding: 1.5rem 1.5rem 1rem;
}
</style>
