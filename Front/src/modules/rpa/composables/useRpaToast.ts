import { reactive } from 'vue';

export type ToastColor = 'success' | 'error' | 'warning' | 'info';

export function useRpaToast() {
  const toast = reactive({
    open: false,
    message: '',
    color: 'success' as ToastColor,
    timeout: 3500,
  });

  function showToast(message: string, color: ToastColor) {
    toast.message = message;
    toast.color = color;
    toast.open = true;
  }

  return { toast, showToast };
}

