export type UltimoEstado = 'Exitosa' | 'Error' | 'Pendiente';

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
}

export interface AutomatizacionCreateInput {
  codigo: string;
  nombre: string;
  descripcion: string | null;
  tipo: string;
  entorno?: string;
}
