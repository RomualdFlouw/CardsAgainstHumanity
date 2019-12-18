<template>
    <div class="full_size">
        <div class="header">
            <div class="header_logout" v-on:click="logoutFunction()">
                <svg class="" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M16 17l5-5-5-5M19.8 12H9M10 3H4v18h6"/></svg>
            </div>
        </div>        
        <div v-show="cardCzar" class="game_content">
            <div class="game_content__information">
                <div class="information_top">
                    <h3>{{$t("GAME_POINTS")}}{{currentPoints}}</h3>
                </div>
                <h2 class="information_player">You are the card Czar</h2>
                <h3 v-show="IsRoundStarting">{{$t("GAME_TIME")}} {{roundVisualCounter}}</h3>
                <h3 v-show="IsChooserStarting">{{$t("GAME_TIME")}} {{chooserVisualCounter}}</h3>
            </div>
            <div class="game_content__placement">
                <div class="placement_black">
                    <h3 class="cards_title_white">{{blackCardValue}}</h3>
                </div>
                <div v-show="!chosenCard" class="placement_dotted">
                    <h3 class="cards_title_white">other players are choosing cards, as soon as the cards appear choose your favorite and submit.</h3>
                </div>                
                <div v-show="chosenCard" class="placement_white">
                    <h3 class="cards_title_black">{{chosenCardText}}</h3>
                </div>
            </div>
            <input v-show="chosenCard" v-on:click="sendChosenCardCzar(chosenCard)" class="game_content__submit" type="submit" :value="`${this.$t('GAME_CONFIRM')}`"> 
            <div class="game_content__cards">
                <div class="cards_container">
                    <cards @clicked="onClickChild" v-for="c in ToChooseCards" :key="c.id"  :card="c" />
                </div>
            </div>
        </div>
        <div v-show="!cardCzar" class="game_content">
            <div class="game_content__information">
                <div class="information_top">
                    <h3>{{$t("GAME_POINTS")}}{{currentPoints}}</h3>
                </div>
                <h2 class="information_player">{{currentChooser}} {{$t("GAME_CZAR")}}</h2>
                <h3 v-show="IsRoundStarting">{{$t("GAME_TIME")}} {{roundVisualCounter}}</h3>
                <h3 v-show="IsChooserStarting">{{$t("GAME_TIME")}} {{chooserVisualCounter}}</h3>
            </div>
            <div class="game_content__placement">
                <div class="placement_black">
                    <h3 class="cards_title_white">{{blackCardValue}}</h3>
                </div>
                <div v-show="!chosenCard" class="placement_dotted">
                    <h3 class="cards_title_white">{{$t("GAME_CHOOSING")}}</h3>
                </div>                
                <div v-show="chosenCard" class="placement_white">
                    <h3 class="cards_title_black">{{chosenCardText}}</h3>
                </div>
            </div>
            <input v-show="chosenCard" v-on:click="sendChosenCard(chosenCard)" class="game_content__submit" type="submit" :value="`${this.$t('GAME_CONFIRM')}`"> 
            <div class="game_content__cards">
                <div class="cards_container">
                    <cards @clicked="onClickChild" v-for="c in Playercards" :key="c.id"  :card="c" />
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import cards from '../components/Cards';
import Vue from 'vue'
export default { 
    name: 'game',
    data(){
        return{
            currentPoints: 0,
            currentChooser: "",
            cardCzar: false,
            blackCardValue: "",
            Playercards:[
            ],
            ToChooseCards:[],

            chosenCard: false,
            chosenCardText: "",

            roundCountdown: 50,
            roundVisualCounter: 50,
            roundTimerId: null,

            chooserCountdown: 30,
            chooserVisualCounter: 30,
            chooserTimerId: null,

            IsRoundStarting: true,
            IsChooserStarting: false,
        }
    },
    components:{
      cards
    },
    methods:{
        logoutFunction: function(){
            this.$store.dispatch('logout');
            window.localStorage.removeItem('readyState');
            this.$router.push('login');
        },
        onClickChild: function (value) {
            this.chosenCard = true // someValue
            this.chosenCard = value;
            this.chosenCardText = value.cardText;
            //doorsturen naar backend
        },
        sendChosenCard: function (value){
            console.log("Selected Card")
            console.log(value);
            this.$gameHub.ClientSelectedCard(value);

        },
        sendChosenCardCzar: function (value){
            console.log("Selected Card")
            console.log(value);
            this.$gameHub.ClientSelectedCard(value);

        },
        startCountdownRound: function(){
            this.roundTimerId = setInterval(this.countdown(this.roundCountdown, this.roundTimerId, this.roundVisualCounter, "start-chooser-round"), 1000);
        },
        startCountdownChooser: function(){
            this.chooserTimerId = setInterval(this.chooserCountdown, this.chooserTimerId, this.chooserVisualCounter, "send-chosen-card");
        },
        countdown: function(typeCountdown, timerId, visualCounter, message){
            if (typeCountdown == 0) {
                clearInterval(timerId);
                this.$emit(message);
                
            } else {
                typeCountdown--;
                visualCounter = typeCountdown;
            }
        },
        ReceiveStartingHand: function (hand){
            this.Playercards = hand;
            console.log(this.Playercards);
        },
        ReceiveRoundInfo: function (info){
            console.log(info);
            this.blackCardValue = info.blackCard.cardText;
            this.currentChooser = info.czar;
            console.log(info.player.isCzar);
            this.cardCzar = info.player.isCzar;
        },
        GameRound: function() {
            // TODO
            // 1. Ask the backend for round info (what black card, who the czar is etc)
            this.$gameHub.GetRoundInfo();
            // TODO
            // 2. Check if Czar, if so act accordingly

            // 3. Start a 30 second countdown

            // 4. If timer is done, send random card 
        },
        PickWinner: function(cards){
            console.log("Has to pick winner");
            console.log(cards);
        }
    },
    created(){
        this.$gameHub.$on("receive-starting-hand", this.ReceiveStartingHand);
        this.$gameHub.$on("receive-round-info", this.ReceiveRoundInfo);
        this.$gameHub.$on("pick-winner", this.PickWinner);
        // Get Starting hand
        this.$gameHub.GetStartingHand();
        this.$gameHub.GetRoundInfo();
        
    }
}
</script>
