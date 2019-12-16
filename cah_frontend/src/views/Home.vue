<template>
    <div class="home">
      <div class="home_header">
        <router-link class="header_logout" v-on:click="logoutFunction" to="">
          <svg class="" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M16 17l5-5-5-5M19.8 12H9M10 3H4v18h6"/></svg>
        </router-link>
      </div>
        <h1 class="logo">{{$t("GAME_TITLE")}}</h1>
        <div class="home_content">
          <lobby v-on:StartingGame="StartGame" :nickname="this.$route.params.nickname"/>
        </div>
    </div>
</template>
<script>
import Lobby from '../components/Lobby';

export default {
    name: 'home',
    data(){
        return{
            nickname: this.$route.params.nickname
        }
    },
    components:{
      Lobby
    },
    methods:{
        logoutFunction: function(){
          // this.$store.dispatch('logout');
          this.$router.back('login');
        },
        StartGame: function(){
          console.log("Starting game");
          //this.$router.push({ path: `game` });
          this.$router.push({ path: '/game' });
        }
    }
}
</script>
