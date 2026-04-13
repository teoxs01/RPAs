import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

import { rpaRoutes } from '@/modules/rpa';

const routes: RouteRecordRaw[] = [
  { path: '/', redirect: '/rpa' },
  ...rpaRoutes,
  { path: '/:pathMatch(.*)*', redirect: '/rpa' },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
