<template>
    <a-spin :spinning="spinning">
        <!-- 鼠标进入触发加载 -->
        <a-cascader
            :options="provinces"
            :load-data="loadData"
            expand-trigger="hover"
            :placeholder="placeHolder" />
        <!-- 点击触发加载 -->
        <p class="cascader-margin"></p>
        <a-cascader
            :options="provinces"
            :load-data="loadData"
            :placeholder="mouseclickplaceHolder" />
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { AddressLinkageServiceProxy } from "@/shared/service-proxies";

export default {
    name: "x-address-linkage",
    mixins: [AppComponentBase],
    props: {
        placeHolder: {
            type: String
        },
        mouseclickplaceHolder: {
            type: String
        }
    },
    components: {},
    data() {
        return {
            spinning: false,
            _addressLinkageServiceProxy: "",
            // 省
            provinces: [],
            // 市
            citys: [],
            // 区
            areas: [],
            // 街道
            streets: []
        };
    },
    created() {
        this._addressLinkageServiceProxy = new AddressLinkageServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._addressLinkageServiceProxy
                .getAll()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    res.provinces.forEach(element => {
                        this.provinces.push({
                            value: element.code,
                            label: element.name,
                            isLeaf: false,
                            index: 0
                        });
                    });
                    res.citys.forEach(element => {
                        this.citys.push({
                            value: element.code,
                            label: element.name,
                            provinceCode: element.provinceCode,
                            isLeaf: false,
                            index: 1
                        });
                    });
                    res.areas.forEach(element => {
                        this.areas.push({
                            value: element.code,
                            label: element.name,
                            provinceCode: element.provinceCode,
                            cityCode: element.cityCode,
                            isLeaf: false,
                            index: 2
                        });
                    });
                    res.streets.forEach(element => {
                        this.streets.push({
                            value: element.code,
                            label: element.name,
                            provinceCode: element.provinceCode,
                            cityCode: element.cityCode,
                            areaCode: element.areaCode,
                            isLeaf: true,
                            index: 3
                        });
                    });
                    console.log(this.provinces);
                });
        },
        /**
         * 动态加载数据
         */
        loadData(selectedOptions) {
            console.log(selectedOptions);
            const targetOption = selectedOptions[selectedOptions.length - 1];
            console.log(targetOption);
            targetOption.loading = true;
            setTimeout(() => {
                targetOption.loading = false;
                if (targetOption.index === 0) {
                    let selectCitys = this.citys.filter(
                        x => x.provinceCode === targetOption.value
                    );
                    targetOption.children = selectCitys;
                } else if (targetOption.index === 1) {
                    let selectAreas = this.areas.filter(
                        x => x.cityCode === targetOption.value
                    );
                    targetOption.children = selectAreas;
                } else {
                    let selectStreet = this.streets.filter(
                        x => x.areaCode == targetOption.value
                    );
                    targetOption.children = selectStreet;
                }
                this.provinces = [...this.provinces];
            }, 500);
        }
    }
};
</script>

<style scoped lang="less">
.cascader-margin {
    margin-top: 10px;
}
</style>
