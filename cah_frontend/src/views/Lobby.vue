<template>
    <div class="lobby">
        <h1 class="logo">Cards Against Humanity</h1>
        <div class="content">
            <input v-on:click="logoutFunction" type="submit" class="button" value="test" >
        </div>
    </div>
</template>
<script>
export default {
    name: 'login',
    data(){
        return{
            logout: null
        }
    },
    methods:{
        logoutFunction: function(){
            this.$store.dispatch('logout');
            this.$router.back('login')
        }
    }
}
</script>