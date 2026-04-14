import { onMounted, ref } from 'vue';

import type { Automation, AutomationUpsertInput } from '@/modules/rpa/types';
import type { ToastColor } from '@/modules/rpa/composables/useRpaToast';

import {
  createAutomation,
  deleteAutomation,
  executeAutomation,
  listAutomations,
  setAutomationActive,
  updateAutomation,
} from '@/modules/rpa/services/automations.service';

export function useRpaAutomations(showToast: (message: string, color: ToastColor) => void) {
  const items = ref<Automation[]>([]);
  const tableLoading = ref(false);
  const rowBusy = ref(new Set<number>());

  async function load() {
    tableLoading.value = true;
    try {
      items.value = await listAutomations();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible cargar automatizaciones';
      showToast(message, 'error');
      items.value = [];
    } finally {
      tableLoading.value = false;
    }
  }

  async function create(input: AutomationUpsertInput) {
    tableLoading.value = true;
    try {
      await createAutomation(input);
      showToast('Automatización creada', 'success');
      await load();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible guardar';
      showToast(message, 'error');
      throw e;
    } finally {
      tableLoading.value = false;
    }
  }

  async function update(id: number, input: AutomationUpsertInput) {
    tableLoading.value = true;
    try {
      await updateAutomation(id, input);
      showToast('Automatización actualizada', 'success');
      await load();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible guardar';
      showToast(message, 'error');
      throw e;
    } finally {
      tableLoading.value = false;
    }
  }

  async function toggleActive(item: Automation) {
    if (rowBusy.value.has(item.autoId)) return;
    rowBusy.value.add(item.autoId);
    try {
      await setAutomationActive(item.autoId, !item.activo);
      showToast(!item.activo ? 'Automatización activada' : 'Automatización desactivada', 'info');
      await load();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible actualizar el estado';
      showToast(message, 'error');
    } finally {
      rowBusy.value.delete(item.autoId);
    }
  }

  async function execute(item: Automation) {
    if (rowBusy.value.has(item.autoId)) return;
    rowBusy.value.add(item.autoId);
    try {
      await executeAutomation(item.autoId);
      showToast('Ejecución enviada', 'success');
      await load();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible ejecutar';
      showToast(message, 'error');
    } finally {
      rowBusy.value.delete(item.autoId);
    }
  }

  async function remove(id: number) {
    if (rowBusy.value.has(id)) return;
    rowBusy.value.add(id);
    try {
      await deleteAutomation(id);
      showToast('Automatización eliminada', 'success');
      await load();
    } catch (e) {
      const message = e instanceof Error ? e.message : 'No fue posible eliminar';
      showToast(message, 'error');
      throw e;
    } finally {
      rowBusy.value.delete(id);
    }
  }

  onMounted(() => {
    void load();
  });

  return {
    items,
    tableLoading,
    rowBusy,
    load,
    create,
    update,
    toggleActive,
    execute,
    remove,
  };
}

