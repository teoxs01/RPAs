import { computed, reactive } from 'vue';

import { type Automation, UltimoEstado } from '@/modules/rpa/types';

export function useRpaAutomationsFilters(items: { value: Automation[] }) {
  const filters = reactive({
    search: '',
    activo: 'Todos' as 'Todos' | 'Sí' | 'No',
    ultimoEstado: 'Todos' as 'Todos' | UltimoEstado,
  });

  const activoOptions = ['Todos', 'Sí', 'No'];
  const estadoOptions: Array<'Todos' | UltimoEstado> = [
    'Todos',
    UltimoEstado.OK,
    UltimoEstado.ERROR,
    UltimoEstado.EN_PROCESO,
    UltimoEstado.PENDIENTE,
  ];

  const dialog = reactive({
    open: false,
  });

  const draft = reactive({
    search: '',
    activo: 'Todos' as 'Todos' | 'Sí' | 'No',
    ultimoEstado: 'Todos' as 'Todos' | UltimoEstado,
  });

  const activeFiltersCount = computed(() => {
    let count = 0;
    if (filters.search.trim().length > 0) count += 1;
    if (filters.activo !== 'Todos') count += 1;
    if (filters.ultimoEstado !== 'Todos') count += 1;
    return count;
  });

  function openFilters() {
    draft.search = filters.search;
    draft.activo = filters.activo;
    draft.ultimoEstado = filters.ultimoEstado;
    dialog.open = true;
  }

  function closeFilters() {
    dialog.open = false;
  }

  function applyFilters() {
    filters.search = draft.search;
    filters.activo = draft.activo;
    filters.ultimoEstado = draft.ultimoEstado;
    dialog.open = false;
  }

  function resetFilters() {
    filters.search = '';
    filters.activo = 'Todos';
    filters.ultimoEstado = 'Todos';
  }

  function clearFilters() {
    resetFilters();
    draft.search = filters.search;
    draft.activo = filters.activo;
    draft.ultimoEstado = filters.ultimoEstado;
  }

  const filteredItems = computed(() => {
    const search = filters.search.trim().toLowerCase();

    return items.value.filter((it) => {
      const matchesSearch =
        !search ||
        it.nombre.toLowerCase().includes(search) ||
        (it.scriptPath ?? '').toLowerCase().includes(search);
      const matchesActivo =
        filters.activo === 'Todos' ||
        (filters.activo === 'Sí' ? it.activo === true : it.activo === false);
      const matchesEstado = filters.ultimoEstado === 'Todos' || it.ultimoEstado === filters.ultimoEstado;

      return matchesSearch && matchesActivo && matchesEstado;
    });
  });

  return {
    filters,
    activoOptions,
    estadoOptions,
    filtersDialog: dialog,
    filtersDraft: draft,
    activeFiltersCount,
    filteredItems,
    openFilters,
    closeFilters,
    applyFilters,
    clearFilters,
    resetFilters,
  };
}

