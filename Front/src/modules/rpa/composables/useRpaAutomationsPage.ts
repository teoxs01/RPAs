import { computed, reactive } from 'vue';

import { type Automation, UltimoEstado } from '@/modules/rpa/types';

import { useRpaAutomationEditor } from '@/modules/rpa/composables/useRpaAutomationEditor';
import { useRpaAutomations } from '@/modules/rpa/composables/useRpaAutomations';
import { useRpaAutomationsFilters } from '@/modules/rpa/composables/useRpaAutomationsFilters';
import { useRpaToast } from '@/modules/rpa/composables/useRpaToast';

export function useRpaAutomationsPage() {
  const headers = [
    { title: 'Nombre', key: 'nombre' },
    { title: 'Activo', key: 'activo', sortable: true },
    { title: 'Último estado', key: 'ultimoEstado', sortable: true },
    { title: 'Última ejecución', key: 'ultimaEjecucion', sortable: true },
    { title: 'Hora ejecución', key: 'horaEjecucion', sortable: true },
    { title: 'Días ejecución', key: 'diasEjecucion', sortable: false },
    { title: '', key: 'acciones', sortable: false, align: 'end' as const },
  ];

  const { toast, showToast } = useRpaToast();
  const automations = useRpaAutomations(showToast);
  const filters = useRpaAutomationsFilters(automations.items);
  const editor = useRpaAutomationEditor(showToast);

  const stats = computed(() => {
    const total = automations.items.value.length;
    const activas = automations.items.value.filter((x) => x.activo).length;
    const errores = automations.items.value.filter((x) => x.ultimoEstado === UltimoEstado.ERROR).length;
    const pendientes = automations.items.value.filter((x) => x.ultimoEstado === UltimoEstado.PENDIENTE).length;

    return { total, activas, errores, pendientes };
  });

  function estadoColor(value: string | UltimoEstado) {
    if (value === UltimoEstado.OK) return 'success';
    if (value === UltimoEstado.ERROR) return 'error';
    if (value === UltimoEstado.EN_PROCESO) return 'info';
    return 'warning';
  }

  function formatDateTime(value: string | null) {
    if (!value) return '-';
    const d = new Date(value);
    if (Number.isNaN(d.getTime())) return value;
    return d.toLocaleString('es-CO', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
    });
  }

  async function toggleActivo(item: Automation) {
    await automations.toggleActive(item);
  }

  async function executeManual(item: Automation) {
    await automations.execute(item);
  }

  function saveEditor() {
    if (!editor.validateBeforeSave()) return;
    void saveEditorToApi();
  }

  async function saveEditorToApi() {
    const payload = editor.buildUpsertInput();
    try {
      if (editor.editor.mode === 'create') {
        await automations.create(payload);
      } else {
        await automations.update(editor.editor.editingId as number, payload);
      }
      editor.editor.open = false;
    } catch {
    }
  }

  const deleter = reactive({
    open: false,
    target: null as Automation | null,
  });

  function openDelete(item: Automation) {
    deleter.target = item;
    deleter.open = true;
  }

  function closeDelete() {
    deleter.open = false;
    deleter.target = null;
  }

  function confirmDelete() {
    if (!deleter.target) return;
    void removeAutomation(deleter.target.autoId);
  }

  async function removeAutomation(id: number) {
    try {
      await automations.remove(id);
      closeDelete();
    } catch {
    }
  }

  return {
    headers,


    toast,

    items: automations.items,
    tableLoading: automations.tableLoading,
    rowBusy: automations.rowBusy,

    activoOptions: filters.activoOptions,
    estadoOptions: filters.estadoOptions,
    filtersDialog: filters.filtersDialog,
    filtersDraft: filters.filtersDraft,
    activeFiltersCount: filters.activeFiltersCount,
    filteredItems: filters.filteredItems,
    openFilters: filters.openFilters,
    closeFilters: filters.closeFilters,
    applyFilters: filters.applyFilters,
    clearFilters: filters.clearFilters,

    stats,
    estadoColor,
    formatDateTime,

    diasSemana: editor.diasSemana,
    isAllDays: editor.isAllDays,
    editor: editor.editor,
    scriptPathError: editor.scriptPathError,
    copyScriptPath: editor.copyScriptPath,
    pasteScriptPath: editor.pasteScriptPath,
    clearScriptSelection: editor.clearScriptSelection,
    openCreate: editor.openCreate,
    openEdit: editor.openEdit,
    closeEditor: editor.closeEditor,
    saveEditor,

    deleter,
    openDelete,
    closeDelete,
    confirmDelete,

    toggleActivo,
    executeManual,
  };
}

