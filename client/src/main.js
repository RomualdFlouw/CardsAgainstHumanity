import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import axios from 'axios'
import lobbyHub from './lobby-hub'
import gameHub from './game-hub'
import {i18n} from './plugins/i18n'
// import * as Sentry from '@sentry/browser';
// import * as Integrations from '@sentry/integrations';

Vue.config.productionTip = false

Sentry.init({
  dsn: 'https://77832e51ffab4f0bb3798483db18d237@sentry.io/1859904',
  integrations: [new Integrations.Vue({Vue, attachProps: true})],
});

axios.interceptors.request.use(request => {
  if (store.state.jwtToken) request.headers['Authorization'] = 
     'Bearer ' + store.state.jwtToken
  return request
})

// Install Vue extensions
Vue.use(lobbyHub)
Vue.use(gameHub)

new Vue({
  i18n,
  Sentry,
  router,
  store,
  axios,
  render: h => h(App)
}).$mount('#app')
