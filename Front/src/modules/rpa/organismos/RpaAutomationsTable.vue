<template>
  <v-card class="mt-4 table-card" elevation="2">
    <div class="table-card__header">
      <div class="table-card__left">
        <div class="table-card__title">Automatizaciones</div>
        <div class="table-card__hint">
          Mostrando <strong>{{ items.length }}</strong> de <strong>{{ totalItems }}</strong>
          <span v-if="activeFiltersCount > 0"> · Filtros activos: {{ activeFiltersCount }}</span>
        </div>
      </div>
    </div>
    <v-divider />
    <v-data-table
      :headers="headers"
      :items="items"
      item-key="autoId"
      :items-per-page="10"
      class="rpa-table"
      density="comfortable"
      hover
      :loading="loading"
      loading-text="Cargando automatizaciones..."
      no-data-text="Sin automatizaciones"
    >
      <template #item.nombre="{ item }">
        <div class="name-cell">
          <div class="name-cell__title">{{ item.nombre }}</div>
          <div class="name-cell__subtitle" :title="item.scriptPath">{{ item.scriptPath }}</div>
        </div>
      </template>

      <template #item.activo="{ item }">
        <v-chip size="small" variant="tonal" :color="item.activo ? 'success' : 'error'">
          {{ item.activo ? 'Sí' : 'No' }}
        </v-chip>
      </template>

      <template #item.ultimoEstado="{ item }">
        <v-chip size="small" variant="tonal" :color="estadoColor(item.ultimoEstado)">
          {{ item.ultimoEstado }}
        </v-chip>
      </template>

      <template #item.ultimaEjecucion="{ item }">
        <span class="cell-muted">{{ item.ultimaEjecucion ? formatDateTime(item.ultimaEjecucion) : '-' }}</span>
      </template>

      <template #item.horaEjecucion="{ item }">
        <span class="cell-mono">{{ item.horaEjecucion }}</span>
      </template>

      <template #item.diasEjecucion="{ item }">
        <div class="days-cell">
          <v-chip v-if="isAllDays(item.diasEjecucion ?? [])" size="x-small" variant="tonal" color="primary" class="mr-1">
            Todos los días
          </v-chip>
          <v-chip
            v-else
            v-for="day in (item.diasEjecucion ?? []).slice(0, 3)"
            :key="day"
            size="x-small"
            variant="tonal"
            color="primary"
            class="mr-1"
          >
            {{ day.substring(0, 3) }}
          </v-chip>
          <span v-if="!isAllDays(item.diasEjecucion ?? []) && (item.diasEjecucion ?? []).length > 3" class="days-cell__more">
            +{{ (item.diasEjecucion ?? []).length - 3 }}
          </span>
        </div>
      </template>

      <template #item.acciones="{ item }">
        <div class="actions">
          <v-tooltip text="Editar" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                size="default"
                variant="text"
                color="primary"
                class="action-btn"
                :aria-label="`Editar ${item.nombre}`"
                :disabled="loading || rowBusy.has(item.autoId)"
                @click="$emit('edit', item)"
              >
                <v-icon size="20">mdi-pencil-outline</v-icon>
              </v-btn>
            </template>
          </v-tooltip>

          <v-tooltip :text="item.activo ? 'Desactivar' : 'Activar'" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                size="default"
                variant="text"
                :color="item.activo ? 'success' : 'error'"
                class="action-btn"
                :aria-label="item.activo ? 'Desactivar' : 'Activar'"
                :disabled="loading || rowBusy.has(item.autoId)"
                @click="$emit('toggle-activo', item)"
              >
                <v-icon size="20">{{ item.activo ? 'mdi-toggle-switch' : 'mdi-toggle-switch-off' }}</v-icon>
              </v-btn>
            </template>
          </v-tooltip>

          <v-tooltip text="Ejecutar manualmente" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                size="default"
                variant="text"
                color="error"
                class="action-btn"
                :aria-label="`Ejecutar ${item.nombre}`"
                :disabled="loading || rowBusy.has(item.autoId)"
                @click="$emit('execute', item)"
              >
                <v-icon size="20">mdi-play-circle</v-icon>
              </v-btn>
            </template>
          </v-tooltip>

          <v-tooltip text="Eliminar" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                size="default"
                variant="text"
                color="error"
                class="action-btn"
                :aria-label="`Eliminar ${item.nombre}`"
                :disabled="loading || rowBusy.has(item.autoId)"
                @click="$emit('delete', item)"
              >
                <v-icon size="20">mdi-delete-outline</v-icon>
              </v-btn>
            </template>
          </v-tooltip>
        </div>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup lang="ts">
import type { Automation } from '@/modules/rpa/types';

defineProps<{
  items: Automation[];
  totalItems: number;
  headers: any[];
  loading: boolean;
  rowBusy: Set<number>;
  activeFiltersCount: number;
  estadoColor: (estado: string) => string;
  formatDateTime: (date: string | null) => string;
  isAllDays: (days: string[]) => boolean;
}>();

defineEmits<{
  (e: 'edit', item: Automation): void;
  (e: 'toggle-activo', item: Automation): void;
  (e: 'execute', item: Automation): void;
  (e: 'delete', item: Automation): void;
}>();
</script>

<style scoped>
.table-card {
  border-radius: 14px;
  overflow: hidden;
  border: 1px solid rgba(0, 0, 0, 0.08);
}

.table-card__header {
  padding: 14px 16px;
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
}

.table-card__left {
  min-width: 240px;
}

.table-card__title {
  font-weight: 900;
  font-size: 14px;
  color: var(--text-primary);
}

.table-card__hint {
  margin-top: 4px;
  font-size: 12px;
  color: var(--text-secondary);
}

.rpa-table :deep(th) {
  font-weight: 800;
}

.rpa-table :deep(tbody tr:hover) {
  background: rgba(28, 141, 206, 0.06);
}

.actions {
  display: inline-flex;
  gap: 4px;
  justify-content: flex-end;
  width: 100%;
}

.action-btn {
  width: 36px;
  height: 36px;
}

.name-cell {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
}

.name-cell__title {
  font-weight: 900;
  font-size: 13px;
  color: var(--text-primary);
  line-height: 1.2;
}

.name-cell__subtitle {
  font-size: 12px;
  color: var(--text-secondary);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 520px;
}

.days-cell {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 2px;
}

.days-cell__more {
  font-size: 12px;
  color: var(--text-secondary);
}

.cell-muted {
  color: var(--text-secondary);
  font-size: 13px;
}

.cell-mono {
  font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, 'Liberation Mono', monospace;
  font-size: 13px;
}
</style>
