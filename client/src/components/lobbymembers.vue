<template>
    <div class="lobby_member">
        <h2 class="lobby_member__name">{{username}}</h2>
        <svg v-show="!isReady"  xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle></svg>
        <svg v-show="isReady"  xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#ffffff" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>
    </div>
</template>

<script>
export default {
    name:'lobbymembers',

    data(){
        return{
        }
    },
    props:{
        username: String,
        isReady: Boolean,
    },
};
</script>