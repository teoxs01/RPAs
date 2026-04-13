<template>
  <v-dialog :model-value="open" @update:model-value="$emit('update:open', $event)" max-width="720">
    <v-card>
      <v-card-title class="dialog-title">
        <v-icon class="mr-2" color="primary">mdi-robot-outline</v-icon>
        {{ mode === 'create' ? 'Nueva automatización' : 'Editar automatización' }}
      </v-card-title>
      <v-card-text>
        <v-row dense>
          <v-col cols="12" md="6">
            <v-text-field
              :model-value="form.nombre"
              @update:model-value="$emit('update:field', { field: 'nombre', value: $event })"
              label="Nombre"
              variant="outlined"
              density="compact"
              hide-details
              class="bg-white"
            />
          </v-col>
          <v-col cols="12" md="6">
            <v-switch
              :model-value="form.activo"
              @update:model-value="$emit('update:field', { field: 'activo', value: $event })"
              color="success"
              base-color="error"
              inset
              hide-details
              :label="form.activo ? 'Activo' : 'Inactivo'"
            />
          </v-col>

          <v-col cols="12">
            <RpaScriptPathField
              :model-value="form.scriptPath ?? ''"
              @update:model-value="$emit('update:field', { field: 'scriptPath', value: $event })"
              :error-messages="scriptPathError ? [scriptPathError] : []"
              @paste="$emit('paste')"
              @copy="$emit('copy')"
              @clear="$emit('clear-script')"
            />
          </v-col>

          <v-col cols="12" md="6">
            <v-text-field
              :model-value="form.horaEjecucion"
              @update:model-value="$emit('update:field', { field: 'horaEjecucion', value: $event })"
              label="Hora de ejecución"
              type="time"
              variant="outlined"
              density="compact"
              hide-details
              class="bg-white"
            />
          </v-col>

          <v-col cols="12" md="6">
            <v-select
              :model-value="form.diasEjecucion"
              @update:model-value="$emit('update:field', { field: 'diasEjecucion', value: $event })"
              :items="diasSemana"
              label="Días de ejecución"
              multiple
              chips
              variant="outlined"
              density="compact"
              hide-details
              class="bg-white"
            >
              <template #selection="{ item, index }">
                <v-chip
                  v-if="isAllDays(form.diasEjecucion ?? []) ? index === 0 : true"
                  size="x-small"
                  variant="tonal"
                  color="primary"
                  class="mr-1"
                >
                {{ isAllDays(form.diasEjecucion ?? []) ? 'Todos los días' : item.title }}
                </v-chip>
              </template>
            </v-select>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions class="pa-4 pt-0">
        <v-spacer />
        <v-btn variant="text" @click="$emit('close')">Cancelar</v-btn>
        <v-btn color="primary" variant="elevated" @click="$emit('save')">Guardar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import RpaScriptPathField from '@/modules/rpa/moleculas/RpaScriptPathField.vue';
import type { Automation, AutomationUpsertInput } from '@/modules/rpa/types';

defineProps<{
  open: boolean;
  mode: 'create' | 'edit';
  form: Partial<AutomationUpsertInput>;
  scriptPathError?: string | null;
  diasSemana: string[];
  isAllDays: (days: string[]) => boolean;
}>();

defineEmits<{
  (e: 'update:open', value: boolean): void;
  (e: 'update:field', payload: { field: keyof AutomationUpsertInput; value: any }): void;
  (e: 'close'): void;
  (e: 'save'): void;
  (e: 'paste'): void;
  (e: 'copy'): void;
  (e: 'clear-script'): void;
}>();</script>

<style scoped>
.dialog-title {
  font-weight: 900;
  font-size: 1.2rem;
  padding: 1.5rem 1.5rem 1rem;
}
</style>
