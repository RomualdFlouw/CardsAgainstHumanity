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
                    <h3>points: {{currentPoints}}</h3>
                    <h3>you are {{currentPosition}}</h3>
                </div>
                <h2 class="information_player">{{currentUser}} is the card czar</h2>
                <h3>time left: {{currentTimer}}</h3>
            </div>
            <div class="game_content__placement">
                <div class="placement_black">
                    <h3 class="cards_title_white">{{blackCardValue}}</h3>
                </div>
                <div v-show="!chosenCard" class="placement_dotted">
                    <h3 class="cards_title_white">choose your card</h3>
                </div>                
                <div v-show="chosenCard" class="placement_white">
                    <h3 class="cards_title_black">{{chosenCardText}}</h3>
                </div>
            </div>
            <input v-show="chosenCard" v-on:click="sendChosenCard(chosenCardText)" class="game_content__submit" type="submit" value="confirm"> 
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

export default {
    name: 'game',
    data(){
        return{
            currentPoints: 5,
            currentPosition: "2nd",
            currentUser: "Jasper",
            currentTimer: "50s",
            blackCardValue: "What is the one thing obama is good at? ___",
            cards:[
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
            chosenCard: false,
            chosenCardText: ""
        }
    },
    components:{
      cards
    },
    methods:{
        logoutFunction: function(){
            this.$store.dispatch('logout');
            this.$router.back('login');
            window.localStorage.removeItem('readyState');
        },
        onClickChild: function (value) {
            this.chosenCard = true // someValue
            this.chosenCard = true // someValue
            this.chosenCardText = this.cards[value].cardText // someValue
        },
        sendChosenCard: function (value){
            console.log("nu zou de card moeten doorgestuurd worden" + value);
              this.$router.push({ path: `/leaderboard` }) // -> /user/123
        }
    }
}
</script>
