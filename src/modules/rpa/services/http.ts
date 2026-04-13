function getApiBaseUrl() {
  return (import.meta.env.VITE_API_BASE_URL as string | undefined)?.replace(/\/+$/, '') ?? '';
}

function apiUrl(path: string) {
  const base = getApiBaseUrl();
  if (!path.startsWith('/')) return base ? `${base}/${path}` : `/${path}`;
  return base ? `${base}${path}` : path;
}

export async function requestJson<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(apiUrl(path), {
    ...init,
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
      ...(init?.headers ?? {}),
    },
  });

  const text = await res.text();
  let data: unknown = null;
  if (text) {
    try {
      data = JSON.parse(text) as unknown;
    } catch {
      data = text;
    }
  }

  if (!res.ok) {
    const message =
      typeof data === 'object' && data && 'message' in data ? String((data as any).message) : res.statusText;
    throw new Error(message || 'Error de servidor');
  }

  return data as T;
}

