<template>
<div class="lobby">
    <div >
        <lobbymembers  v-for="p in players" :key="p.id" :username="p.name" :isReady="p.readyState"/>
    </div>
    <input v-show="!isReady" v-on:click="getReady()" class="button" type="submit" value="Ready" >
    <h2 v-show="isReady">waiting for everyone to get ready...</h2>
    <input v-show="isReady" v-on:click="getReady()" class="red_button" type="submit" value="UnReady" >

</div>

</template> 

<script>
import lobbymembers from '../components/lobbymembers'
export default {
    name:'lobby',
    data(){
        return{
            players:[],
            isReady: false,
            gameStarting: false
        }
    },
    props:{
        nickname: String,
    },
    components:{
        lobbymembers
    },
    methods:{
        getReady: function(){
            //this.$router.push({ path: `/game/${this.nickname}` })
            if (this.gameStarting){
                return;
            };
            this.$lobbyHub.ClientReadyStateChange({Name: this.nickname, ReadyState: !this.isReady});
        },
        UpdateLobby: function(users){
            console.log(users);
            this.players = users;
        },
        UpdateReadyState: function(){
            this.isReady = !this.isReady;
            localStorage.setItem('readyState', this.isReady);
        },
        StartCountdownToGame: function(){
            console.log("STARTING COUNTDOWN TO GAME");
            this.gameStarting = true;
            //TODO ADD COUNTDOWN TIMER
            //TODO DISABLE BUTTON
        },
        StopCountdownToGame: function(){
            console.log("STOPPING COUNTDOWN TO GAME");
            this.gameStarting = false;
            //TODO ADD COUNTDOWN TIMER
            //TODO DISABLE BUTTON
        }


    },
    created(){
        localStorage.setItem('readyState', false);
        this.$lobbyHub.$on("update-lobby", this.UpdateLobby);
        this.$lobbyHub.$on("update-ready-state", this.UpdateReadyState)
        this.$lobbyHub.$on("start-countdown-game", this.StartCountdownToGame)
        this.$lobbyHub.$on("stop-countdown-game", this.StopCountdownToGame)
        this.$lobbyHub.GetAllPlayers();
    }
}
</script>