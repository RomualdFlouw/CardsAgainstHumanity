<template>
    <div v-on:click="ChooseCard(cardIndex)" class="cards_white">
        <h3 class="cards_title_black">{{cardText}}</h3>
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
        cardText: String,
        cardIndex: Number,
    },
    methods:{
        ChooseCard: function(index){
            this.$emit('clicked', index)
        }
    }
};
</script>