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
            <v-stepper-item :complete="step > 1" :value="1" title="Información" />
            <v-divider />
            <v-stepper-item :complete="step > 2" :value="2" title="Configuración" />
            <v-divider />
            <v-stepper-item :complete="step > 3" :value="3" title="App Config" />
          </v-stepper-header>

          <v-stepper-window>
            <!-- PASO 1: Nombre y Descripción -->
            <v-stepper-window-item :value="1" class="pa-4">
              <v-row dense>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.codigo"
                    label="Código de la automatización*"
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
                    label="Nombre de la automatización*"
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
                    :items="['API', 'PYTHON']"
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
                    :items="['PROD', 'DESA']"
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
            <v-stepper-window-item :value="2" class="pa-4">
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

            <!-- PASO 3: App Config (Opcional) -->
            <v-stepper-window-item :value="3" class="pa-4">
              <v-row dense>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.appName"
                    label="Nombre de la Aplicación"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.appUrl"
                    label="URL de la Aplicación"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.appUser"
                    label="Usuario"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="localForm.appPassword"
                    label="Contraseña"
                    :type="showPassword ? 'text' : 'password'"
                    :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                    variant="outlined"
                    density="compact"
                    hide-details
                    class="bg-white"
                    @click:append-inner="showPassword = !showPassword"
                  />
                </v-col>
                <v-col cols="12" class="mt-2">
                  <v-alert type="info" variant="tonal" density="compact" class="text-caption">
                    Estos campos son opcionales y se guardarán junto con la automatización.
                  </v-alert>
                </v-col>
              </v-row>
            </v-stepper-window-item>
          </v-stepper-window>
        </v-stepper>
      </v-card-text>
      <v-divider />
      <v-card-actions class="pa-4">
        <v-btn v-if="step > 1" variant="tonal" @click="step--" prepend-icon="mdi-chevron-left">Atrás</v-btn>
        <v-spacer />
        <v-btn variant="text" @click="handleClose" class="mr-2">Cancelar</v-btn>
        <v-btn v-if="step < 3" color="primary" variant="elevated" @click="nextStep" append-icon="mdi-chevron-right">Siguiente</v-btn>
        <v-btn v-else color="success" variant="elevated" :loading="loading" @click="handleSave" prepend-icon="mdi-check">Guardar Automatización</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import RpaScriptPathField from '@/modules/rpa/moleculas/RpaScriptPathField.vue';
import type { AutomationUpsertInput } from '@/modules/rpa/types';

import { useRpaToast } from '@/modules/rpa/composables/useRpaToast';

const props = defineProps<{
  open: boolean;
  mode: 'create' | 'edit';
  form: Partial<AutomationUpsertInput>;
  scriptPathError?: string | null;
  diasSemana: string[];
  isAllDays: (days: string[]) => boolean;
  loading?: boolean;
}>();

const emit = defineEmits<{
  (e: 'update:open', value: boolean): void;
  (e: 'update:field', payload: { field: keyof AutomationUpsertInput; value: any }): void;
  (e: 'close'): void;
  (e: 'save'): void;
  (e: 'save-complete'): void;
  (e: 'paste'): void;
  (e: 'copy'): void;
  (e: 'clear-script'): void;
}>();

const { showToast } = useRpaToast();
const step = ref(1);
const showPassword = ref(false);

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
  appName: '',
  appUser: '',
  appPassword: '',
  appUrl: '',
});

watch(() => props.open, (newVal) => {
  if (newVal) {
    step.value = 1;
    showPassword.value = false;
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
      appName: props.form.appName ?? '',
      appUser: props.form.appUser ?? '',
      appPassword: props.form.appPassword ?? '',
      appUrl: props.form.appUrl ?? '',
    };
  }
});

function getTitle() {
  const prefix = props.mode === 'create' ? 'Nueva' : 'Editar';
  const steps: Record<number, string> = {
    1: 'Información',
    2: 'Configuración',
    3: 'App Config'
  };
  return `${prefix} automatización - ${steps[step.value] || ''}`;
}

function nextStep() {
  if (step.value === 1) {
    if (!localForm.value.nombre.trim() || !localForm.value.codigo.trim()) {
      showToast('El código y el nombre son obligatorios', 'warning');
      return;
    }
  }
  step.value++;
}

function handleClose() {
  emit('close');
  emit('update:open', false);
}

function handleSave() {
  // Sincronizamos los campos del formulario local al formulario del padre antes de emitir save
  Object.keys(localForm.value).forEach(key => {
    emit('update:field', { field: key as keyof AutomationUpsertInput, value: (localForm.value as any)[key] });
  });
  
  // Emitimos el evento save que el padre está escuchando
  setTimeout(() => {
    emit('save');
  }, 0);
}
</script>

<style scoped>
.dialog-title {
  font-weight: 900;
  font-size: 1.2rem;
  padding: 1.5rem 1.5rem 1rem;
}
</style>
