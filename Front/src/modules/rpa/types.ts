export type UltimoEstado = 'Exitosa' | 'Error' | 'Pendiente';

export interface Automation {
  id: number;
  nombre: string;
  scriptPath: string;
  horaEjecucion: string;
  diasEjecucion: string[];
  activo: boolean;
  ultimoEstado: UltimoEstado;
  ultimaEjecucion: string | null;
}

export interface AutomationUpsertInput {
  nombre: string;
  scriptPath: string;
  horaEjecucion: string;
  diasEjecucion: string[];
  activo: boolean;
}

