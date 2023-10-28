<template>
    <div v-if="showGarage" class="row float-right" style="background: transparent; margin-right: 0.2vw; margin-top: 10vw;">
        <div class="col-4 d-flex justify-content-between align-items center">
            <div class="list-group" id="list-tab" role="tablist">
                <a class="list-group-item list-group-item-action active" style="color: white">Garage</a>
                <a class="list-group-item list-group-item-action"><strong>Fahrzeug einparken</strong></a>
                <a class="list-group-item list-group-item-action">Infernus</a>
                <a class="list-group-item list-group-item-action">Infernus</a>
                <a class="list-group-item list-group-item-action">Infernus</a>
                <a class="list-group-item list-group-item-action">Infernus</a>
                <a class="list-group-item list-group-item-action">Infernus</a>
                <a class="list-group-item list-group-item-action">Infernus</a>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
    name: 'Garage',
    data() {
        return {
            showGarage: false,
            garageVehicles: [],
        }
    },
    methods: {
        showHideGarage: function(json) {
            this.showGarage = !this.showGarage;
            if(this.showGarage)
            {
                this.garageVehicles = JSON.parse(json);
            }
        }
    },
}
</script>

<style scoped>

</style>
