<template>
<div class="lobby">
    <div >
        <lobbymembers  v-for="p in players" :key="p.id" :username="p.username" :isReady="p.isReady"/>
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
            players:[
                {username: "jasper", isReady: false},
                {username: "Romuald", isReady: true},
                {username: "kevin", isReady: true},
                {username: "ayron", isReady: true},
                {username: "thibault", isReady: true},
                {username: "debrycke", isReady: false},
            ],
            isReady: false
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
            this.isReady = !this.isReady;
            localStorage.setItem('readyState', this.isReady);
            this.$router.push({ path: `/game/${this.nickname}` })
      }
    },
    created(){
        localStorage.setItem('readyState', false);
    }
}
</script>