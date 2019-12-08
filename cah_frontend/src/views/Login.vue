<template>
  <div class="login">
    <h1 class="logo">Cards Against Humanity</h1>
    <div class="content">
      <input
        v-model="nickname"
        class="login_input"
        type="text"
        placeholder="Type your nickname"
        id="nickname"
      />
      <input
        v-on:click="login(nickname)"
        class="button"
        type="submit"
        value="Send"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "login",
  data() {
    return {
      nickname: null
    };
  },
  components: {},
  methods: {
    login: function(nickname) {
      console.log(nickname);
      if (nickname == null || nickname == "") {
        console.log("is empty");
      } else {
        this.$lobbyHub.LoginUser({ Name: nickname, ReadyState: false });
      }
    },
    UsernameChecked: function(isAvailable){
      if (isAvailable) {
        this.$router.push({ path: `/home/${this.nickname}` }); // -> /user/123
      } else {
        console.log("username already taken");
        // TODO 
        // WRITE CODE TO DISPLAY THAT USERNAME IS TAKEN ON THE WEBSITE
      }
    },
    LobbyFull: function(){
      console.log("Lobby is full");
    }

  },
  created() {
    this.$store.dispatch("loginToken");
    this.$lobbyHub.$on("username-checked", this.UsernameChecked);
    this.$lobbyHub.$on("lobby-full", this.LobbyFull);
  }
};
</script>
