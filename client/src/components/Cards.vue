<template>
    <div v-on:click="ChooseCard(card)" class="cards_white">
        <h3 class="cards_title_black">{{card.cardText}}</h3>
    </div>   
</template>

<script>
export default {
    name:'cards',

    data(){
        return{
        }
    },
    props:{
        card: {
            cardText: String,
            cardIndex: Number,
        }
    },
    methods:{
        ChooseCard: function(index){
            this.$emit('clicked', index)
        }
    }
};
</script>