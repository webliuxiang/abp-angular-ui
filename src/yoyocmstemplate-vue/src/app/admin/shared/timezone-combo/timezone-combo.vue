<template>
    <a-select :defaultValue="selectedTimeZoneVal" @change="selectChange($event)">
        <a-select-option v-for="item in timeZones" :key="item.value" :value="item.value">{{ item.name }}</a-select-option>
    </a-select>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { TimingServiceProxy } from "@/shared/service-proxies";

export default {
    name: "timezone-combo",
    mixins: [AppComponentBase],
    props: ["selectedTimeZone", "defaultTimezoneScope"],
    data() {
        return {
            timingService: null,
            timeZones: [],
            selectedTimeZoneVal: this.selectedTimeZone
        };
    },
    created() {
        this.timingService = new TimingServiceProxy(this.$apiUrl, this.$api);
    },
    mounted() {
        this.timingService
            .getTimezones(this.defaultTimezoneScope)
            .then(result => {
                this.timeZones = result.items;
            });
    },
    methods: {
        selectChange(val) {
            this.$emit("selectedTimeZoneChange", val);
        }
    },
    watch: {}
};
</script>

<style scoped>
</style>
