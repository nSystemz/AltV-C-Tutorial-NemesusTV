<template>
  <div id="app">
    <Inventory ref="inventory"/>
  </div>
</template>

<script>
import Inventory from './components/Inventory.vue'

export default {
  name: 'App',
  components: {
    Inventory,
  },
  mounted() {
    global.gui.inventory = this.$refs.inventory;
  }
}
</script>

<style>
</style>
