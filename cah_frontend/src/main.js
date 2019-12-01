import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import axios from 'axios'

Vue.config.productionTip = false

axios.interceptors.request.use(request => {
  if (store.state.jwtToken) request.headers['Authorization'] = 
     'Bearer ' + store.state.jwtToken
  return request
})

new Vue({
  router,
  store,
  axios,
  render: h => h(App)
}).$mount('#app')
