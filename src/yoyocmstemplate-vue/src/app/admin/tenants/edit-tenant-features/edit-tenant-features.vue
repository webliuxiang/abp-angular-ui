<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">{{l('EditTenantFeatures')}}</div>
        </div>
        <edition-feature-tree :editData="editData" ref="featureTree"></edition-feature-tree>
    </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { EditionFeatureTree } from "@/app/admin/shared";
import { EditionServiceProxy } from "@/shared/service-proxies";

export default {
    name: "common-lookup",
    mixins: [ModalComponentBase],
    data() {
        return {
            spinning: false,
            editData: {}
            // // 输入框
            // filterText: "",
            // // 表格
            // columns: [
            //     {
            //         title: this.l("Select"),
            //         dataIndex: "select",
            //         key: "select",
            //         width: 150,
            //         scopedSlots: { customRender: "select" }
            //     },
            //     {
            //         title: this.l("Name"),
            //         dataIndex: "name",
            //         key: "name"
            //     }
            // ],
            // tableData: [],
            // // 分页
            // page: 1,
            // pageSize: 10,
            // totalCount: 0
        };
    },
    components: { EditionFeatureTree },
    created() {
        this._editionService = new EditionServiceProxy(this.$apiUrl, this.$api);
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {
        console.log(this.tenantId);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._editionService
                .getEditionForEdit(this.tenantId)
                .finally(() => {
                    this.spinning = false;
                })
                .then(editionResult => {
                    console.log(editionResult);
                    // this.edition = editionResult.edition;
                    this.editData = editionResult;

                    // this.form.setFieldsValue({
                    //     displayName: this.edition.displayName
                    // });

                    // this.expireAction =
                    //     this.edition.expiringEditionId > 0
                    //         ? AppEditionExpireAction.AssignToAnotherEdition
                    //         : AppEditionExpireAction.DeactiveTenant;

                    // this.isFree =
                    //     !editionResult.edition.monthlyPrice &&
                    //     !editionResult.edition.annualPrice
                    //         ? "true"
                    //         : "false";
                    // this.isTrialActive =
                    //     editionResult.edition.trialDayCount > 0;
                    // this.isWaitingDayActive =
                    //     editionResult.edition.waitingDayAfterExpire > 0;
                });
        }
        // /**
        //  * 搜索
        //  */
        // refresh() {
        //     this.pageSize = 10;
        //     this.page = 1;
        //     this.getData();
        // },
        // /**
        //  * 分页
        //  */
        // onShowSizeChange(current, pageSize) {
        //     this.pageSize = pageSize;
        //     this.getData();
        // },
        // onChange(page, pageSize) {
        //     this.page = page;
        //     this.getData();
        // },
        // /**
        //  * 选择
        //  */
        // selectItem(item) {
        //     const boolOrPromise = this.options.canSelect(item);
        //     console.log(item);
        //     // if (!boolOrPromise) {
        //     //   return;
        //     // }
        //     // if (boolOrPromise === true) {
        //     //   this.itemSelected.emit(item);
        //     //   this.success(item);
        //     //   return;
        //     // }
        //     // // assume as observable
        //     // (boolOrPromise as Observable<boolean>).subscribe(result => {
        //     //   if (result) {
        //     //     this.itemSelected.emit(item);
        //     //     this.success(item);
        //     //   }
        //     // });
        // }
    }
};
</script>

<style lang="less" scoped>
// .lookup-table {
//     margin: 20px auto;
// }
// .pagination {
//     text-align: right;
// }
</style>
