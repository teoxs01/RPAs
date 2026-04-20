export const UltimoEstado = {
  OK: 'OK',
  ERROR: 'ERROR',
  EN_PROCESO: 'EN_PROCESO',
  PENDIENTE: 'Pendiente'
} as const;

export type UltimoEstado = typeof UltimoEstado[keyof typeof UltimoEstado];

export interface Automatizacion {
  autoId: number;
  codigo: string;
  nombre: string;
  descripcion: string | null;
  tipo: string;
  entorno: string | null;
  version: string | null;
  estado: string | null;
  usuaCrea: number;
  fechCrea: string;
  usuaModi: number | null;
  fechModi: string | null;
  configuracion: ConfiguracionProceso | null;
}

export interface ConfiguracionProceso {
  id: number;
  autoId: number;
  nombre: string;
  scriptPath: string;
  horaEjecucion: string;
  diasEjecucion: string[];
  activo: boolean;
}

export interface Automation {
  autoId: number;
  codigo: string;
  nombre: string;
  descripcion: string | null;
  tipo: string;
  entorno: string | null;
  estado: string | null;
  scriptPath?: string;
  horaEjecucion?: string;
  diasEjecucion?: string[];
  activo?: boolean;
  ultimoEstado: UltimoEstado;
  ultimaEjecucion: string | null;
  // Nuevos campos
  appName?: string | null;
  appUser?: string | null;
  appPassword?: string | null;
  appUrl?: string | null;
}

export interface AutomationUpsertInput {
  codigo: string;
  nombre: string;
  descripcion: string | null;
  tipo: string;
  entorno?: string;
  scriptPath?: string;
  horaEjecucion?: string;
  diasEjecucion?: string[];
  activo?: boolean;
  // Nuevos campos
  appName?: string | null;
  appUser?: string | null;
  appPassword?: string | null;
  appUrl?: string | null;
}

export interface AutomatizacionCreateInput {
  codigo: string;
  nombre: string;
  descripcion: string | null;
  tipo: string;
  entorno?: string;
}
