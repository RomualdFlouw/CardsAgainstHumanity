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
      <h2 class="login_errorMessage">{{ errorMessage }}</h2>
    </div>
  </div>
</template>

<script>
export default {
  name: "login",
  data() {
    return {
      nickname: null,
      errorMessage: null
    };
  },
  components: {},
  methods: {
    login: function(nickname) {
      console.log(nickname);
      if (nickname == null || nickname == "") {
        console.log("is empty");
        this.errorMessage = "The input field is empty, please type a username.";
      } else {
        this.$router.push({ path: `/home/${this.nickname}` }); // -> /user/123
        
        this.errorMessage = null;
        this.$lobbyHub.LoginUser({ Name: nickname, ReadyState: false });
      }
    },
    UsernameChecked: function(isAvailable){
      if (isAvailable) {
        this.errorMessage = null;
        this.$router.push({ path: `/home/${this.nickname}` }); // -> /user/123
      } else {
        console.log("username already taken");
        this.errorMessage = "This username is already in use.";
      }
    },
    LobbyFull: function(){
      console.log("Lobby is full");
        this.errorMessage = "The lobby is full or game has already started.";
    }

  },
  created() {
    console.log("reset");
    this.$store.dispatch("loginToken");
    this.$lobbyHub.$on("username-checked", this.UsernameChecked);
    this.$lobbyHub.$on("lobby-full", this.LobbyFull);
  }
};
</script>
