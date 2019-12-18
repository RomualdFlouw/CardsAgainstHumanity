<template>
  <div class="full_size">
      <div class="header">
        <router-link class="header_logout" to="/settings">
          <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="3"></circle><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"></path></svg>
        </router-link>
      </div>
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
      <h3 class="login_errorMessage">{{ errorMessage }}</h3>
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
        localStorage.CurrentUser = this.nickname;
        this.$router.push({ path: "home" }); // -> /user/123
      } else {
        this.errorMessage = this.$t('LOGIN_ERROR_USING');
      }
    },
    LobbyFull: function(){
        this.errorMessage = this.$t('LOGIN_ERROR_FULL');
    },
    GoToSettings: function(){
          this.$router.push('settings');
        },

  },
  created() {
      this.$store.dispatch("loginToken");
      this.$lobbyHub.$on("username-checked", this.UsernameChecked);
      this.$lobbyHub.$on("lobby-full", this.LobbyFull);
    }
};
</script>
