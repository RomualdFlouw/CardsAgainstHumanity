import Vue from 'vue'
import axios from 'axios'

export default{
    state: {
        profile: {},
        jwtToken: "null"
      },
      mutations: {
        setJwtToken (state, jwtToken) {
            state.jwtToken = jwtToken 
            if (jwtToken) window.localStorage.setItem('jwtToken', jwtToken)
            else window.localStorage.removeItem('jwtToken')
          },
          setProfile (state, profile) {
            state.profile = profile 
            if (profile) window.localStorage.setItem('profile', JSON.stringify(profile))
            else window.localStorage.removeItem('profile')
          }
      },
      actions: {
        // Login methods. Either use cookie-based auth or jwt-based auth
        login ({ state, dispatch }, { credentials }) {
          const loginAction = dispatch('loginToken', credentials)
        },
        loginToken ({ commit }) {
          return axios.post('https://localhost:44395/api/Auth/token').then(res => {
            const profile = res.data
            const jwtToken = res.data.token
            delete profile.token
            commit('setProfile', res.data)
            commit('setJwtToken', jwtToken) 
            Vue.prototype.startSignalR(jwtToken)
          })
        },
        logout ({ commit, state }) {
            const logoutAction = state.jwtToken
              ? Promise.resolve()
              : axios.post('account/logout')
           
            return logoutAction.then(() => {
              commit('setProfile', {})
              commit('setJwtToken', null)
              return Vue.prototype.stopSignalR()
            })
          },
          restoreContext ({ commit, getters, state }) {
            const jwtToken = window.localStorage.getItem('jwtToken')
            if (jwtToken)
              commit('setJwtToken', jwtToken)
         
            return axios.get('account/context').then(res => {
              commit('setProfile', res.data)
              if (getters.isAuthenticated) return Vue.prototype.startSignalR()
            })
          },
      }
}