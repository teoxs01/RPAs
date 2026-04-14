import type { Automatizacion, AutomatizacionCreateInput } from '@/modules/rpa/types';

import { requestJson } from '@/modules/rpa/services/http';

export async function listAutomatizaciones() {
  const data = await requestJson<Automatizacion[]>('/api/rpa/automatizaciones', { method: 'GET' });
  return Array.isArray(data) ? data : [];
}

export async function getAutomatizacion(id: number) {
  return requestJson<Automatizacion>(`/api/rpa/automatizaciones/${id}`, { method: 'GET' });
}

export async function createAutomatizacion(input: AutomatizacionCreateInput) {
  return requestJson<Automatizacion>('/api/rpa/automatizaciones', {
    method: 'POST',
    body: JSON.stringify(input),
  });
}

export async function updateAutomatizacion(id: number, input: AutomatizacionCreateInput) {
  return requestJson(`/api/rpa/automatizaciones/${id}`, {
    method: 'PUT',
    body: JSON.stringify(input),
  });
}

export async function deleteAutomatizacion(id: number) {
  return requestJson(`/api/rpa/automatizaciones/${id}`, { method: 'DELETE' });
}

export async function saveConfiguracion(id: number, input: any) {
  return requestJson(`/api/rpa/automatizaciones/${id}/configuracion`, {
    method: 'POST',
    body: JSON.stringify(input),
  });
}
