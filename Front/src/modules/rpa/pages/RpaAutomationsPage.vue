<template>
  <RpaPageHeader
    title="Gestión de Automatizaciones"
    subtitle="Administración y ejecución manual de procesos RPA"
    icon="mdi-cog-outline"
  >
    <template #actions>
      <v-btn color="primary" variant="elevated" @click="openCreate">
        <v-icon start>mdi-plus-circle</v-icon>
        Nueva automatización
      </v-btn>
      <v-badge
        :model-value="activeFiltersCount > 0"
        :content="activeFiltersCount"
        color="primary"
        bordered
        offset-x="6"
        offset-y="6"
      >
        <v-btn variant="outlined" color="primary" class="text-none" @click="openFilters">
          <v-icon start>mdi-filter-variant</v-icon>
          Filtros
        </v-btn>
      </v-badge>
    </template>
  </RpaPageHeader>

  <RpaAutomationsStats :stats="stats" />

  <RpaAutomationsTable
    :items="filteredItems"
    :total-items="items.length"
    :headers="headers"
    :loading="tableLoading"
    :row-busy="rowBusy"
    :active-filters-count="activeFiltersCount"
    :estado-color="estadoColor"
    :format-date-time="formatDateTime"
    :is-all-days="isAllDays"
    @edit="openEdit"
    @toggle-activo="toggleActivo"
    @execute="executeManual"
    @delete="openDelete"
  />

  <RpaAutomationEditor
    v-model:open="editor.open"
    :mode="editor.mode"
    :form="editor.form"
    :script-path-error="scriptPathError"
    :dias-semana="diasSemana"
    :is-all-days="isAllDays"
    @update:field="({ field, value }: { field: string; value: any }) => {
      (editor.form as any)[field] = value;
    }"
    @close="closeEditor"
    @save="saveEditor"
    @paste="pasteScriptPath"
    @copy="copyScriptPath"
    @clear-script="clearScriptSelection"
  />

  <RpaAutomationsFilters
    v-model:open="filtersDialog.open"
    :draft="filtersDraft"
    :activo-options="activoOptions"
    :estado-options="estadoOptions"
    @update:field="({ field, value }) => (filtersDraft[field as keyof typeof filtersDraft] = value)"
    @clear="clearFilters"
    @close="closeFilters"
    @apply="applyFilters"
  />

  <RpaConfirmDialog
    v-model="deleter.open"
    title="Confirmar eliminación"
    confirm-text="Eliminar"
    confirm-color="error"
    icon="mdi-alert-circle-outline"
    icon-color="error"
    :loading="!!deleter.target && rowBusy.has(deleter.target.id)"
    @cancel="closeDelete"
    @confirm="confirmDelete"
  >
    ¿Eliminar la automatización <strong>{{ deleter.target?.nombre }}</strong>?
  </RpaConfirmDialog>

  <v-snackbar v-model="toast.open" :timeout="toast.timeout" :color="toast.color">
    {{ toast.message }}
    <template #actions>
      <v-btn variant="text" @click="toast.open = false">Cerrar</v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
import RpaPageHeader from '@/modules/rpa/moleculas/RpaPageHeader.vue';
import RpaConfirmDialog from '@/modules/rpa/moleculas/RpaConfirmDialog.vue';
import RpaAutomationsStats from '@/modules/rpa/organismos/RpaAutomationsStats.vue';
import RpaAutomationsTable from '@/modules/rpa/organismos/RpaAutomationsTable.vue';
import RpaAutomationEditor from '@/modules/rpa/organismos/RpaAutomationEditor.vue';
import RpaAutomationsFilters from '@/modules/rpa/organismos/RpaAutomationsFilters.vue';

import { useRpaAutomationsPage } from '@/modules/rpa/composables/useRpaAutomationsPage';

const {
  headers,
  toast,
  items,
  tableLoading,
  rowBusy,
  activoOptions,
  estadoOptions,
  filtersDialog,
  filtersDraft,
  activeFiltersCount,
  filteredItems,
  openFilters,
  closeFilters,
  applyFilters,
  clearFilters,
  stats,
  estadoColor,
  formatDateTime,
  diasSemana,
  isAllDays,
  editor,
  scriptPathError,
  copyScriptPath,
  pasteScriptPath,
  clearScriptSelection,
  openCreate,
  openEdit,
  closeEditor,
  saveEditor,
  deleter,
  openDelete,
  closeDelete,
  confirmDelete,
  toggleActivo,
  executeManual,
} = useRpaAutomationsPage();
</script>

