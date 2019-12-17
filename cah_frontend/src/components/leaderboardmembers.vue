<template>
    <div class="leaderboard_member">
        <h3 class="leaderboard_placement">{{ position }}</h3>
        <h3>{{ username }}</h3>
        <h3 class="leaderboard_points">{{ points }}/10</h3>
    </div>
</template>

<script>
export default {
    name:'leaderboardmembers',

    data(){
        return{
        }
    },
    props:{
        position: Number,
        username: String,
        points: Number,
    },
};
</script>

<style lang="scss" scoped>
    .leaderboard_placement{
        text-align: start;
    }
    .leaderboard_points{
        text-align: end;
    }
</style>