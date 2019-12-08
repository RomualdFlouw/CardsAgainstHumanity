import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import axios from 'axios'
import lobbyHub from './lobby-hub'

Vue.config.productionTip = false

axios.interceptors.request.use(request => {
  if (store.state.jwtToken) request.headers['Authorization'] = 
     'Bearer ' + store.state.jwtToken
  return request
})

// Install Vue extensions
Vue.use(lobbyHub)

new Vue({
  router,
  store,
  axios,
  render: h => h(App)
}).$mount('#app')
