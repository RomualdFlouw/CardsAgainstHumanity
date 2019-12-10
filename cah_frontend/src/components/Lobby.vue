<template>
<div class="lobby">
    <div>
        <lobbymembers  v-for="p in players" :key="p.id" :username="p.name" :isReady="p.readyState"/>
    </div>
    <div v-show="!gameStarting">
        <input v-show="!isReady" v-on:click="getReady()" class="button" type="submit" :value="`${this.$t('INPUT_BUTTON_READY')}`" >
        <h2 v-show="isReady">{{$t("LOBBY_READY_TEXT")}}</h2>
        <input v-show="isReady" v-on:click="getReady()" class="red_button" type="submit" :value="`${this.$t('INPUT_BUTTON_UNREADY')}`" >
    </div>
    <h2 v-show="gameStarting">{{$t("GAME_STARTING")}} {{visualCounter}} secs...</h2>


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
            gameStarting: false,
            gameCountdown: 15,
            visualCounter: null
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
            this.gameStarting = true;
            var timerId = setInterval(countdown, 1000);
        },
        countdown: function(){
            if (this.gameCountdown == 0) {
                clearTimeout(this.timerId);
                this.$router.push({ path: `game` });
            } else {
                this.visualCounter = this.gameCountdown;
                this.gameCountdown--;
            }
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