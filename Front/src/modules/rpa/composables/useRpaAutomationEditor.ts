import { computed, reactive } from 'vue';

import type { Automation, AutomationUpsertInput } from '@/modules/rpa/types';
import type { ToastColor } from '@/modules/rpa/composables/useRpaToast';

export function useRpaAutomationEditor(showToast: (message: string, color: ToastColor) => void) {
  const diasSemana = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

  function isAllDays(value: string[]) {
    const set = new Set(value);
    return diasSemana.every((d) => set.has(d));
  }

  const editor = reactive({
    open: false,
    mode: 'create' as 'create' | 'edit',
    editingId: null as number | null,
    form: {
      codigo: '',
      nombre: '',
      descripcion: null as string | null,
      tipo: 'RPA',
      entorno: 'PROD',
      scriptPath: '',
      horaEjecucion: '08:00',
      diasEjecucion: [] as string[],
      activo: true,
    },
  });

  function normalizeServerPath(path: string) {
    const trimmed = path.trim();
    if (!trimmed) return '';
    const replaced = trimmed.replace(/\//g, '\\');
    if (replaced.startsWith('\\\\')) {
      return '\\\\' + replaced.slice(2).replace(/\\+/g, '\\');
    }
    return replaced.replace(/\\+/g, '\\');
  }

  function isServerPath(path: string) {
    const value = path.trim();
    if (!value) return false;
    const normalized = value.replace(/\//g, '\\');
    const isDrive = /^[A-Za-z]:\\/.test(normalized);
    const isUnc = /^\\\\[^\\]+\\[^\\]+\\/.test(normalized);
    return isDrive || isUnc;
  }

  const scriptPathError = computed(() => {
    const value = editor.form.scriptPath.trim();
    if (!value) return 'Pega una ruta del servidor.';
    if (!isServerPath(value))
      return 'Debe ser una ruta del servidor (ej: C:\\RPA\\... o \\\\srv-rpa\\share\\...).';
    return '';
  });

  async function copyScriptPath() {
    const value = editor.form.scriptPath?.trim();
    if (!value) return;
    try {
      await navigator.clipboard.writeText(value);
      showToast('Ruta copiada', 'success');
    } catch {
      showToast('No fue posible copiar la ruta', 'error');
    }
  }

  async function pasteScriptPath() {
    try {
      const text = await navigator.clipboard.readText();
      const normalized = normalizeServerPath(text);
      editor.form.scriptPath = normalized;
      if (!normalized) {
        showToast('Portapapeles vacío', 'warning');
        return;
      }
      if (!isServerPath(normalized)) {
        showToast('La ruta pegada no parece ser del servidor', 'warning');
        return;
      }
      showToast('Ruta pegada', 'success');
    } catch {
      showToast('No fue posible leer el portapapeles; pega la ruta manualmente en el campo', 'info');
    }
  }

  function clearScriptSelection() {
    editor.form.scriptPath = '';
  }

  const canSave = computed(() => {
    return (
      editor.form.codigo.trim().length > 0 &&
      editor.form.nombre.trim().length > 0 &&
      !scriptPathError.value &&
      editor.form.horaEjecucion.trim().length > 0 &&
      editor.form.diasEjecucion.length > 0
    );
  });

  function openCreate() {
    editor.mode = 'create';
    editor.editingId = null;
    editor.form = {
      codigo: '',
      nombre: '',
      descripcion: null,
      tipo: 'RPA',
      entorno: 'PROD',
      scriptPath: '',
      horaEjecucion: '08:00',
      diasEjecucion: [],
      activo: true,
    };
    editor.open = true;
  }

  function openEdit(item: Automation) {
    editor.mode = 'edit';
    editor.editingId = item.autoId;
    editor.form = {
      autoId: item.autoId, // Añadimos el ID para que el editor sepa qué está editando
      codigo: item.codigo ?? '',
      nombre: item.nombre,
      descripcion: item.descripcion ?? null,
      tipo: item.tipo ?? 'RPA',
      entorno: item.entorno ?? 'PROD',
      scriptPath: item.scriptPath ?? '',
      horaEjecucion: item.horaEjecucion ?? '08:00',
      diasEjecucion: item.diasEjecucion ? [...item.diasEjecucion] : [],
      activo: item.activo ?? true,
    } as any;
    editor.open = true;
  }

  function closeEditor() {
    editor.open = false;
  }

  function buildUpsertInput(): AutomationUpsertInput {
    return {
      codigo: editor.form.codigo.trim(),
      nombre: editor.form.nombre.trim(),
      descripcion: editor.form.descripcion,
      tipo: editor.form.tipo,
      entorno: editor.form.entorno,
      scriptPath: normalizeServerPath(editor.form.scriptPath),
      horaEjecucion: editor.form.horaEjecucion,
      diasEjecucion: [...editor.form.diasEjecucion],
      activo: editor.form.activo,
    };
  }

  function validateBeforeSave() {
    if (canSave.value) return true;

    const codigo = editor.form.codigo.trim();
    const nombre = editor.form.nombre.trim();
    const hora = editor.form.horaEjecucion.trim();
    const dias = editor.form.diasEjecucion;
    const pathError = scriptPathError.value;

    const parts: string[] = [];
    if (!codigo) parts.push('Código');
    if (!nombre) parts.push('Nombre');
    if (pathError) parts.push('Script');
    if (!hora) parts.push('Hora');
    if (!dias.length) parts.push('Días');

    if (pathError) {
      showToast(pathError, 'warning');
      return false;
    }

    showToast(`Completa los campos: ${parts.join(', ')}`, 'warning');
    return false;
  }

  return {
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
    buildUpsertInput,
    validateBeforeSave,
  };
}

