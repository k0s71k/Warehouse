import { createRouter, createWebHistory } from 'vue-router' 

const routes = [
  {
    path: '/',
    component: () => import("@/pages/HomePage.vue")
  },
  {
    path: '/clients',
    component: () => import("@/pages/ClientsPage.vue")
  },
  {
    path: '/clients/add',
    component: () => import("@/pages/AddClientPage.vue")
  },
  {
    path: '/measure-units',
    component: () => import("@/pages/MeasureUnitsPage.vue")
  },
  {
    path: '/measure-units/add',
    component: () => import("@/pages/AddMeasureUnitPage.vue")
  },
  {
    path: '/resources',
    component: () => import("@/pages/ResourcesPage.vue")
  },
  {
    path: '/resources/add',
    component: () => import("@/pages/AddResourcePage.vue")
  },
  {
    path: '/balances',
    component: () => import("@/pages/BalancesPage.vue")
  },
  {
    path: '/receive-documents',
    component: () => import("@/pages/ReceiveDocumentsPage.vue")
  },
  {
    path: '/receive-documents/add',
    component: () => import("@/pages/AddReceiveDocumentPage.vue")
  },
  {
    path: '/receive-documents/:id',
    component: () => import("@/pages/ReceiveDocumentDetailsPage.vue")
  },
  {
    path: '/send-documents',
    component: () => import("@/pages/SendDocumentsPage.vue")
  },
  {
    path: '/send-documents/add',
    component: () => import("@/pages/AddSendDocumentPage.vue")
  },
  {
    path: '/send-documents/:id',
    component: () => import("@/pages/SendDocumentDetailsPage.vue")
  },
  {
    path: '/not-found',
    component: () => import('@/pages/NotFoundPage.vue')
  },
  {
    path: '/:catchAll(.*)',
    redirect: 'not-found'
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
