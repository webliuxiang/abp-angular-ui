<template>
    <a-select
        :allowClear="true"
        :placeholder="placeholder"
        :value="selectedEditionIndex"
        @change="selectChange($event)">
        <a-select-option
            v-for="(item,index) of editions"
            :key="index"
            :value="index">{{item.displayText}}</a-select-option>
    </a-select>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { EditionServiceProxy } from "@/shared/service-proxies";

export default {
    name: "edition-combo",
    mixins: [AppComponentBase],
    props: ["placeholder", "selectedEdition", "editionId"],
    data() {
        return {
            editionService: null, // 版本信息api
            editions: [], // 所有的版本信息
            editionIdVal: this.editionId, // 版本id
            selectedEditionVal: this.selectedEdition,
            selectedEditionIndex: null // 选中版本信息的索引
        };
    },
    created() {
        this.editionService = new EditionServiceProxy(this.$apiUrl, this.$api);
    },
    mounted() {
        this.editionService
            .getEditionComboboxItems(0, true, false)
            .then(editions => {
                this.editions = editions;
                this.autoSelect();
            });
    },
    computed: {},
    methods: {
        autoSelect() {
            if (!this.editions) {
                return;
            }

            let callSelectChange = false;

            if (this.selectedEditionVal) {
                this.selectedEditionIndex = this.editions.findIndex(item => {
                    return (
                        item.value === this.selectedEditionVal.value.toString()
                    );
                });
                callSelectChange = true;
            } else if (this.editionIdVal) {
                this.selectedEditionIndex = this.editions.findIndex(item => {
                    return item.value === this.editionIdVal.toString();
                });
                callSelectChange = true;
            }

            if (callSelectChange) {
                this.selectChange(this.editions[this.selectedEditionIndex]);
            }
        },
        selectChange(e) {
            if (typeof e === "object") {
                this.$emit("selectedEditionChange", e);
                return;
            }

            this.selectedEditionIndex = e;

            if (e >= 0) {
                this.$emit(
                    "selectedEditionChange",
                    this.editions[this.selectedEditionIndex]
                );
                return;
            }

            this.$emit("selectedEditionChange", null);
        }
    },
    watch: {
        selectedEdition(val) {
            this.selectedEditionVal = val;
            this.autoSelect();
        },
        editionId(val) {
            this.editionIdVal = val;
            this.autoSelect();
        }
    }
};
</script>

<style lang="less" scoped>
.ant-select {
    width: 100%;
}
</style>