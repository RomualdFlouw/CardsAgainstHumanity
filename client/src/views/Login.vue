<template>
  <div class="login">
    <h1 class="logo">{{$t("GAME_TITLE")}}</h1>
    <div class="content">
      <input
        v-model="nickname"
        class="login_input"
        type="text"
        :placeholder="`${this.$t('INPUT_PLACEHOLDER')}`"
        id="nickname"
      />
      <input
        v-on:click="login(nickname)"
        class="button"
        type="submit"
        :value="`${this.$t('INPUT_BUTTON_TEXT')}`"
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
        this.errorMessage = this.$t('LOGIN_ERROR_EMPTY');
      } else {
        this.errorMessage = null;
        // this.$router.push({ path: "Leaderboard" }); // -> /user/123
        this.$lobbyHub.LoginUser({ Name: nickname, ReadyState: false });
      }
    },
    UsernameChecked: function(isAvailable){
      if (isAvailable) {
        this.errorMessage = null;
        this.$router.push({ path: "home" }); // -> /user/123
      } else {
        this.errorMessage = this.$t('LOGIN_ERROR_USING');
      }
    },
    LobbyFull: function(){
        this.errorMessage = this.$t('LOGIN_ERROR_FULL');
    }

  },
  created() {
    try {
      this.$store.dispatch("loginToken");
      this.$lobbyHub.$on("username-checked", this.UsernameChecked);
      this.$lobbyHub.$on("lobby-full", this.LobbyFull);
    } catch (err) {
      Sentry.captureException(err);
    }
    }
};
</script>
