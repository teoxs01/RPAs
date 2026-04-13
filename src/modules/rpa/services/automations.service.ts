import type { Automation, AutomationUpsertInput } from '@/modules/rpa/types';

import { requestJson } from '@/modules/rpa/services/http';

export async function listAutomations() {
  const data = await requestJson<Automation[]>('/api/rpa/automatizaciones', { method: 'GET' });
  return Array.isArray(data) ? data : [];
}

export async function createAutomation(input: AutomationUpsertInput) {
  return requestJson('/api/rpa/automatizaciones', {
    method: 'POST',
    body: JSON.stringify(input),
  });
}

export async function updateAutomation(id: number, input: AutomationUpsertInput) {
  return requestJson(`/api/rpa/automatizaciones/${id}`, {
    method: 'PUT',
    body: JSON.stringify(input),
  });
}

export async function setAutomationActive(id: number, activo: boolean) {
  return requestJson(`/api/rpa/automatizaciones/${id}/activo`, {
    method: 'PATCH',
    body: JSON.stringify({ activo }),
  });
}

export async function executeAutomation(id: number) {
  return requestJson(`/api/rpa/automatizaciones/${id}/ejecutar`, { method: 'POST' });
}

export async function deleteAutomation(id: number) {
  return requestJson(`/api/rpa/automatizaciones/${id}`, { method: 'DELETE' });
}

