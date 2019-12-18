import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../views/Login.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'login',
    component: Login
  },{
    path: '/home',
    name: 'home',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Home.vue')
  },
  {
    path: '/game',
    name: 'game',
    // route level code-splitting
    // this generates a separate chunk (game.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "game" */ '../views/Game.vue')
  },
  {
    path: '/leaderboard',
    name: 'leaderboard',
    // route level code-splitting
    // this generates a separate chunk (leaderboard.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "leaderboard" */ '../views/Leaderboard.vue')
  },
  {
    path: '/settings',
    name: 'settings',
    // route level code-splitting
    // this generates a separate chunk (Settings.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "Settings" */ '../views/Settings.vue')
  },
  {
    path: '*',
    name: 'error',
    // route level code-splitting
    // this generates a separate chunk (error.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "error" */ '../views/Error.vue')
  },
]

const router = new VueRouter({
  routes
})

export default router
