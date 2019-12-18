<template>
    <div class="full_size">
        <div class="header">
            <div class="header_logout" v-on:click="logoutFunction()" to="/">
                <svg class="" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M16 17l5-5-5-5M19.8 12H9M10 3H4v18h6"/></svg>
            </div>
        </div>
        <div class="game_content">
            <div class="game_content__information">
                <div class="information_top">
                    <h3>{{$t("GAME_POINTS")}}{{currentPoints}}</h3>
                </div>
                <h2 class="information_player">{{currentChooser}} {{$t("GAME_CZAR")}}</h2>
                <h3>{{$t("GAME_TIME")}} {{currentTimer}}</h3>
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
            <input v-show="chosenCard" v-on:click="sendChosenCard(chosenCardText)" class="game_content__submit" type="submit" :value="`${this.$t('GAME_CONFIRM')}`"> 
            <div class="game_content__cards">
                <div class="cards_container">
                    <cards @clicked="onClickChild" v-for="c in cards" :key="c.id" :cardText="c.cardText" :cardIndex="c.cardIndex" />
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
            this.$router.back('login');
        },
        onClickChild: function (value) {
            this.chosenCard = true // someValue
            console.log(card);

            this.chosenCard = card;
            //doorsturen naar backend
        },
        // sendChosenCard: function (value){
        //     console.log("nu zou de card moeten doorgestuurd worden" + value);
        //       this.$router.push({ path: `/leaderboard` }) // -> /user/123
        // },
        startCountdownRound: function(){
            this.roundTimerId = setInterval(this.roundCountdown, this.roundTimerId, this.roundVisualCounter, "start-chooser-round");
        },
        startCountdownChooser: function(){
            this.roundTimerId = setInterval(this.chooserCountdown, this.chooserTimerId, this.chooserVisualCounter, "send-chosen-card");
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
            this.cards = hand;
            console.log(hand);
        },
        GameRound: function() {
            // TODO
            // 1. Tell the backend to initiate a round
            //      Picks a Czar 
            //      Picks a black card for the round
            // 1. Get Roles from Backend (Czar/Pleb)
            // 2. Ask the black
        }
    },
    created(){
        this.$gameHub.$on("receive-starting-hand", this.ReceiveStartingHand);
        // Get Starting hand
        this.$gameHub.GetStartingHand();
        
    }
}
</script>
