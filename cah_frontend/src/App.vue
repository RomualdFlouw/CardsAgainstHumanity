<template>
  <router-view></router-view>
</template> 

<script>
import Container from './components/Layout/Container';
import Row from './components/Layout/Row';

export default {
  components: {
    Container,
    Row
  }
}
</script>

<style lang="scss">
@import '@/assets/style/screen';
</style>
