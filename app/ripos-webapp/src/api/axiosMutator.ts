import api from './axiosConfig';
import { AxiosRequestConfig, AxiosResponse } from 'axios';

// Define a generic type for your mutator
const customMutator = async <T = unknown, D = unknown>(
  config: AxiosRequestConfig<D>
): Promise<AxiosResponse<T>> => {
  const { url, method, params, data, ...rest } = config;
  return api.request<T>({ url, method, params, data, ...rest });
};

export default customMutator;
