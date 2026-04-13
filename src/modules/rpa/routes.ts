import type { RouteRecordRaw } from 'vue-router';

import RpaDashboardPage from '@/modules/rpa/pages/RpaDashboardPage.vue';
import RpaAutomationsPage from '@/modules/rpa/pages/RpaAutomationsPage.vue';

export const rpaRoutes: RouteRecordRaw[] = [
  { path: '/rpa', name: 'RpaDashboard', component: RpaDashboardPage },
  { path: '/rpa/automatizaciones', name: 'RpaAutomations', component: RpaAutomationsPage },
];

