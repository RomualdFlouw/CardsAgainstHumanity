<template>
  <container>
    <router-view></router-view>
  </container>
</template> 

<script>
import Container from './components/Layout/Container';
import Row from './components/Layout/Row';

export default {
  created(){
    this.$store.dispatch("restoreContext");
  },
  components: {
    Container,
    Row
  }
}
</script>

<style lang="scss">
@import '@/assets/style/screen';
</style>
