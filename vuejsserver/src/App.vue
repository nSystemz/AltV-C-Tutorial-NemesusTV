<template>
  <div id="app">
    <Inventory ref="inventory"/>
    <Garage ref="garage"/>
    <MDC ref="mdc"/>
  </div>
</template>

<script>
import Inventory from './components/Inventory.vue'
import Garage from './components/Garage.vue'
import MDC from './components/MDC.vue'

export default {
  name: 'App',
  components: {
    Inventory,
    Garage,
    MDC
  },
  mounted() {
    global.gui.inventory = this.$refs.inventory;
    global.gui.garage = this.$refs.garage;
    global.gui.mdc = this.$refs.mdc;
  }
}
</script>

<style>
@import './assets/adminlte/dist/css/adminlte.css';
@import './assets/adminlte/plugins/fontawesome-free/css/all.css';
</style>
