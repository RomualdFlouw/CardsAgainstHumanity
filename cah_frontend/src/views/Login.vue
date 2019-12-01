<template>
    <div class="login">
        <h1 class="logo">Cards Against Humanity</h1>
        <div class="content">
            <input  v-model="nickname" class="login_input" type="text" placeholder="Type your nickname" id="nickname">    
            <input v-on:click="login(nickname)" class="button" type="submit" value="Send" >
        </div>
    </div>
</template>

<script>
export default {
  name: 'login',
  data(){
      return{
        nickname: null,
      }
  },
  components: {
      
  },
  methods:{
      login: function(nickname){
          console.log(nickname);
          if(nickname == null){
              console.log("is empty");
          }else{
              this.$store.dispatch("loginToken", {UserName : nickname});
              this.$router.push('lobby')
          }
      }
  }
}
</script>