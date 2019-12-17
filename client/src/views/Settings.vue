<template>
    <div>
        <div class="home_header">
            <router-link class="header_logout" to="/">
                <svg xmlns="http://www.w3.org/2000/svg" width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="#FF0000" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
            </router-link>
        </div>
      <h1 class="logo">{{$t("SETTINGS")}}</h1>
    <div class="content">
        <select class="settings_select" name="LeaveType" @change="onChange($event)" v-model="key">
            <option class="settings_select__child" value="en">english</option>
            <option class="settings_select__child" value="nl">nederlands</option>
        </select> 
    </div>
    </div>
</template>
<script>
// @ is an alias to /src

export default {
  name: 'settings', 
  data() {
    return{
      key: ""
    }
  },
  components: {
  },
  methods:{
      onChange: function(event){
        console.log(event.target.value);
        localStorage.setItem('locale', event.target.value)
        this.$i18n.locale = event.target.value;
      }
  },

  created() {},
}
</script>

<style scoped>
.settings_select{
  width: 80%;
  margin: auto;
  max-width: 320px;

  background-color: transparent;
  border: 2px solid white;
  color: white
}

</style>