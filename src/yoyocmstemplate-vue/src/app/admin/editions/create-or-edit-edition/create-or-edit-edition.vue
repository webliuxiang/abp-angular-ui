<template>
    <div>
        <div class="modal-header">
            <div class="modal-title" v-if="!editionId">{{l('CreateNewEdition')}}</div>
            <div class="modal-title" v-if="editionId">
                {{l('EditEdition')}}:
                <span>{{edition.displayName}}</span>
            </div>
        </div>
        <a-form :form="form" @submit="save()" autocomplete="off">
            <a-tabs @change="callback" defaultActiveKey="1">
                <a-tab-pane :tab="l('EditionProperties')" key="1">
                    <a-form-item
                            :label="l('EditionName')"
                    >
                        <a-input
                                type="text"
                                v-decorator="['displayName', {
                                    rules: [{ required: true, message: l('ThisFieldIsRequired') },
                                            { required: true, message: l('MaxLength'),max:64  },
                                            ]
                                 }]"
                        />
                    </a-form-item>
                </a-tab-pane>
                <a-tab-pane :tab="l('Features')" forceRender key="2">
                    <div v-if="editData">
                        <edition-feature-tree :editData="editData" ref="featureTree"></edition-feature-tree>
                    </div>
                </a-tab-pane>
            </a-tabs>
        </a-form>

        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                {{l("Cancel")}}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="save()">
                <i class="acticon acticon-save"></i>
                <span>{{l("Save")}}</span>
            </a-button>
        </div>
    </div>
</template>
<script>
    import {ModalComponentBase} from "@/shared/component-base";
    import {EditionFeatureTree} from '@/app/admin/shared';
    import {AppEditionExpireAction} from '@/abpPro/AppEnums';
    import {
        CommonLookupServiceProxy,
        CreateOrUpdateEditionDto,
        EditionEditDto,
        EditionServiceProxy
    } from '@/shared/service-proxies';

    export default {
        name: "create-or-edit-edition",
        mixins: [ModalComponentBase],
        components: {EditionFeatureTree},
        data() {
            return {
                editionService: null,
                commonLookupService: null,

                form: this.$form.createForm(this),
                editData: null,

                edition: undefined,
                editionId: undefined,
                expiringEditions: [],
                expireAction: undefined,
                expireActionEnum: AppEditionExpireAction,
                isFree: true,
                isTrialActive: false,
                isWaitingDayActive: false,
            };
        },
        created() {
            this.fullData(); // 模态框必须,填充数据到data字段

            this.edition = new EditionEditDto();

            this.editionService = new EditionServiceProxy(this.$apiUrl, this.$api);
            this.commonLookupService = new CommonLookupServiceProxy(this.$apiUrl, this.$api);

            this.show(this.editionId);
        },
        computed: {
            featureTree() {
                return this.$refs.featureTree;
            }
        },
        methods: {
            callback(e) {

            },
            show(editionId) {
                this.commonLookupService
                    .getEditionsForCombobox(true)
                    .then(editionsResult => {
                        this.expiringEditions = editionsResult.items;

                        this.editionService
                            .getEditionForEdit(editionId)
                            .then(editionResult => {
                                this.edition = editionResult.edition;
                                this.editData = editionResult;

                                this.form.setFieldsValue({
                                    displayName: this.edition.displayName
                                });

                                this.expireAction =
                                    this.edition.expiringEditionId > 0
                                        ? AppEditionExpireAction.AssignToAnotherEdition
                                        : AppEditionExpireAction.DeactiveTenant;

                                this.isFree =
                                    !editionResult.edition.monthlyPrice &&
                                    !editionResult.edition.annualPrice
                                        ? 'true'
                                        : 'false';
                                this.isTrialActive = editionResult.edition.trialDayCount > 0;
                                this.isWaitingDayActive =
                                    editionResult.edition.waitingDayAfterExpire > 0;
                            });
                    });
            },
            updateAnnualPrice(value) {
                this.edition.annualPrice = value;
            },

            updateMonthlyPrice(value) {
                this.edition.monthlyPrice = value;
            },

            resetPrices(isFree) {
                this.edition.annualPrice = undefined;
                this.edition.monthlyPrice = undefined;
            },

            removeExpiringEdition(isDeactivateTenant) {
                this.edition.expiringEditionId = null;
            },

            save() {

                this.edition.displayName=this.form.getFieldValue('displayName');

                const input = new CreateOrUpdateEditionDto();
                input.edition = this.edition;
                input.featureValues = this.featureTree.getGrantedFeatures();
                this.saving = true;
                this.editionService
                    .createOrUpdateEdition(input)
                    .finally(() => {
                        this.saving = false;
                    })
                    .then(() => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.saving = false;
                        this.success();
                    });
            }
        }
    };


</script>
