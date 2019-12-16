import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import axios from 'axios'
import lobbyHub from './lobby-hub'
import gameHub from './game-hub'
import {i18n} from './plugins/i18n'

Vue.config.productionTip = false

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
  router,
  store,
  axios,
  render: h => h(App)
}).$mount('#app')
