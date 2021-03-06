import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TransformDataService {
  transformData(message: any) {
    // console.log(message);
    let chartData = {
      tableHeader: [],
      tablerows: [],
    };
    let aggregationConf = JSON.parse(message.originData.chart_opts.originAgg);
    let chartAxisValue = null;
    let chartAxisType = null;
    let hasXAxis = false;
    let axisArr = [];
    let axisFields = [];
    let chartRenderData = [];
    let chartFormatData = [];
    switch (message.originData.chart_type) {
      case 'tableChart':
        chartData.tableHeader = [];
        chartData.tablerows = [];
        // table header
        message.responseData.headers.bucket.map(resbucketItem => {
          message.originData.aggregation.buckets.map(oribucketItem => {
            if (resbucketItem.name.toString() === oribucketItem.name.toString()) {
              chartData.tableHeader.push({
                field: oribucketItem.type === 'filters' ? oribucketItem.type : oribucketItem.field,
                unit: oribucketItem.calendar_interval,
                name: oribucketItem.name,
                label: oribucketItem.label,
              });
            }
          });
        });
        message.responseData.headers.metric.map(resmetricItem => {
          message.originData.aggregation.metrics.map(orimetricItem => {
            if (resmetricItem.name.toString() === orimetricItem.name.toString()) {
              chartData.tableHeader.push({
                field: orimetricItem.type,
                unit: '',
                name: orimetricItem.name,
                label: orimetricItem.label,
              });
            }
          });
        });

        message.responseData.rows.map(rowItem => {
          let rowData = [];
          message.responseData.headers.bucket.map(resbucketItem => {
            rowData.push({
              result: rowItem.bucket[resbucketItem.name].key,
              result_as_string: rowItem.bucket[resbucketItem.name].key_as_string,
            });
          });
          message.responseData.headers.metric.map(resmetricItem => {
            rowData.push({
              result: rowItem.metric[resmetricItem.name].value,
              result_as_string: rowItem.metric[resmetricItem.name].value_as_string,
            });
          });
          chartData.tablerows.push(rowData);
        });

        break;
      case 'lineChart':
        /**
         * ???????????????????????? key value ?????????
         * ????????? X ???
         * ?????????????????? Bucket ???????????? X ??????????????????????????? X ???
         * ??????????????? Bucket ???????????? X ??????????????????????????????????????????????????? X ??????????????????????????? X ???
         */
        if (message.responseData.headers.bucket.length === 0) return;
        // if (message.responseData.headers.bucket.length === 1) {
        //   chartAxisValue = this.setBucketName(message.originData.aggregation.buckets[0]);
        //   chartAxisType = aggregationConf.bucket[0].bucketsSetting.fields.type;
        // } else {

        message.responseData.headers.bucket.map((resbucketItem, index) => {
          if (aggregationConf.bucket[index].seriesType === 'xAxis') {
            hasXAxis = true;
            chartAxisValue = this.setBucketName(message.originData.aggregation.buckets[index]);
            chartAxisType = aggregationConf.bucket[index].bucketsSetting.fields.type;
          }
        });
        if (!hasXAxis) {
          chartAxisValue = 'all_docs';
          chartAxisType = 'noXAxis';
        }
        // }
        // console.log(chartAxisValue);
        chartData.tableHeader = [];
        chartData.tableHeader.push({ xAxis: chartAxisValue, type: chartAxisType });

        message.responseData.rows.map(rowItem => {
          let rowData = {
            seriesData: [],
            metricsData: [],
            xAxisData: [],
          };
          message.responseData.headers.bucket.map((resbucketItem, index) => {
            let dataKey = this.setBucketName(message.originData.aggregation.buckets[index]);
            // console.log(resbucketItem);

            if (dataKey === chartAxisValue) {
              rowData.xAxisData.push({
                [dataKey]: rowItem.bucket[resbucketItem.name].key
                  ? rowItem.bucket[resbucketItem.name].key
                  : rowItem.bucket[resbucketItem.name].key_as_string,
              });
            } else {
              rowData.seriesData.push({
                [dataKey]:
                  resbucketItem.label +
                  ' ' +
                  (rowItem.bucket[resbucketItem.name].key
                    ? rowItem.bucket[resbucketItem.name].key
                    : rowItem.bucket[resbucketItem.name].key_as_string),
              });
            }
          });
          message.responseData.headers.metric.map((resmetricItem, index) => {
            let dataKey = this.setMetricName(message.originData.aggregation.metrics[index]);
            rowData.metricsData.push({
              [dataKey]: rowItem.metric[resmetricItem.name].value
                ? rowItem.metric[resmetricItem.name].value
                : rowItem.metric[resmetricItem.name].value_as_string,
            });
          });
          // console.log(rowData);
          // ??????????????????????????????
          let objTemp = {};
          rowData.metricsData.map(metricItem => {
            let stringTemp = '';
            rowData.seriesData.map((seriesItem, index) => {
              for (const key in seriesItem) {
                if (Object.prototype.hasOwnProperty.call(seriesItem, key)) {
                  const element = seriesItem[key];
                  stringTemp += (index < 1 ? '' : '-') + element;
                }
              }
            });
            for (const key in rowData.xAxisData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.xAxisData[0], key)) {
                const element = rowData.xAxisData[0][key];
                objTemp[key] = element;
              }
            }
            for (const key in rowData.seriesData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.seriesData[0], key)) {
                objTemp[key] = stringTemp;
              }
            }
            for (const key in metricItem) {
              if (Object.prototype.hasOwnProperty.call(metricItem, key)) {
                const element = metricItem[key];
                objTemp[stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key] = element;
                axisArr.push(stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key);
              }
            }
          });
          // console.log(objTemp);
          chartData.tablerows.push(objTemp);
        });
        axisArr = Array.from(new Set(axisArr));
        chartData.tableHeader.push({ allAxis: axisArr });

        // let test = this.groupBy(chartData.tablerows, link => {
        //   return [link[chartAxisValue]];
        // });
        // console.log(test);
        /**
         * ????????? series ???????????? [{ip_keyword: '1.1.1.1'},{memory: 3333}] ?????? {ip_keyword: '1.1.1.1', memory: 3333}
         * ????????? metrics ???????????? [{max: 999},{min: 111}] ?????? {max: 999, min: 111}
         * ????????? [{'1.1.1.1': 999},{'1.1.1.1': 111},{'3333': 999},{'3333': 111}] ?????? {'1.1.1.1': 999, '1.1.1.1': 111, '3333': 999, '3333': 111} ??????????????????
         */
        break;
      case 'pieChart':
        /**
         * ????????????????????? metric
         * ???????????????3??? bucket
         * ?????? X ?????????
         * ???????????????
         */
        if (message.responseData.headers.bucket.length === 0) return;

        message.responseData.rows.map(rowItem => {
          let rowData = {
            seriesData: [],
            metricsData: [],
            xAxisData: [],
          };
          message.responseData.headers.bucket.map((resbucketItem, index) => {
            let dataKey = this.setBucketName(message.originData.aggregation.buckets[index]);
            let objTemp = {
              [dataKey]:
                resbucketItem.label +
                ' ' +
                (rowItem.bucket[resbucketItem.name].key
                  ? rowItem.bucket[resbucketItem.name].key
                  : rowItem.bucket[resbucketItem.name].key_as_string),
            };
            rowData.seriesData.push(objTemp);
          });
          message.responseData.headers.metric.map((resmetricItem, index) => {
            let dataKey = this.setMetricName(message.originData.aggregation.metrics[index]);
            rowData.metricsData.push({
              [dataKey]: rowItem.metric[resmetricItem.name].value
                ? rowItem.metric[resmetricItem.name].value
                : rowItem.metric[resmetricItem.name].value_as_string,
            });
          });
          chartFormatData.push(rowData);

          let objTemp = {};
          rowData.seriesData.map(seriesItem => {
            for (const key in seriesItem) {
              if (Object.prototype.hasOwnProperty.call(seriesItem, key)) {
                const element = seriesItem[key];
                objTemp[key] = element;
                axisArr.push(key);
              }
            }
          });
          for (const key in rowData.metricsData[0]) {
            if (Object.prototype.hasOwnProperty.call(rowData.metricsData[0], key)) {
              const element = rowData.metricsData[0][key];
              objTemp[key] = element * 1;
              chartAxisValue = key;
            }
          }
          chartData.tablerows.push(objTemp);
        });
        chartData.tableHeader = [{ xAxis: chartAxisValue }, { allAxis: Array.from(new Set(axisArr)) }];
        // console.log(chartFormatData);
        // console.log(chartRenderData);
        break;
      case 'barChart':
        if (message.responseData.headers.bucket.length === 0) return;

        message.responseData.headers.bucket.map((resbucketItem, index) => {
          if (aggregationConf.bucket[index].seriesType === 'xAxis') {
            hasXAxis = true;
            chartAxisValue = this.setBucketName(message.originData.aggregation.buckets[index]);
            chartAxisType = aggregationConf.bucket[index].bucketsSetting.fields.type;
          }
        });
        if (!hasXAxis) {
          chartAxisValue = 'all_docs';
          chartAxisType = 'noXAxis';
        }
        // console.log(chartAxisValue);
        chartData.tableHeader = [];
        chartData.tableHeader.push({ xAxis: chartAxisValue, type: chartAxisType });

        message.responseData.rows.map(rowItem => {
          let rowData = {
            seriesData: [],
            metricsData: [],
            xAxisData: [],
          };
          message.responseData.headers.bucket.map((resbucketItem, index) => {
            let dataKey = this.setBucketName(message.originData.aggregation.buckets[index]);
            if (dataKey === chartAxisValue) {
              rowData.xAxisData.push({
                [dataKey]: rowItem.bucket[resbucketItem.name].key
                  ? rowItem.bucket[resbucketItem.name].key
                  : rowItem.bucket[resbucketItem.name].key_as_string,
              });
            } else {
              rowData.seriesData.push({
                [dataKey]:
                  resbucketItem.label +
                  ' ' +
                  (rowItem.bucket[resbucketItem.name].key
                    ? rowItem.bucket[resbucketItem.name].key
                    : rowItem.bucket[resbucketItem.name].key_as_string),
              });
            }
          });
          message.responseData.headers.metric.map((resmetricItem, index) => {
            let dataKey = this.setMetricName(message.originData.aggregation.metrics[index]);
            rowData.metricsData.push({
              [dataKey]: rowItem.metric[resmetricItem.name].value
                ? rowItem.metric[resmetricItem.name].value
                : rowItem.metric[resmetricItem.name].value_as_string,
            });
          });
          chartFormatData.push(rowData);

          let objTemp = {};

          rowData.metricsData.map(metricItem => {
            let stringTemp = '';
            rowData.seriesData.map((seriesItem, index) => {
              for (const key in seriesItem) {
                if (Object.prototype.hasOwnProperty.call(seriesItem, key)) {
                  const element = seriesItem[key];
                  stringTemp += (index < 1 ? '' : '-') + element;
                }
              }
            });

            for (const key in rowData.xAxisData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.xAxisData[0], key)) {
                const element = rowData.xAxisData[0][key];
                objTemp[key] = element;
              }
            }
            for (const key in rowData.seriesData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.seriesData[0], key)) {
                objTemp[key] = stringTemp;
              }
            }
            for (const key in metricItem) {
              if (Object.prototype.hasOwnProperty.call(metricItem, key)) {
                const element = metricItem[key] * 1;
                objTemp[stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key] = element;
                axisArr.push(stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key);
                chartRenderData.push({
                  type: stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key,
                  // [rowData.xAxisData[0][chartAxisValue]]: element,
                  xAxis: hasXAxis ? rowData.xAxisData[0][chartAxisValue] : 'all_docs',
                  value: element,
                });
                axisFields.push(hasXAxis ? rowData.xAxisData[0][chartAxisValue] : 'all_docs');
              }
            }
          });
          // console.log(objTemp);
          // chartData.tablerows.push({});
        });
        axisArr = Array.from(new Set(axisArr));
        axisFields = Array.from(new Set(axisFields));
        // chartData.tableHeader.push({ allAxis: axisArr });
        chartData.tableHeader.push({ allAxis: axisFields });
        chartData.tablerows = chartRenderData;
        // console.log(chartFormatData);
        // console.log(chartRenderData);

        break;
      case 'areaChart':
        if (message.responseData.headers.bucket.length === 0) return;

        message.responseData.headers.bucket.map((resbucketItem, index) => {
          if (aggregationConf.bucket[index].seriesType === 'xAxis') {
            hasXAxis = true;
            chartAxisValue = this.setBucketName(message.originData.aggregation.buckets[index]);
            chartAxisType = aggregationConf.bucket[index].bucketsSetting.fields.type;
          }
        });
        if (!hasXAxis) {
          chartAxisValue = 'all_docs';
          chartAxisType = 'noXAxis';
        }
        // console.log(chartAxisValue);
        chartData.tableHeader = [];
        chartData.tableHeader.push({ xAxis: chartAxisValue, type: chartAxisType });

        message.responseData.rows.map(rowItem => {
          let rowData = {
            seriesData: [],
            metricsData: [],
            xAxisData: [],
          };
          message.responseData.headers.bucket.map((resbucketItem, index) => {
            let dataKey = this.setBucketName(message.originData.aggregation.buckets[index]);
            if (dataKey === chartAxisValue) {
              rowData.xAxisData.push({
                [dataKey]: rowItem.bucket[resbucketItem.name].key
                  ? rowItem.bucket[resbucketItem.name].key
                  : rowItem.bucket[resbucketItem.name].key_as_string,
              });
            } else {
              rowData.seriesData.push({
                [dataKey]:
                  resbucketItem.label +
                  ' ' +
                  (rowItem.bucket[resbucketItem.name].key
                    ? rowItem.bucket[resbucketItem.name].key
                    : rowItem.bucket[resbucketItem.name].key_as_string),
              });
            }
          });
          message.responseData.headers.metric.map((resmetricItem, index) => {
            let dataKey = this.setMetricName(message.originData.aggregation.metrics[index]);
            rowData.metricsData.push({
              [dataKey]: rowItem.metric[resmetricItem.name].value
                ? rowItem.metric[resmetricItem.name].value
                : rowItem.metric[resmetricItem.name].value_as_string,
            });
          });
          chartFormatData.push(rowData);

          let objTemp = {};

          rowData.metricsData.map(metricItem => {
            let stringTemp = '';
            rowData.seriesData.map((seriesItem, index) => {
              for (const key in seriesItem) {
                if (Object.prototype.hasOwnProperty.call(seriesItem, key)) {
                  const element = seriesItem[key];
                  stringTemp += (index < 1 ? '' : '-') + element;
                }
              }
            });

            for (const key in rowData.xAxisData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.xAxisData[0], key)) {
                const element = rowData.xAxisData[0][key];
                objTemp[key] = element;
              }
            }
            for (const key in rowData.seriesData[0]) {
              if (Object.prototype.hasOwnProperty.call(rowData.seriesData[0], key)) {
                objTemp[key] = stringTemp;
              }
            }
            for (const key in metricItem) {
              if (Object.prototype.hasOwnProperty.call(metricItem, key)) {
                const element = metricItem[key] * 1;
                objTemp[stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key] = element;
                axisArr.push(stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key);
                chartRenderData.push({
                  type: stringTemp + (rowData.seriesData.length < 1 ? '' : ': ') + key,
                  // [rowData.xAxisData[0][chartAxisValue]]: element,
                  xAxis: hasXAxis ? rowData.xAxisData[0][chartAxisValue] : 'all_docs',
                  value: element,
                });
                axisFields.push(hasXAxis ? rowData.xAxisData[0][chartAxisValue] : 'all_docs');
              }
            }
          });
          // console.log(objTemp);
          // chartData.tablerows.push({});
        });
        axisArr = Array.from(new Set(axisArr));
        axisFields = Array.from(new Set(axisFields));
        // chartData.tableHeader.push({ allAxis: axisArr });
        chartData.tableHeader.push({ allAxis: axisFields });
        chartData.tablerows = chartRenderData;
        // console.log(chartFormatData);
        // console.log(chartRenderData);

        break;
    }
    return chartData;
  }
  // ?????? Bucket ??????
  setBucketName(obj) {
    let name = null;
    switch (obj.type) {
      case 'date_histogram':
        name = obj.field + ' per ' + obj.calendar_interval;
        break;
      case 'terms':
        if (obj.order_direction === 'desc') {
          name = obj.field + ' ??????';
        } else if (obj.order_direction === 'asc') {
          name = obj.field + ' ??????';
        }
        break;
      case 'filters':
        break;
    }
    return name;
  }
  // ?????? Metric ??????
  setMetricName(obj) {
    // console.log(obj);
    let name = null;
    if (obj.label === '') {
      switch (obj.type) {
        case 'count':
          name = 'Count';
          break;
        case 'min':
          name = 'Min ' + obj.field;
          break;
        case 'max':
          name = 'Max ' + obj.field;
          break;
        case 'avg':
          name = 'Average ' + obj.field;
          break;
        case 'sum':
          name = 'Sum ' + obj.field;
          break;
      }
    } else {
      name = obj.label;
    }

    return name;
  }
  // Group_By
  groupBy(array, f) {
    let groups = {};
    array.forEach(function (o) {
      let group = JSON.stringify(f(o));
      groups[group] = groups[group] || [];
      groups[group].push(o);
    });
    return Object.keys(groups).map(function (group) {
      return groups[group];
    });
  }
}
