import axios from 'axios';


const httpClient = axios.create({
  baseURL: '',
  timeout: 300000
});

export default httpClient;
