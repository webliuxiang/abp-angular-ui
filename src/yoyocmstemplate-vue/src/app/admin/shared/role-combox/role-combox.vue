<template>
    <a-select
        mode="multiple"
        :dropdownStyle="dropDownStyle"
        :placeholder="l('FilterByRole')"
        :defaultValue="selectedRoleVal"
        @change="selectedChange($event)"
        allowClear
        style="width: 100%;">
        <a-select-option :key="index" :value="role.id" v-for="(role,key,index) of roles">
            {{role.displayName}}
        </a-select-option>

    </a-select>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { RoleServiceProxy } from "@/shared/service-proxies";

export default {
    name: "role-combox",
    mixins: [AppComponentBase],
    props: ["dropDownStyle", "selectMode", "selectedRole"],
    data() {
        return {
            roleService: null,
            roles: [],
            selectedRoleVal: this.selectedRole
        };
    },
    created() {
        this.roleService = new RoleServiceProxy(this.$apiUrl, this.$api);
    },
    mounted() {
        this.roleService.getAll(undefined).then(result => {
            this.roles = result.items;
        });
    },
    methods: {
        selectedChange(selectKey) {
            this.$emit("selectedRoleChange", selectKey);
        }
    },
    watch: {}
};
</script>

<style scoped>
</style>
