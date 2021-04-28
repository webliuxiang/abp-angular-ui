import { AppConsts } from '@/abpPro/AppConsts';
import { abpService } from '@/shared/abp';
import { localizationService } from '@/shared/i18n';
import { Modal } from 'ant-design-vue';
import axios from 'axios';


const apiHttpClient = axios.create({
  baseURL: AppConsts.remoteServiceBaseUrl,
  timeout: 300000
});

// request interceptor
apiHttpClient.interceptors.request.use(function (config) {
  if (!!abpService.abp.auth.getToken()) {
    config.headers.common.Authorization = 'Bearer ' + abpService.abp.auth.getToken() || '';
  }
  config.headers.common['.AspNetCore.Culture'] = abpService.abp.utils.getCookieValue('Abp.Localization.CultureName');
  config.headers.common['Abp.TenantId'] = abpService.abp.multiTenancy.getTenantIdCookie() || '';
  return config;

}, function (error) {

  return Promise.reject(error);
});


// response error interceptor
apiHttpClient.interceptors.response.use((response) => {
  if (response.data.__abp) {
    response.data = response.data.result;
  }
  return response;
}, (error) => {
  if (!!error.response && !!error.response.data.error && !!error.response.data.error.message && error.response.data.error.details) {
    Modal.error({
      title: error.response.data.error.message,
      content: error.response.data.error.details
    });
  } else if (!!error.response && !!error.response.data.error && !!error.response.data.error.message) {
    Modal.error({
      title: localizationService.l('LoginFailed'),
      content: error.response.data.error.message
    });
  } else if (!error.response) {
    Modal.error({
      content: localizationService.l('UnknownError')
    });
  }

  return Promise.reject(error);
});

export default apiHttpClient;
