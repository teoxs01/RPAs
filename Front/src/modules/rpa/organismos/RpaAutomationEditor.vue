<template>
  <v-dialog 
    :model-value="open" 
    @update:model-value="$emit('update:open', $event)" 
    max-width="800"
    persistent
    scrollable
  >
    <v-card class="rounded-lg">
      <v-card-title class="dialog-title d-flex align-center bg-primary text-white py-4">
        <v-icon class="mr-2" color="white">mdi-robot-outline</v-icon>
        <span class="text-h6 font-weight-bold">{{ getTitle() }}</span>
        <v-spacer />
        <v-btn icon="mdi-close" variant="text" color="white" @click="handleClose" />
      </v-card-title>
      
      <v-divider />

      <v-card-text class="pa-0" style="height: 500px;">
        <v-stepper v-model="step" alt-labels flat class="bg-transparent">
          <v-stepper-header>
            <v-stepper-item :complete="step > 1" value="1" title="Información" />
            <v-divider />
            <v-stepper-item :complete="step > 2" value="2" title="Configuración" />
          </v-stepper-header>

          <v-stepper-window>
            <!-- PASO 1: Nombre y Descripción -->
            <v-stepper-window-item value="1" class="pa-4">
              <v-row dense>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.codigo"
                    label="Código de la automatización"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                    :rules="[v => !!v || 'El código es requerido']"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.nombre"
                    label="Nombre de la automatización"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                    :rules="[v => !!v || 'El nombre es requerido']"
                  />
                </v-col>
                <v-col cols="12">
                  <v-textarea
                    v-model="localForm.descripcion"
                    label="Descripción (opcional)"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                    rows="3"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-select
                    v-model="localForm.tipo"
                    :items="['RPA', 'API', 'Script']"
                    label="Tipo"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-select
                    v-model="localForm.entorno"
                    :items="['PROD', 'DESA', 'QA']"
                    label="Entorno"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                  />
                </v-col>
              </v-row>
            </v-stepper-window-item>

            <!-- PASO 2: Configuración -->
            <v-stepper-window-item value="2" class="pa-4">
              <v-row dense>
                <v-col cols="12">
                  <RpaScriptPathField
                    v-model="localForm.scriptPath"
                    :error-messages="scriptPathError ? [scriptPathError] : []"
                    @paste="$emit('paste')"
                    @copy="$emit('copy')"
                    @clear="$emit('clear-script')"
                  />
                </v-col>

                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.horaEjecucion"
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
                    v-model="localForm.diasEjecucion"
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
                        v-if="isAllDays(localForm.diasEjecucion) ? index === 0 : true"
                        size="x-small"
                        variant="tonal"
                        color="primary"
                        class="mr-1"
                      >
                        {{ isAllDays(localForm.diasEjecucion) ? 'Todos los días' : item.title }}
                      </v-chip>
                    </template>
                  </v-select>
                </v-col>

                <v-col cols="12">
                  <v-switch
                    v-model="localForm.activo"
                    color="success"
                    base-color="error"
                    inset
                    hide-details
                    :label="localForm.activo ? 'Activo' : 'Inactivo'"
                  />
                </v-col>
              </v-row>
            </v-stepper-window-item>
          </v-stepper-window>
        </v-stepper>
      </v-card-text>
      <v-divider />
      <v-card-actions class="pa-4">
        <v-btn v-if="step === 2 && mode === 'create'" variant="tonal" @click="step = 1" prepend-icon="mdi-chevron-left">Atrás</v-btn>
        <v-spacer />
        <v-btn variant="text" @click="handleClose" class="mr-2">Cancelar</v-btn>
        <v-btn v-if="step === 1" color="primary" variant="elevated" @click="handleNext" append-icon="mdi-chevron-right">Siguiente</v-btn>
        <v-btn v-else color="success" variant="elevated" :loading="saving" @click="handleSave" prepend-icon="mdi-check">Guardar Automatización</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import RpaScriptPathField from '@/modules/rpa/moleculas/RpaScriptPathField.vue';
import type { AutomationUpsertInput } from '@/modules/rpa/types';

import { createAutomatizacion, updateAutomatizacion, saveConfiguracion } from '@/modules/rpa/services/automatizaciones.service';
import { useRpaToast } from '@/modules/rpa/composables/useRpaToast';

const props = defineProps<{
  open: boolean;
  mode: 'create' | 'edit';
  form: Partial<AutomationUpsertInput>;
  scriptPathError?: string | null;
  diasSemana: string[];
  isAllDays: (days: string[]) => boolean;
}>();

const emit = defineEmits<{
  (e: 'update:open', value: boolean): void;
  (e: 'update:field', payload: { field: keyof AutomationUpsertInput; value: any }): void;
  (e: 'close'): void;
  (e: 'save-complete'): void;
  (e: 'paste'): void;
  (e: 'copy'): void;
  (e: 'clear-script'): void;
}>();

const { showToast } = useRpaToast();
const step = ref(1);
const saving = ref(false);
const autoId = ref<number | null>(null);

const localForm = ref({
  codigo: '',
  nombre: '',
  descripcion: null as string | null,
  tipo: 'RPA',
  entorno: 'PROD',
  scriptPath: '',
  horaEjecucion: '08:00',
  diasEjecucion: [] as string[],
  activo: true,
});

watch(() => props.open, (newVal) => {
  if (newVal) {
    step.value = 1;
    // En modo edición, cargamos el ID que viene del form (que ahora incluye autoId)
    autoId.value = (props.form as any).autoId ?? null;
    localForm.value = {
      codigo: props.form.codigo ?? '',
      nombre: props.form.nombre ?? '',
      descripcion: props.form.descripcion ?? null,
      tipo: props.form.tipo ?? 'RPA',
      entorno: props.form.entorno ?? 'PROD',
      scriptPath: props.form.scriptPath ?? '',
      horaEjecucion: props.form.horaEjecucion ?? '08:00',
      diasEjecucion: [...(props.form.diasEjecucion ?? [])],
      activo: props.form.activo ?? true,
    };
  }
});

function getTitle() {
  if (props.mode === 'create') {
    return step.value === 1 ? 'Nueva automatización - Información' : 'Nueva automatización - Configuración';
  }
  return step.value === 1 ? 'Editar automatización - Información' : 'Editar automatización - Configuración';
}

async function handleNext() {
  if (!localForm.value.nombre.trim() || !localForm.value.codigo.trim()) {
    showToast('El código y el nombre son obligatorios', 'warning');
    return;
  }

  saving.value = true;
  try {
    const payload = {
      codigo: localForm.value.codigo,
      nombre: localForm.value.nombre,
      descripcion: localForm.value.descripcion,
      tipo: localForm.value.tipo,
      entorno: localForm.value.entorno
    };

    if (props.mode === 'create' && !autoId.value) {
      const res = await createAutomatizacion(payload);
      autoId.value = (res as any).autoId;
      showToast('Automatización creada correctamente', 'success');
    } else {
      // Si ya tenemos autoId (editando o acabamos de crear), actualizamos
      if (autoId.value) {
        await updateAutomatizacion(autoId.value, payload);
        showToast('Información básica actualizada', 'info');
      }
    }
    
    step.value = 2;
  } catch (e) {
    showToast('Error al guardar la automatización', 'error');
  } finally {
    saving.value = false;
  }
}

function handleClose() {
  emit('close');
  emit('update:open', false);
}

async function handleSave() {
  if (!autoId.value) return;

  saving.value = true;
  try {
    await saveConfiguracion(autoId.value, {
      scriptPath: localForm.value.scriptPath,
      horaEjecucion: localForm.value.horaEjecucion,
      diasSemana: localForm.value.diasEjecucion,
      activo: localForm.value.activo
    });
    
    showToast('Configuración guardada correctamente', 'success');
    emit('save-complete');
    handleClose();
  } catch (e) {
    showToast('Error al guardar la configuración', 'error');
  } finally {
    saving.value = false;
  }
}
</script>

<style scoped>
.dialog-title {
  font-weight: 900;
  font-size: 1.2rem;
  padding: 1.5rem 1.5rem 1rem;
}
</style>
