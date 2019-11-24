import { RouteConfig } from 'vue-router'

import auth from './auth'

const routes: Array<RouteConfig> = [
  {
    path: '/',
    component: () => import('@/layouts/MyLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('@/pages/Index.vue'),
        beforeEnter: auth
      },
      {
        path: '/models/:name',
        component: () => import('@/pages/models/Index.vue'),
        name: 'models',
        props: true,
        beforeEnter: auth
      },
      {
        path: '/administration/:name',
        component: () => import('@/pages/models/Index.vue'),
        name: 'administration',
        props: true,
        beforeEnter: auth
      },
      { path: '/register', component: () => import('@/pages/authentication/Register.vue') },
      { path: '/login', component: () => import('@/pages/authentication/Login.vue') }
    ]
  }
]

if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('@/pages/Error404.vue')
  })
}

export default routes
