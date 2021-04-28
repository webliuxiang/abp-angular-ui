import AppCompoentBase from './app-component-base';
import {AppConsts} from '@/abpPro/AppConsts';


const PagedListingComponentBase = {
    mixins: [AppCompoentBase],
    data() {
        return {
            pageSize: AppConsts.grid.defaultPageSize,
            pageNumber: 1,
            totalPages: 1,
            totalItems: 0,
            isTableLoading: true,
            allChecked: false,
            allCheckboxDisabled: false,
            checkboxIndeterminate: false,
            selectedDataItems: [],
            sorting: undefined,
            filterText: '',
            dataList: [],
            booleanFilterList: [
                {text: this.l('All'), value: 'All'},
                {text: this.l('Yes'), value: true},
                {text: this.l('No'), value: false},
            ],
            selectedRowKeys:[]
        }
    },
    mounted() {
        this.refresh();
    },
    computed: {},
    methods: {
        refresh() { // 刷新表格数据
            this.restCheckStatus(this.dataList);
            this.getDataPage(this.pageNumber);
        },
        refreshGoFirstPage() { // 刷新表格数据并跳转到第一页（`pageNumber = 1`）
            this.pageNumber = 1;
            this.restCheckStatus(this.dataList);
            this.getDataPage(this.pageNumber);
        },
        getDataPage(page) { // 请求分页数据
            let req = {
                maxResultCount: 0,
                skipCount: 0,
                sorting: null,
            };
            req.maxResultCount = this.pageSize;
            req.skipCount = (page - 1) * this.pageSize;
            req.sorting = this.sorting;
            this.isTableLoading = true;
            this.fetchDataList(req, page, () => {
                this.isTableLoading = false;
                // 更新全选框禁用状态
                this.refreshAllCheckBoxDisabled();
            });
        },
        refreshAllCheckBoxDisabled() {  // 刷新全选框是否禁用
            this.allCheckboxDisabled = this.dataList.length <= 0;
        },
        pageNumberChange() { // 页码发生改变
            if (this.pageNumber > 0) {
                this.refresh();
            }
        },
        checkAll(value) { // 选中全部记录
            this.dataList.forEach(data => data.checked = this.allChecked);
            this.refreshCheckStatus(this.dataList);
        },
        refreshCheckStatus(entityList) { // 刷新选中状态
            // // 是否全部选中
            // const allChecked = entityList.every(value => value.checked === true);
            // // 是否全部未选中
            // const allUnChecked = entityList.every(value => !value.checked);
            // // 是否全选
            // this.allChecked = allChecked;
            // // 全选框样式控制
            // this.checkboxIndeterminate = !allChecked && !allUnChecked;
            // // 已选中数据
            // this.selectedDataItems = entityList.filter(value => value.checked);

            this.selectedRowKeys = [];
        },
        restCheckStatus(entityList) { // 重置选中状态
            // if (!Array.isArray(entityList)) {
            //     entityList = this.dataList;
            // }
            // this.allChecked = false;
            // this.checkboxIndeterminate = false;
            // // 已选中数据
            // this.selectedDataItems = [];
            // // 设置数据为未选中状态
            // entityList.forEach(value => (value.checked = false));

            this.selectedRowKeys = [];
        },
        showPaging(result) { // 计算分页
            this.totalItems = result.totalCount;
            this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        gridSort(sort) { // 数据表格排序
            let ascOrDesc = sort.value; // 'ascend' or 'descend' or null
            const filedName = sort.key; // 字段名
            if (ascOrDesc) {
                ascOrDesc = abp.utils.replaceAll(ascOrDesc, 'end', '');
                const args = ['{0} {1}', filedName, ascOrDesc];
                const sortingStr = abp.utils.formatString.apply(this, args);
                if (this.sorting === sortingStr) {
                    return;
                }
                this.sorting = sortingStr;
            } else {
                this.sorting = undefined;
            }
            this.refresh();
        },
        tableChange(pagination, filters, sorter, data) { // antd-vue table change事件
            // 调用排序
            sorter['value'] = sorter.order;
            sorter['key'] = sorter.columnKey;
            this.gridSort(sorter);
        },
        onSelectChange(keys) { // antd-vue table selectChange事件
            this.selectedRowKeys = keys;
        },
        fetchDataList(request, pageNumber, finishedCallback) { // 获取数据抽象接口，必须实现
            console.log('请重写 fetchDataList 函数');
        }
    }
};

export default PagedListingComponentBase;

