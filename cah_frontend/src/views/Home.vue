<template>
    <div class="main_setup center_vertical">
      <div></div>
      <div class="main_content">
          <h1 class="setup_header__text">Cards Against Humanity</h1>
          <lobby></lobby>
      </div>
      <div></div>
  </div>
</template>
<script>
// @ is an alias to /src
import nickname from '@/components/Layout/Nickname.vue'
import avatar from '@/components/Layout/Avatar.vue'
import lobby from '@/components/Layout/Lobby.vue'
import game from '@/components/Layout/Game.vue'
import leaderboard from '@/components/Layout/Leaderboard.vue'
import chat from '@/components/Layout/Chat.vue'

export default {
  name: 'home',
  components: {
     nickname,
     avatar,
     lobby,
     game,
     leaderboard,
     chat
  }
}
</script>
