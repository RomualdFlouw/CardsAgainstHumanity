<template>
    <div class="home">
        <h1 class="logo">{{$t("GAME_TITLE")}}</h1>
        <div class="content">
            <h2 class="leaderboard_header">The funniest members are...</h2>
            <leaderboardmembers v-for="m in members" :key="m.id" :position="m.position" :username="m.username" :points="m.points"></leaderboardmembers>
            <div class="Leaderboard_buttons">
                <input class="button" type="submit" :value="`${this.$t('GAME_RESTART')}`" > 
                <input class="red_button" type="submit" :value="`${this.$t('GAME_DISCONNECT')}`" >
            </div>
        </div>
    </div>
</template>

<script>
import leaderboardmembers from '../components/leaderboardmembers'
import Vue from 'vue'

export default {
    name:'leaderboard',
    data(){
        return{
            members:[
                {position: 1, username : "Romuald", points : 6},
                {position: 2, username : "Jasper", points : 2},
                {position: 3, username : "Ayron", points : 1},
            ],
        }
    },
    props:{
    },
    components:{
        leaderboardmembers
    },
    methods:{
    },
    created(){
    }
}
</script>

<style lang="scss">
.leaderboard_header{
    margin-bottom: 10%;
}
.Leaderboard_buttons{
    width: 100%;
    text-align: center;
    margin-top: 10%
}
</style>