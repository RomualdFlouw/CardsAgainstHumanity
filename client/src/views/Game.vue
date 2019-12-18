<template>
    <div class="full_size">
        <div class="header">
            <div class="header_logout" v-on:click="logoutFunction()">
                <svg class="" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M16 17l5-5-5-5M19.8 12H9M10 3H4v18h6"/></svg>
            </div>
        </div>
        <div class="game_content">
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

            blackCardValue: "What is the one thing obama is good at? ___",
            Playercards:[
                {cardText: "this is the text dzada sd asdazdasd azd ", cardIndex:0},
                {cardText: "this is the text", cardIndex:1},
                {cardText: "this is the text", cardIndex:2},
                {cardText: "this is the text", cardIndex:3},
                {cardText: "this is the text", cardIndex:4},
                {cardText: "this is the text", cardIndex:5},
                {cardText: "this is the text", cardIndex:6},
                {cardText: "this is the text", cardIndex:7},
                {cardText: "this is the text", cardIndex:8},
                {cardText: "this is the text", cardIndex:9}
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
            console.log(hand);
        },
        ReceiveRoundInfo: function (info){
            console.log(info);
            this.blackCardValue = info.blackCard.cardText;
            this.currentChooser = info.czar;
        },
        GameRound: function() {
            // TODO
            // 1. Ask the backend for round info (what black card, who the czar is etc)
            this.$gameHub.GetRoundInfo();
            // TODO
            // 2. Check if Czar, if so act accordingly

            // 3. Start a 30 second countdown

            // 4. If timer is done, send random card 
        }
    },
    created(){
        this.$gameHub.$on("receive-starting-hand", this.ReceiveStartingHand);
        this.$gameHub.$on("receive-round-info", this.ReceiveRoundInfo);
        // Get Starting hand
        this.$gameHub.GetStartingHand();
        this.$gameHub.GetRoundInfo();
        
    }
}
</script>
