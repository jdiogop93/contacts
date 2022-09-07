<template>
  <div>
    <router-view></router-view>
  </div>
</template>
<script>
import {
  mapMutations,
} from 'vuex'

export default {
  data () {
    return {}
  },
  created () {
    this.setModule('contacts.name')
  },
  methods: {
    ...mapMutations('app', ['setModule']),
  },
}

</script>
