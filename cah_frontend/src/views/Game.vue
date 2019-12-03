<template>
    <div class="game">
        <div class="game_header">
            <router-link class="header_logout" v-on:click="logoutFunction()" to="/">
            <svg class="" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M16 17l5-5-5-5M19.8 12H9M10 3H4v18h6"/></svg>
            </router-link>
        </div>
        <div class="game_content">
            <div class="game_content__information">
                <div class="information_top">
                    <h3>round {{currentRound}}/10</h3>
                    <h3>you are {{currentPosition}}st</h3>
                </div>
                <div class="information_middle">
                    
                </div>
                <div class="information_time">

                </div>
            </div>
            <div class="game_content__placement">

            </div>
            <div class="game_content__cards">

            </div>
        </div>
    </div>
</template>
<script>
import Lobby from '../components/Lobby';

export default {
    name: 'game',
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
            this.$store.dispatch('logout');
            this.$router.back('login');
            window.localStorage.removeItem('readyState');
        }
    }
}
</script>
